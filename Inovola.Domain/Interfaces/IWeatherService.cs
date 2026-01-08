using Inovola.Domain.Entities;

namespace Inovola.Domain.Interfaces;

public interface IWeatherService
{
    Task<WeatherInfo> GetWeatherAsync(string city);
}
