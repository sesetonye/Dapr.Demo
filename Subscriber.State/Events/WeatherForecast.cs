namespace Subscriber.State.Events;

public record struct WeatherForecast(DateTime Date, int TemperatureC, string? Summary);
