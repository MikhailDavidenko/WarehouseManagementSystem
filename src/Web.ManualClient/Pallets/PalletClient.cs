using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Web.Contracts;
using WarehouseManagementSystem.Web.Contracts.Routes;
using WarehouseManagementSystem.Web.Contracts.Pallets;
using Microsoft.Extensions.Options;
using WarehouseManagementSystem.Common;

namespace WarehouseManagementSystem.Web.ManualClient.Pallets;

internal sealed class PalletClient : IPalletClient
{
    private readonly HttpClient client;
    private readonly Uri serverUrl;

    public PalletClient(HttpClient client, IOptions<WebClientOptions> options)
    {
        this.client = client;
        serverUrl = options.Value.RequiredServerUrl;
    }

    public async Task<PalletResponse?> GetPalletByIdAsync(Guid palletId, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.PalletWithPalletId(palletId));
        return await client.GetFromJsonAsync<PalletResponse?>(url, cancellationToken);
    }

    public async Task<IReadOnlyList<PalletResponse>> GetPalletsAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.PalletsWithPagination(paginationParams));
        var response = await client.GetFromJsonAsync<IReadOnlyList<PalletResponse>?>(url, cancellationToken);

        return response ?? Array.Empty<PalletResponse>();
    }

    public async Task<PalletResponse?> CreatePalletAsync(PalletRequest palletRequest, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.Pallets);
        var response = await client.PostAsJsonAsync(url, palletRequest, cancellationToken);
        if (response.IsSuccessStatusCode is true)
        {
            return await response.Content.ReadFromJsonAsync<PalletResponse>(cancellationToken);
        }

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
        throw new WebApiException(problemDetails.Detail, problemDetails);

    }

    public async Task UpdatePalletAsync(Guid palletId, PalletRequest palletRequest, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.PalletWithPalletId(palletId));
        await client.PutAsJsonAsync(url, palletRequest, cancellationToken);
    }

    public async Task DeletePalletAsync(Guid palletId, CancellationToken cancellationToken = default)
    {
        var url = new Uri(serverUrl, ApiV1.Warehouse.PalletWithPalletId(palletId));
        await client.DeleteAsync(url, cancellationToken);
    }
}
