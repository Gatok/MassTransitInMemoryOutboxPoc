using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using MassTransitOutbox.Saga;
using MassTransitOutbox.Saga.Consumers;

namespace MassTransitOutbox;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<ISagaRepository<TestSagaState>, InMemorySagaRepository<TestSagaState>>();

        builder.Services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<TestSagaStateMachine, TestSagaState>()
                .InMemoryRepository();

            cfg.AddConsumer<TestSagaConsumer1>();
            cfg.AddConsumer<TestSagaConsumer2>();

            cfg.UsingRabbitMq((context, opt) =>
            {
                opt.Host(builder.Configuration["RabbitMQ:Uri"], config =>
                {
                    config.Username(builder.Configuration["RabbitMQ:Username"]);
                    config.Password(builder.Configuration["RabbitMQ:Password"]);
                });

                opt.UseMessageRetry(r => r.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1)));
                opt.UseInMemoryOutbox();

                //opt.UseMessageScope(context);
                //opt.UsePublishFilter(typeof(TestFilter<>), context);

                opt.ReceiveEndpoint("TestSaga", config =>
                {
                    config.StateMachineSaga<TestSagaState>(context.GetService<IServiceProvider>());
                    config.SetQuorumQueue();
                    config.UseInMemoryOutbox();
                });

                opt.ReceiveEndpoint("TestSagaConsumer1", config =>
                {
                    config.ConfigureConsumer<TestSagaConsumer1>(context);
                    config.SetQuorumQueue();
                    config.UseInMemoryOutbox();
                });

                opt.ReceiveEndpoint("TestSagaConsumer2", config =>
                {
                    config.ConfigureConsumer<TestSagaConsumer2>(context);
                    config.SetQuorumQueue();
                    config.UseInMemoryOutbox();
                });
            });

            //cfg.UsingInMemory((context, opt) =>
            //{
            //    opt.UseInMemoryOutbox();
            //    opt.ConfigureEndpoints(context);
            //});
        });

        builder.Services.AddMassTransitHostedService();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}