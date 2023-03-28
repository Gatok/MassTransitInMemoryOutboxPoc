using Automatonymous;
using MassTransitOutbox.Saga.Events;

namespace MassTransitOutbox.Saga;

public sealed class TestSagaStateMachine : MassTransitStateMachine<TestSagaState>
{
    public TestSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Initially(
            When(StartSagaEvent)
                .TransitionTo(Started));

        During(Started,
            When(ProgressSagaEvent)
                .TransitionTo(InProgress));

        During(InProgress,
            When(FinalizeSagaEvent)
            .Then(x => Console.WriteLine($"Saga finalize for {x.Data.CorrelationId}"))
                .Finalize());

        Event(() => StartSagaEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => ProgressSagaEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => FinalizeSagaEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
    }

    public State Started { get; set; }

    public State InProgress { get; set; }

    public Event<StartSagaEvent> StartSagaEvent { get; set; }

    public Event<ProgressSagaEvent> ProgressSagaEvent { get; set; }

    public Event<FinalizeSagaEvent> FinalizeSagaEvent { get; set; }
}