using Subscriber.State.Events;
using Subscriber.State.Services;

namespace Subscriber.State.Controllers;

[ApiController]
[Route("[controller]")]
public class StateController : ControllerBase
{
    private readonly ILogger<StateController> _logger;
    private readonly DaprClient _daprClient;
    private const string DAPR_STORE_NAME = "statestore";
    private readonly SubscriberService _subscriberService;

    public StateController(ILogger<StateController> logger, DaprClient daprClient, SubscriberService subscriberService)
    {
        _logger = logger;
        _subscriberService = subscriberService;
        _daprClient = daprClient;
    }

    [HttpGet("weather")]
    public ActionResult GetWeather()
    {
        _logger.LogInformation("this is a tests.");
        var number = Random.Shared.Next(-20, 55);
        var weatherForecast = _subscriberService.GetWeatherForecast(number).Result;
        return Ok(weatherForecast);
    }


    [Topic("pubsub", "name-subscription")]
    [HttpPost("processState")]
    public async Task<ActionResult> ProcessState(StateEvent stateEvent, [FromServices] DaprClient daprClient)
    {
        // Log even data

        _logger.LogInformation($"Customer: {stateEvent.Name}. " +
            $"Purchase Price: {stateEvent.Amount} " +
            $"On: {stateEvent.Timestamp.ToString("dd-MM-yyyy")} " +
            $"at {stateEvent.Timestamp.ToString("hh:mm:ss")}.");

        return Ok();
    }
}
