using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Events;

public record struct TransactionEvent(int TransactionId, string TransactionNumber, string Name, string Amount, DateTime Timestamp);
public record struct StateEvent(string Name, string Amount, DateTime Timestamp);
public record struct WeatherForecast(DateTime Date, int TemperatureC, string? Summary);
