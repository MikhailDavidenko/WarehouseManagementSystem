using WarehouseManagementSystem.Common;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Business.Pallets;

internal sealed class PalletService : IPalletService
{
    private readonly IPalletRepository palletRepository;

    public PalletService(IPalletRepository palletRepository)
    {
            this.palletRepository = palletRepository;
    }

    public async Task<Pallet> GetPalletAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await palletRepository.GetPalletByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException($"Паллеты с {id} не найдено");
    }

    public Task<IReadOnlyList<Pallet>> GetPalletsWithPaginationAsync(
        int limit,
        int offset,
        CancellationToken cancellationToken = default)
    {
        return palletRepository.GetPalletsWithPaginationAsync(offset, limit, cancellationToken);
    }

    public async Task<Pallet> CreatePalletAsync(
        CreatePalletCommand palletCommand,
        CancellationToken cancellationToken = default)
    {
        var pallet = Pallet.Create(
            palletCommand.Width,
            palletCommand.Height,
            palletCommand.Depth
            );

        await palletRepository.AddPalletAsync(pallet, cancellationToken);

        return pallet;
    }

    public async Task UpdatePalletAsync(
        Guid id,
        UpdatePalletCommand palletCommand,
        CancellationToken cancellationToken = default)
    {
        var pallet = await palletRepository.GetPalletByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException($"Паллеты с Id {id} не найдено");

        pallet.Resize(
            palletCommand.Width,
            palletCommand.Height,
            palletCommand.Depth
            );

        await palletRepository.UpdatePalletAsync(pallet, cancellationToken);
    }

    public async Task DeletePalletAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await palletRepository.DeleteByIdAsync(id, cancellationToken);
    }
}
