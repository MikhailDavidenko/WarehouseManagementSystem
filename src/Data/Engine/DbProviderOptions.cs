namespace WarehouseManagementSystem.Data.Engine;

public sealed class DbProviderOptions
{
    public enum DbProvider
    {
        Sqlite,
        Postgre
    }

    public const string OptionsKey = "Database";

    public DbProvider? Provider { get; set; }

    public DbProvider RequiredProvider => Provider ?? throw new ArgumentNullException(nameof(Provider), EmptyServerUrlMessage);

    private const string EmptyServerUrlMessage = $"Конфигурационное значение «{OptionsKey}.{nameof(Provider)}» не задано";
}
