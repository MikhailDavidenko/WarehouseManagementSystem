namespace WarehouseManagementSystem.Domain;
public sealed class Pallet : StorageItem, IEquatable<object>
{
    private const double BaseWeight = 30; // Вес паллеты без коробок

    private List<Box> boxes = new List<Box>();

    private Pallet(Guid id, double width, double height, double depth)
        : base(id, width, height, depth, BaseWeight)
    {
    }

    public IReadOnlyList<Box> Boxes => boxes;

    public override double Weight => BaseWeight + Boxes.Sum(b => b.Weight);

    public static Pallet Create(double width, double height, double depth)
    {
        if (width <= 0 || height <= 0 || depth <= 0)
        {
            throw new ArgumentException("Размеры должны быть больше 0");
        }

        return new Pallet(Guid.NewGuid(), width, height, depth);
    }

    public override double CalculateVolume()
    {
        return Width * Height * Depth + Boxes.Sum(b => b.CalculateVolume());
    }

    public DateTime? GetEarliestExpirationDate()
    {
        if (Boxes.Count == 0)
        {
            return null;
        }
        return Boxes.Min(b => b.ExpirationDate);
    }

    public bool CanFitBox(Box box)
    {
        return box.Width <= Width && box.Depth <= Depth;
    }

    public bool CanFitBox(double width, double depth)
    {
        return width <= Width && depth <= Depth;
    }

    public void AddBox(Box box)
    {
        if (CanFitBox(box))
        {
            box.MoveToAnotherPallet(Id);
            boxes.Add(box);
        }
        else
        {
            throw new ArgumentException("Размеры коробки превышают вместимость поддона");
        }
    }

    public void Resize(double width, double height, double depth)
    {
        ValidateDimensions(width, height, depth, Weight);

        if (!CanFitAllBoxesInNewSize(width, depth))
        {
            throw new ArgumentException($"Новые размеры паллеты({width}x{height}x{depth}) не вмещают коробки на ней");
        }

        Width = width;
        Height = height;
        Depth = depth;
    }

    public override string ToString()
    {
        return $"Паллет ID: {Id}, Объём: {CalculateVolume()}, Дата окончания срока годности: " +
                $"{GetEarliestExpirationDate():dd.MM.yyyy}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Pallet)obj;

        return Id == other.Id &&
                Width == other.Width &&
                Height == other.Height &&
                Depth == other.Depth &&
                Weight == other.Weight;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Width, Height, Depth, Weight);
    }

    private bool CanFitAllBoxesInNewSize(double width, double depth)
    {
        foreach (var box in boxes)
        {
            if (box.Width > width || box.Depth > depth)
            {
                return false;
            }
        }

        return true;
    }

    private Guid? SelectNotFittingBoxId(IReadOnlyList<Box> boxes)
    {
        return boxes.FirstOrDefault(b => !CanFitBox(b))?.Id;
    }
}
