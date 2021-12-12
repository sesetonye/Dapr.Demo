using Dapr.Client;
using Publisher.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher;
public class TransactionSimulation
{
    private Random _rnd;
    private int _transactionId;
    private int _minDelayInMS = 50;
    private int _maxDelayInMS = 5000;
    private readonly string _pubsubName = "pubsub";
    private const string DAPR_STORE_NAME = "statestore";
    public TransactionSimulation(int transactionNumber)
    {
        _rnd = new Random();
        _transactionId = transactionNumber;
    }
    public Task Start()
    {
        Console.WriteLine($"Start Transaction {_transactionId} simulation.");
        var client = new DaprClientBuilder().Build();
        while (true)
        {
            try
            {
                TimeSpan delay = TimeSpan.FromMilliseconds(_rnd.Next(_minDelayInMS, _maxDelayInMS) + _rnd.NextDouble());
                Task.Delay(delay).Wait();

                Task.Run(async () =>
                {

                    var transaction = new TransactionEvent
                    {
                        TransactionId = _transactionId,
                        TransactionNumber = GenerateRandomTransactionNumber(),
                        Name = GenerateRandomName(),
                        Amount = $"${_rnd.Next(1, 1000)}.00",
                        Timestamp = DateTime.Now
                    };
                  

                    try
                    {
                        // 1. push to transaction Topic
                        await client.PublishEventAsync(_pubsubName, "transaction", transaction);

                        var eventData = new StateEvent{ Name = GenerateRandomName(), Amount = $"${_rnd.Next(1, 1000)}.00", Timestamp = DateTime.Now };

                        // 2. Save to Redis Store
                        await client.SaveStateAsync<StateEvent>(DAPR_STORE_NAME, eventData.Name, eventData);
                        Task.Delay(delay).Wait();
                        var test = (await client.GetStateAsync<StateEvent>(DAPR_STORE_NAME, eventData.Name));
                        Console.WriteLine($"this is state store: {test.Amount}");
                        
                        // 3. push to name-subscription Topic
                        await client.PublishEventAsync(_pubsubName, "name-subscription", eventData);

                        // 4. Service to service invocation
                        var weatherInfo1 = await client.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
                            HttpMethod.Get,
                           "subscriber.state",
                           $"weatherforecast");
                        Console.WriteLine($"{weatherInfo1.First()}");

                        var weatherInfo = await client.InvokeMethodAsync<WeatherForecast>(
                            HttpMethod.Get,
                           "subscriber.state",
                           $"state/weather");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.InnerException}");
                        throw ex;
                    }
                    Console.WriteLine($"Simulated SENDING of Transaction with Transaction-number {transaction.TransactionNumber} with total {transaction.Amount}");

                }).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction {_transactionId} error: {ex.Message}");
            }
        }
    }

    #region Private helper methods

    private string _validTransactionNumberChars = "FRANKLIN";

    private string GenerateRandomTransactionNumber()
    {
        int type = _rnd.Next(1, 9);
        string kenteken = string.Empty;
        switch (type)
        {
            case 1: // 99-AA-99
                kenteken = string.Format("{0:00}-{1}-{2:00}", _rnd.Next(1, 99), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                break;
            case 2: // AA-99-AA
                kenteken = string.Format("{0}-{1:00}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 99), GenerateRandomCharacters(2));
                break;
            case 3: // AA-AA-99
                kenteken = string.Format("{0}-{1}-{2:00}", GenerateRandomCharacters(2), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                break;
            case 4: // 99-AA-AA
                kenteken = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(2), GenerateRandomCharacters(2));
                break;
            case 5: // 99-AAA-9
                kenteken = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                break;
            case 6: // 9-AAA-99
                kenteken = string.Format("{0}-{1}-{2:00}", _rnd.Next(1, 9), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                break;
            case 7: // AA-999-A
                kenteken = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 999), GenerateRandomCharacters(1));
                break;
            case 8: // A-999-AA
                kenteken = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(1), _rnd.Next(1, 999), GenerateRandomCharacters(2));
                break;
        }

        return kenteken;
    }

    private string GenerateRandomCharacters(int aantal)
    {
        char[] chars = new char[aantal];
        for (int i = 0; i < aantal; i++)
        {
            chars[i] = _validTransactionNumberChars[_rnd.Next(_validTransactionNumberChars.Length - 1)];
        }
        return new string(chars);
    }

    private string GenerateRandomName()
    {
        string[] maleNames = new string[10] { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian" };
        string[] femaleNames = new string[4] { "abby", "abigail", "adele", "adrian" };
        string[] lastNames = new string[5] { "abbott", "acosta", "adams", "adkins", "aguilar" };
        var firstName = "";

        Random rand = new Random(DateTime.Now.Second);
        if (rand.Next(1, 2) == 1)
        {
            firstName = maleNames[rand.Next(0, maleNames.Length - 1)];
        }
        else
        {
            firstName = femaleNames[rand.Next(0, femaleNames.Length - 1)];
        }
        var lastName = lastNames[rand.Next(0, lastNames.Length - 1)];
        return $"{firstName} {lastName}";
    }

    #endregion
}

