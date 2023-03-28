using MassTransit;

namespace MassTransitOutbox.Saga.Events;

public sealed class StartSagaEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}