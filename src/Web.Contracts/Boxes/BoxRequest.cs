namespace WarehouseManagementSystem.Web.Contracts.Boxes;

/// <summary>
/// Тело запроса для REST для создания/изменения коробки
/// </summary>
public sealed class BoxRequest
{
    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Depth { get; set; }

    public double? Weight { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime? ProductionDate { get; set; }
}
