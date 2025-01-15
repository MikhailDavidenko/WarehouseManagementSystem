using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using WarehouseManagementSystem.Data.Engine;
using WarehouseManagementSystem.Web;

namespace WarehouseManagementSystem.IntegrationTests.Infrastructure;

public sealed class TestWebApplication : WebApplicationFactory<IWebMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer postgreContainer = new PostgreSqlBuilder()
        .WithDatabase("TestWmsDb")
        .WithUsername("wmsuser")
        .WithPassword("password")
        .Build();

    public async Task InitializeAsync()
        => await postgreContainer.StartAsync();

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await postgreContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting($"{DbConnectionOptions.OptionsKey}:{nameof(DbConnectionOptions.Wms)}",
            postgreContainer.GetConnectionString());
        builder.UseSetting($"{DbProviderOptions.OptionsKey}:{nameof(DbProviderOptions.Provider)}",
            DbProviderOptions.DbProvider.Postgre.ToString());
        base.ConfigureWebHost(builder);
    }
}
