namespace WarehouseManagementSystem.Kafka.Common.Consumer;

/// <summary>
/// Сервис для чтения сообщений из Кафки
/// </summary>
public interface IConsumerService
{
    /// <summary>
    /// Получить сообщение
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task ConsumeMessages(CancellationToken cancellationToken);
}
