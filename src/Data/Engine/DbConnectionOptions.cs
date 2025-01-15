namespace WarehouseManagementSystem.Data.Engine;

public sealed class DbConnectionOptions
{
    public const string OptionsKey = "ConnectionStrings";

    public string? Wms { get; set; }

    public string RequiredConnectionString => Wms ?? throw new ArgumentNullException(EmptyServerUrlMessage);

    private const string EmptyServerUrlMessage = $"Конфигурационное значение «{nameof(Wms)}» не задано";
}
