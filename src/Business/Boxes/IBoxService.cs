using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Business.Boxes;

public interface IBoxService
{
    Task<Box> AddBoxToPalletAsync(Guid palletId, CreateBoxCommand boxCommand, CancellationToken cancellationToken = default);

    Task UpdateOnPalletAsync(Guid palletId, Guid boxId, UpdateBoxCommand boxCommand, CancellationToken cancellationToken = default);

    Task DeleteBoxAsync(Guid id, CancellationToken cancellationToken = default);
}
