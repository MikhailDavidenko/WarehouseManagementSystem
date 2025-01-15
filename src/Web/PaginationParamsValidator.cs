using FluentValidation;
using WarehouseManagementSystem.Web.Contracts;

namespace WarehouseManagementSystem.Web;

public sealed class PaginationParamsValidator : AbstractValidator<PaginationParams>
{
    public PaginationParamsValidator()
    {
        RuleFor(x => x.Limit).GreaterThan(0);
        RuleFor(x => x.Offset).GreaterThan(0);
    }
}
