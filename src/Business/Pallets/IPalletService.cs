using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Business.Pallets;

public interface IPalletService
{
    Task<Pallet> GetPalletAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Pallet>> GetPalletsWithPaginationAsync(int limit, int offset, CancellationToken cancellationToken = default);

    Task<Pallet> CreatePalletAsync(CreatePalletCommand palletCommand, CancellationToken cancellationToken = default);

    Task UpdatePalletAsync(Guid id, UpdatePalletCommand palletCommand, CancellationToken cancellationToken = default);

    Task DeletePalletAsync(Guid id, CancellationToken cancellationToken = default);
}
