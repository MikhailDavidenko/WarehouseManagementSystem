using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WarehouseManagementSystem.Web.GeneratedClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeneratedClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<WebClientOptions>()
            .BindConfiguration(WebClientOptions.OptionsKey);
        services.AddHttpClient<IGeneratedBoxClient, GeneratedBoxClient>();
        services.AddHttpClient<IGeneratedPalletClient, GeneratedPalletClient>();
        return services;
    }
}
