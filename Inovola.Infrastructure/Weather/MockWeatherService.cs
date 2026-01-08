using Inovola.Domain.Entities;
using Inovola.Domain.Interfaces;

namespace Inovola.Infrastructure.Weather;

public class MockWeatherService : IWeatherService
{
    private static readonly string[] Conditions =
    {
        "Sunny",
        "Cloudy",
        "Windy",
        "Rainy"
    };

    public Task<WeatherInfo> GetWeatherAsync(string city)
    {
        var random = new Random();

        var weather = new WeatherInfo
        {
            City = city,
            Temperature = random.Next(15, 40),
            Condition = Conditions[random.Next(Conditions.Length)]
        };

        return Task.FromResult(weather);
    }
}
