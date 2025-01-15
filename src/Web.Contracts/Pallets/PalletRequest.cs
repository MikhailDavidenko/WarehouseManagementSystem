namespace WarehouseManagementSystem.Web.Contracts.Pallets;

/// <summary>
/// Тело запроса для REST для создания/изменения паллеты
/// </summary>
public sealed class PalletRequest
{
    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Depth { get; set; }
}
