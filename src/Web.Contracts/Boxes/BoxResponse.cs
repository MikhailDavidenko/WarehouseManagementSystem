namespace WarehouseManagementSystem.Web.Contracts.Boxes;

/// <summary>
/// Ответ для REST для возврата коробки
/// </summary>
public sealed class BoxResponse
{
    public Guid Id { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Depth { get; set; }

    public double Weight { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime? ProductionDate { get; set; }

    public Guid PalletId { get; set; }
}
