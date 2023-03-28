using MassTransit;

namespace MassTransitOutbox.Saga.Events;

public sealed class ProgressSagaEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}