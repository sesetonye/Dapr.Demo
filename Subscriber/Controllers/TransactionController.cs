using Subscriber.Events;

namespace Subscriber.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly DaprClient _daprClient;

    public TransactionController(ILogger<TransactionController> logger, DaprClient daprClient)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        _logger.LogInformation("this is a tests.");
        var rng = new Random();
        return rng.Next(-20, 55).ToString();
    }


    [Topic("pubsub", "transaction")]
    [HttpPost("processtransaction")]
    public async Task<ActionResult> ProcessTransaction(TransactionEvent transaction, [FromServices] DaprClient daprClient)
    {
     
        //var vehicleInfo = _vehicleRegistrationService.GetVehicleInfo(speedingViolation.VehicleId).Result;
        //var vehicleInfo = await _daprClient.InvokeMethodAsync<VehicleInfo>(
        //    HttpMethod.Get,
        //   "vehicleregistrationservice",
        //   $"vehicleInfo");


        // log fine
        string fineString = $"{transaction.Amount} Euro";
        _logger.LogInformation($"Sent speeding ticket to {transaction.Name}. " +
            $"Road: {transaction.TransactionId}, Licensenumber: {transaction.TransactionId}, " +
            $"Vehicle: {transaction.TransactionNumber} {transaction.TransactionNumber}, " +
            $"Violation: {transaction.Amount} Km/h, Fine: {fineString}, " +
            $"On: {transaction.Timestamp.ToString("dd-MM-yyyy")} " +
            $"at {transaction.Timestamp.ToString("hh:mm:ss")}.");

        return Ok();
    }
}

