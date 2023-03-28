using MassTransit;
using MassTransitOutbox.Saga.Events;

namespace MassTransitOutbox.Saga.Consumers;

public sealed class TestSagaConsumer1 : IConsumer<StartSagaEvent>
{
    public async Task Consume(ConsumeContext<StartSagaEvent> context)
    {
        Console.WriteLine($"TestSagaConsumer1 for {context.Message.CorrelationId}");
        await Task.Delay(5000);

        await context.Publish(new ProgressSagaEvent
        {
            CorrelationId = context.Message.CorrelationId,
        });

        if (context.GetRetryAttempt() == 0)
        {
            throw new Exception();
        }
    }
}