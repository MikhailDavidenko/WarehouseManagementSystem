using WarehouseManagementSystem.Web.Contracts.Boxes;

namespace WarehouseManagementSystem.Web.ManualClient.Boxes;

public interface IBoxClient
{
    Task<BoxResponse> CreateBoxOnPalletAsync(Guid palletId, BoxRequest boxRequest, CancellationToken cancellationToken = default);

    Task UpdateBoxOnPalletAsync(Guid palletId, Guid boxId, BoxRequest boxRequest, CancellationToken cancellationToken = default);

    Task DeleteBoxOnPalletAsync(Guid palletId, Guid boxId, CancellationToken cancellationToken = default);
}
