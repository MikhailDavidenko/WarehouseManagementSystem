using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Data.Pallets;

public sealed class PalletEntityConfiguration : IEntityTypeConfiguration<Pallet>
{
    public void Configure(EntityTypeBuilder<Pallet> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.Boxes)
            .WithOne(b => b.Pallet)
            .HasForeignKey(b => b.PalletId);
    }
}
