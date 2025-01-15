using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.Web.ManualClient.Boxes;
using WarehouseManagementSystem.Web.ManualClient.Pallets;

namespace WarehouseManagementSystem.Web.ManualClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManualClient(
        this IServiceCollection services)
    {
        services.AddOptions<WebClientOptions>()
            .BindConfiguration(WebClientOptions.OptionsKey);
        services.AddHttpClient<IBoxClient, BoxClient>();
        services.AddHttpClient<IPalletClient, PalletClient>();
        return services;
    }
}
