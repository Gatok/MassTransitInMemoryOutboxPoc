using MassTransit;
using MassTransitOutbox.Saga.Events;

namespace MassTransitOutbox.Saga.Consumers;

public sealed class TestSagaConsumer2 : IConsumer<ProgressSagaEvent>
{
    public async Task Consume(ConsumeContext<ProgressSagaEvent> context)
    {
        Console.WriteLine($"TestSagaConsumer2 for {context.Message.CorrelationId}");
        await Task.Delay(5000);

        await context.Publish(new FinalizeSagaEvent
        {
            CorrelationId = context.Message.CorrelationId,
        });
    }
}