namespace WarehouseManagementSystem.Kafka.Common.Consumer;

/// <summary>
/// Обработчик сообщений полученных Консюмером
/// </summary>
public interface IConsumerHandler<TData>
{
    /// <summary>
    /// Метод для обработки сообщения
    /// </summary>
    /// <param name="message">Полученное сообщение из Кафки</param>
    Task HandleMessage(TData message);
}
