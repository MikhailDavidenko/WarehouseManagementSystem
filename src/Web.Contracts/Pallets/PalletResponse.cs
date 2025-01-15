using WarehouseManagementSystem.Web.Contracts.Boxes;

namespace WarehouseManagementSystem.Web.Contracts.Pallets;

/// <summary>
/// Ответ для REST для возврата паллет с коробками
/// </summary>
public sealed class PalletResponse
{
    public Guid Id { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Depth { get; set; }

    public double Weight { get; set; }

    public IReadOnlyList<BoxResponse> Boxes { get; set; } = Array.Empty<BoxResponse>();
}
