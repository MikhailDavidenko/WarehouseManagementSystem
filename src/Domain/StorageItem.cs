using WarehouseManagementSystem.Common;

namespace WarehouseManagementSystem.Domain;

public abstract class StorageItem
{
    public Guid Id { get; private init; }
    public double Width { get; protected set; }
    public double Height { get; protected set; }
    public double Depth { get; protected set; }
    public virtual double Weight { get; protected set; }

    protected StorageItem(Guid id, double width, double height, double depth, double weight)
    {
        Id = id;
        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;
    }

    public abstract double CalculateVolume();

    protected static void ValidateDimensions(double width, double height, double depth, double weight)
    {
        if (width <= 0 || height <= 0 || depth <= 0 || weight <= 0)
        {
            throw new ArgumentException("Размеры и вес должны быть больше 0");
        }
    }
}
