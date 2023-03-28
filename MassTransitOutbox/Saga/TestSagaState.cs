using Automatonymous;
using MassTransit.Saga;

namespace MassTransitOutbox.Saga;

public sealed class TestSagaState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }

    public int Version { get; set; }
}