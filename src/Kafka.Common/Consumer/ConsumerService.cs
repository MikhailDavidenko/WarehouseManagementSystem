using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace WarehouseManagementSystem.Kafka.Common.Consumer;

internal sealed class ConsumerService<TData> : IConsumerService, IHostedService, IDisposable
{
    private readonly IConsumer<Ignore, TData> consumer;
    private readonly IConsumerHandler<TData> consumerHandler;
    private readonly string topic;

    public ConsumerService(
        string bootstrapServers,
        string groupId,
        string topic,
        IConsumerHandler<TData> consumerHandler)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = true
        };

        consumer = new ConsumerBuilder<Ignore, TData>(config).Build();
        this.topic = topic;
        this.consumerHandler = consumerHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => ConsumeMessages(cancellationToken));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        consumer.Close();
        return Task.CompletedTask;
    }

    public async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(cancellationToken);

                    await consumerHandler.HandleMessage(result.Message.Value);
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"В работе консьюмера произошла ошибка: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        consumer.Close();
    }
}
