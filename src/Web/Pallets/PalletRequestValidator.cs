using FluentValidation;
using WarehouseManagementSystem.Web.Contracts.Pallets;

namespace WarehouseManagementSystem.Web.Pallets;

public sealed class PalletRequestValidator : AbstractValidator<PalletRequest>
{
    public PalletRequestValidator()
    {
        RuleFor(x => x.Height).NotNull().GreaterThan(0);
        RuleFor(x => x.Depth).NotNull().GreaterThan(0);
        RuleFor(x => x.Width).NotNull().GreaterThan(0);
    }
}
