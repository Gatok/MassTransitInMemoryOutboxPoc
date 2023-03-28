using GreenPipes;
using MassTransit;

namespace MassTransitOutbox.Saga.Filters;

public sealed class TestFilter<T> : IFilter<PublishContext<T>>
    where T : class
{
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        await next.Send(context);

        Console.WriteLine("TestFilter");
    }
}