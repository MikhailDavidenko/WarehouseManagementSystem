using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Business.Boxes;

public interface IBoxRepository
{
    Task AddBoxToPalletAsync(Box box, CancellationToken cancellationToken = default);

    Task UpdateBoxAsync(Box box, CancellationToken cancellationToken = default);

    Task DeleteBoxAsync(Guid boxId, CancellationToken cancellationToken = default);
}
