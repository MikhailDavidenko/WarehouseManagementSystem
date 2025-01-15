using FluentValidation;
using WarehouseManagementSystem.Web.Contracts.Boxes;

namespace WarehouseManagementSystem.Web.Boxes;

public sealed class BoxRequestValidator : AbstractValidator<BoxRequest>
{
    public BoxRequestValidator()
    {
        RuleFor(x => x.Height).NotNull().GreaterThan(0);
        RuleFor(x => x.Depth).NotNull().GreaterThan(0);
        RuleFor(x => x.Width).NotNull().GreaterThan(0);
        RuleFor(x => x.Weight).NotNull().GreaterThan(0);
    }
}
