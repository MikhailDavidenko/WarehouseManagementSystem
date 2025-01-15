namespace WarehouseManagementSystem.Business.Boxes;

public sealed record UpdateBoxCommand(
    double Width,
    double Height,
    double Depth,
    double Weight,
    DateTime? ProductionDate,
    DateTime? ExpirationDate);
