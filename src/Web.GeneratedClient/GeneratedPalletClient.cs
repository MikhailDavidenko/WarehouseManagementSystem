using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WarehouseManagementSystem.Web.GeneratedClient;

internal partial class GeneratedPalletClient
{
    [ActivatorUtilitiesConstructor]
    public GeneratedPalletClient(HttpClient httpClient, IOptions<WebClientOptions> options)
    {
        _baseUrl = options.Value.RequiredServerUrl.ToString();
    }
}
