using WarehouseManagementSystem.Business.Boxes;
using WarehouseManagementSystem.Data.Engine;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Data.Boxes;

internal sealed class DbBoxRepository : IBoxRepository
{
    private readonly DataContext context;

    public DbBoxRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task AddBoxToPalletAsync(
        Box box,
        CancellationToken cancellationToken = default)
    {
        await context.Boxes.AddAsync(box, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateBoxAsync(
        Box box,
        CancellationToken cancellationToken = default)
    {
        context.Boxes.Update(box);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBoxAsync(
        Guid boxId,
        CancellationToken cancellationToken = default)
    {
        var box = context.Boxes.FirstOrDefault(p => p.Id == boxId);

        if (box != null)
        {
            context.Boxes.Remove(box);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
