using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.Business.Boxes;
using WarehouseManagementSystem.Business.Pallets;

namespace WarehouseManagementSystem.Business;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует бизнес-сервисы
    /// </summary>
    public static IServiceCollection AddBusinessServices(
        this IServiceCollection services)
        => services
            .AddTransient<IPalletService, PalletService>()
            .AddTransient<IBoxService, BoxService>();
}
