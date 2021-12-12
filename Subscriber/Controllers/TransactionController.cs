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
        _logger.LogInformation($"Process Transaction for {transaction.Name}. " +
            $"TransactionId: {transaction.TransactionId} " +
            $"TransactionNumber: {transaction.TransactionNumber} " +
            $"Amount: {transaction.Amount}" +
            $"On: {transaction.Timestamp.ToString("dd-MM-yyyy")} " +
            $"at {transaction.Timestamp.ToString("hh:mm:ss")}.");

        return Ok();
    }
}

