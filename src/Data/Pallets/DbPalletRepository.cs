using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Business.Pallets;
using WarehouseManagementSystem.Data.Engine;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Data.Pallets;

internal sealed class DbPalletRepository : IPalletRepository
{
    private readonly DataContext context;

    public DbPalletRepository(DataContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Возвращает паллету по Id
    /// </summary>
    public Task<Pallet?> GetPalletByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return context.Pallets
            .Include(p => p.Boxes)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Возвращает список паллетов
    /// </summary>
    public async Task<IReadOnlyList<Pallet>> GetPalletsAsync(CancellationToken cancellationToken = default)
    {
        return await context.Pallets
            .Include(p => p.Boxes)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Возвращает список паллетов с пагинацией
    /// </summary>
    /// <param name="offset">С какого элемента начинать выборку. По-дефолту 0</param>
    /// <param name="limit">Сколько элементов выбрать. По-дефолту 10</param>
    public async Task<IReadOnlyList<Pallet>> GetPalletsWithPaginationAsync(
        int offset = 0,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        return await context.Pallets
            .Include(p => p.Boxes)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Pallet>> GetPalletsSortedByExpirationAndWeightAsync(CancellationToken cancellationToken = default)
    {
        return await context.Pallets
            .Include(p => p.Boxes)
            .Where(p => p.Boxes.Any())
            .OrderBy(p => p.Weight)
            .ThenBy(pallet => pallet.Boxes.Min(b => b.ExpirationDate))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<Pallet>> GetTopThreeWithLatestExpirationDateSortedByWeightAsync(CancellationToken cancellationToken = default)
    {
        var pallets = await context.Pallets
            .Include(p => p.Boxes)
            .OrderBy(p => p.Boxes.Max(b => b.ExpirationDate))
            .Take(3)
            .ToListAsync(cancellationToken);

        return pallets
            .OrderBy(p => p.CalculateVolume())
            .ToList();
    }

    public async Task AddPalletAsync(
        Pallet pallet,
        CancellationToken cancellationToken = default)
    {
        await context.Pallets.AddAsync(pallet, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdatePalletAsync(
        Pallet pallet,
        CancellationToken cancellationToken = default)
    {
        context.Pallets.Update(pallet);
        return context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(
        Guid palletId,
        CancellationToken cancellationToken = default)
    {
        var pallet = await context.Pallets.FirstOrDefaultAsync(p => p.Id == palletId, cancellationToken);

        if (pallet != null)
        {
            context.Pallets.Remove(pallet);
            await context.SaveChangesAsync(cancellationToken);
        }        
    }
}
