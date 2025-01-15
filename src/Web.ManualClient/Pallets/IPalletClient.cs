using WarehouseManagementSystem.Web.Contracts;
using WarehouseManagementSystem.Web.Contracts.Pallets;

namespace WarehouseManagementSystem.Web.ManualClient.Pallets;

public interface IPalletClient
{
    Task<PalletResponse?> GetPalletByIdAsync(Guid palletId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PalletResponse>> GetPalletsAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default);

    Task<PalletResponse?> CreatePalletAsync(PalletRequest palletRequest, CancellationToken cancellationToken = default);

    Task UpdatePalletAsync(Guid palletId, PalletRequest palletRequest, CancellationToken cancellationToken = default);

    Task DeletePalletAsync(Guid palletId, CancellationToken cancellationToken = default);
}
