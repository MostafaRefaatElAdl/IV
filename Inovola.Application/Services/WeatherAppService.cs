using Inovola.Application.Interfaces;
using Inovola.Domain.Entities;
using Inovola.Domain.Interfaces;

namespace Inovola.Application.Services;

public class WeatherAppService
{
    private readonly IWeatherService _weatherService;
    private readonly ICacheService _cacheService;

    public WeatherAppService(
        IWeatherService weatherService,
        ICacheService cacheService)
    {
        _weatherService = weatherService;
        _cacheService = cacheService;
    }

    public async Task<WeatherInfo> GetWeatherAsync(string city)
    {
        var cacheKey = $"weather_{city.ToLower()}";

        var cached = await _cacheService.GetAsync<WeatherInfo>(cacheKey);
        if (cached != null)
            return cached;

        var weather = await _weatherService.GetWeatherAsync(city);

        await _cacheService.SetAsync(
            cacheKey,
            weather,
            TimeSpan.FromMinutes(5));

        return weather;
    }
}
