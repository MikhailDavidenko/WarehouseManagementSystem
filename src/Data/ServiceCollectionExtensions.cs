using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WarehouseManagementSystem.Business.Boxes;
using WarehouseManagementSystem.Business.Pallets;
using WarehouseManagementSystem.Data.Engine;
using WarehouseManagementSystem.Data.Pallets;
using WarehouseManagementSystem.Data.Boxes;

namespace WarehouseManagementSystem.Data;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует репозитории
    /// </summary>
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
        => services
            .AddTransient<IPalletRepository, DbPalletRepository>()
            .AddTransient<IBoxRepository, DbBoxRepository>();

    /// <summary>
    /// Регистрирует контекст
    /// </summary>
    public static IServiceCollection AddContext(
        this IServiceCollection services)
    {
        services.AddOptions<DbConnectionOptions>().BindConfiguration(DbConnectionOptions.OptionsKey);
        services.AddOptions<DbProviderOptions>().BindConfiguration(DbProviderOptions.OptionsKey);

        services
            .AddDbContext<DataContext>((provider, builder) =>
            {
                var connectionOptions = provider.GetRequiredService<IOptions<DbConnectionOptions>>();
                var migrationOptions = provider.GetRequiredService<IOptions<DbProviderOptions>>();
                switch (migrationOptions.Value.RequiredProvider)
                {
                    case DbProviderOptions.DbProvider.Sqlite:
                        builder.UseSqlite(connectionOptions.Value.RequiredConnectionString,
                            b => b.MigrationsAssembly("Data.Migrations.Sqlite"));
                        break;
                    case DbProviderOptions.DbProvider.Postgre:
                        builder.UseNpgsql(connectionOptions.Value.RequiredConnectionString,
                            b => b.MigrationsAssembly("Data.Migrations.Postgre"));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            });

        return services;
    }
}
