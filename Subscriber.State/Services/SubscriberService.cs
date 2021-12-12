﻿using Subscriber.State.Events;

namespace Subscriber.State.Services
{
    public class SubscriberService
    {
        private HttpClient _httpClient;
        public SubscriberService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetWeatherForecast(int forecastId)
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast>($"WeatherForecast/{forecastId}");
        }
    }
}
