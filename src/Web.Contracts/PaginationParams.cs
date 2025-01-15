namespace WarehouseManagementSystem.Web.Contracts;

public sealed record PaginationParams
(
    int? Offset,
    int? Limit
);
