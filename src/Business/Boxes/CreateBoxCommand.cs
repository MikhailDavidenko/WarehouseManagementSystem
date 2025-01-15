namespace WarehouseManagementSystem.Business.Boxes;

public sealed record CreateBoxCommand(
    double Width,
    double Height,
    double Depth,
    double Weight,
    DateTime? ProductionDate,
    DateTime? ExpirationDate);
