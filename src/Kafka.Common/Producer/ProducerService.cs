using Confluent.Kafka;

namespace WarehouseManagementSystem.Kafka.Common.Producer;

internal sealed class ProducerService<TData> : IProducerService<TData>, IDisposable
{
    private readonly IProducer<Ignore, TData> producer;

    public ProducerService(string bootstrapServers)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true
        };

        producer = new ProducerBuilder<Ignore, TData>(producerConfig).Build();
    }

    public async Task SendMessage(string topic, TData data, CancellationToken cancellationToken)
    {
        try
        {
            var message = new Message<Ignore, TData> { Value = data };

            await producer.ProduceAsync(topic, message, cancellationToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Отправка сообщения завершилась с ошибкой. Сообщение: '{exception.Message}'");
            throw;
        }

    }

    public void Dispose()
    {
        producer.Dispose();
    }
}
