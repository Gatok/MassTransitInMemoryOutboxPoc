using MassTransit;

namespace MassTransitOutbox.Saga.Events;

public sealed class FinalizeSagaEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}