using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Data.Engine;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Box> Boxes => Set<Box>();
    public DbSet<Pallet> Pallets => Set<Pallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
