using WarehouseManagementSystem.Common;

namespace WarehouseManagementSystem.Domain;

/// <summary>
/// Класс, представляющий коробку
/// </summary>
public sealed class Box : StorageItem
{
    private const int ExpirationFromProductionDays = 100;

    /// <summary>
    /// Конструктор коробки.
    /// </summary>
    /// <param name="id">Уникальный идентификатор коробки.</param>
    /// <param name="width">Ширина коробки.</param>
    /// <param name="height">Высота коробки.</param>
    /// <param name="depth">Глубина коробки.</param>
    /// <param name="weight">Вес коробки.</param>
    /// <param name="palletId">Идентификатор паллеты, на которой находится коробка.</param>
    /// <param name="expirationDate">Дата окончания срока годности коробки.</param>
    /// <param name="productionDate">Дата производства коробки.</param>
    /// <exception cref="BusinessLogicException">Дата производства и дата окончания срока годности не могут быть равны null одновременно.</exception>
    private Box(
        Guid id,
        double width,
        double height,
        double depth,
        double weight,
        Guid palletId,
        DateTime? expirationDate = null,
        DateTime? productionDate = null)
        : base(id, width, height, depth, weight)
    {
        PalletId = palletId;
        ProductionDate = productionDate;
        ExpirationDate = expirationDate
            ?? productionDate?.AddDays(ExpirationFromProductionDays)
            ?? throw new BusinessLogicException("Дата производства и дата окончания срока годности не могут быть равны null одновременно");
    }

    public Guid PalletId { get; private set; }

    public Pallet? Pallet { get; private set; }

    public DateTime? ExpirationDate { get; private set; } = null;

    public DateTime? ProductionDate { get; private set; } = null;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Box"/> с заданными параметрами.
    /// </summary>
    /// <param name="width">Ширина коробки.</param>
    /// <param name="height">Высота коробки.</param>
    /// <param name="depth">Глубина коробки.</param>
    /// <param name="weight">Вес коробки.</param>
    /// <param name="palletId">Идентификатор паллеты, на которой находится коробка.</param>
    /// <param name="expirationDate">Дата окончания срока годности коробки.</param>
    /// <param name="productionDate">Дата производства коробки.</param>
    /// <exception cref="BusinessLogicException">Дата производства и дата окончания срока годности не могут быть равны null одновременно.</exception>
    public static Box Create(
        double width,
        double height,
        double depth,
        double weight,
        Guid palletId,
        DateTime? expirationDate = null,
        DateTime? productionDate = null)
    {
        ValidateDimensions(width, height, depth, weight);

        return new Box(Guid.NewGuid(), width, height, depth, weight, palletId, expirationDate, productionDate);
    }

    /// <summary>
    /// Метод, валидирующий новые размеры коробки и изменяющий её размер
    /// </summary>
    /// <param name="width">Новая ширина коробки</param>
    /// <param name="height">Новая высота коробки</param>
    /// <param name="depth">Новая глубина коробки</param>
    /// /// <param name="weight">Новый вес коробки</param>
    /// <exception cref="ArgumentException">Если новые размеры коробки меньше или равны нулю</exception>
    public void ChangeDimensionsAndWeight(
        double width,
        double height,
        double depth,
        double weight)
    {
        ValidateDimensions(width, height, depth, weight);

        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;
    }

    /// <summary>
    /// Метод, изменяющий идентификатор паллеты, на которой находится коробка
    /// </summary>
    /// <param name="palletId">Новый идентификатор паллеты</param>
    public void MoveToAnotherPallet(Guid palletId)
    {
        PalletId = palletId;
    }

    /// <summary>
    /// Метод, вычисляющий объем коробки
    /// </summary>
    /// <returns>Объем коробки</returns>
    public override double CalculateVolume()
    {
        return Width * Height * Depth;
    }
}
