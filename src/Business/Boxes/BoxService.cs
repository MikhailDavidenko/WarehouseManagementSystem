using WarehouseManagementSystem.Business.Pallets;
using WarehouseManagementSystem.Common;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Business.Boxes;

internal sealed class BoxService : IBoxService
{
    private readonly IBoxRepository boxRepository;
    private readonly IPalletRepository palletRepository;

    public BoxService(IBoxRepository boxRepository, IPalletRepository palletRepository)
    {
        this.boxRepository = boxRepository;
        this.palletRepository = palletRepository;
    }

    public async Task<Box> AddBoxToPalletAsync(
        Guid palletId,
        CreateBoxCommand boxCommand,
        CancellationToken cancellationToken = default)
    {
        var pallet = await palletRepository.GetPalletByIdAsync(palletId, cancellationToken)
            ?? throw new EntityNotFoundException($"Паллеты с Id {palletId} не существует");

        if (!pallet.CanFitBox(boxCommand.Width, boxCommand.Depth))
        {
            throw new ArgumentException($"Коробка с параметрами " +
                                        $"{boxCommand.Width}x{boxCommand.Height}x{boxCommand.Depth} не помещается на паллету");
        }

        var box = Box.Create(
            boxCommand.Width,
            boxCommand.Height,
            boxCommand.Depth,
            boxCommand.Weight,
            palletId,
            boxCommand.ExpirationDate,
            boxCommand.ProductionDate);

        await boxRepository.AddBoxToPalletAsync(box, cancellationToken);

        return box;
    }

    public async Task UpdateOnPalletAsync(
        Guid palletId,
        Guid boxId,
        UpdateBoxCommand boxCommand,
        CancellationToken cancellationToken = default)
    {
        var pallet = await palletRepository.GetPalletByIdAsync(palletId, cancellationToken)
            ?? throw new EntityNotFoundException($"Паллеты с Id {palletId} не существует");

        var box = pallet.Boxes.FirstOrDefault(b => b.Id == boxId)
            ?? throw new EntityNotFoundException($"Коробки с Id {palletId} на паллете {palletId} не существует");

        if (!pallet.CanFitBox(boxCommand.Width, boxCommand.Depth))
        {
            throw new ArgumentException($"Коробка c параметрами {boxCommand.Width}x{boxCommand.Depth} не помещается на паллету");
        }

        box.ChangeDimensionsAndWeight(
            boxCommand.Width,
            boxCommand.Height,
            boxCommand.Depth,
            boxCommand.Weight
            );

        await boxRepository.UpdateBoxAsync(box, cancellationToken);
    }

    public async Task DeleteBoxAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await boxRepository.DeleteBoxAsync(id, cancellationToken);
    }
}
