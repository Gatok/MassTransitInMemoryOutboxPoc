using MassTransit;
using MassTransitOutbox.Saga.Events;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitOutbox.Controllers;

[ApiController]
[Route("[controller]")]
public class SagaController : ControllerBase
{
    private readonly IPublishEndpoint publishEndpoint;

    public SagaController(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    [HttpPost()]
    public async Task<IActionResult> StartSaga([FromBody] bool exception = false)
    {
        await this.publishEndpoint.Publish(new StartSagaEvent
        {
            CorrelationId = Guid.NewGuid(),
        });

        if (exception)
        {
            throw new Exception();
        }

        return Ok();
    }
}