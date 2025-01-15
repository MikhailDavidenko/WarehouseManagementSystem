using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WarehouseManagementSystem.Common;
using WarehouseManagementSystem.Web.Contracts.Boxes;
using WarehouseManagementSystem.Web.Contracts.Routes;

namespace WarehouseManagementSystem.Web.ManualClient.Boxes;

internal sealed class BoxClient : IBoxClient
{
    private readonly HttpClient client;
    private readonly Uri serverUrl;

    public BoxClient(HttpClient client, IOptions<WebClientOptions> options)
    {
        this.client = client;
        serverUrl = options.Value.RequiredServerUrl;
    }

    public async Task<BoxResponse> CreateBoxOnPalletAsync(Guid palletId, BoxRequest boxRequest, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.BoxOnPallet(palletId));
        var response = await client.PostAsJsonAsync(url, boxRequest, cancellationToken);
        if (response.IsSuccessStatusCode is true)
        {
            return await response.Content.ReadFromJsonAsync<BoxResponse>(cancellationToken);
        }

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
        throw new WebApiException(problemDetails.Detail, problemDetails);
    }

    public async Task UpdateBoxOnPalletAsync(Guid palletId, Guid boxId, BoxRequest boxRequest, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.BoxOnPalletWithBoxId(palletId, boxId));
        await client.PutAsJsonAsync(url, boxRequest, cancellationToken);
    }

    public async Task DeleteBoxOnPalletAsync(Guid palletId, Guid boxId, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.BoxOnPalletWithBoxId(palletId, boxId));
        await client.DeleteAsync(url, cancellationToken);
    }
}
