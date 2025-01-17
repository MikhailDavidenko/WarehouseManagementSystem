namespace WarehouseManagementSystem.Kafka.Common.Producer;

/// <summary>
/// Сервис для публикации сообщений в Кафку
/// </summary>
/// <typeparam name="TData">Тип публикуемого сообщения</typeparam>
public interface IProducerService<TData>
{
    Task SendMessage(string topic, TData data, CancellationToken cancellationToken);
}
