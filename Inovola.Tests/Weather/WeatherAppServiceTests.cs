using FluentAssertions;
using Inovola.Application.Interfaces;
using Inovola.Application.Services;
using Inovola.Domain.Entities;
using Inovola.Domain.Interfaces;
using Moq;

namespace Inovola.Application.Tests.Weather;

public class WeatherAppServiceTests
{
    private readonly Mock<IWeatherService> _weatherServiceMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();

    private WeatherAppService CreateService()
        => new WeatherAppService(
            _weatherServiceMock.Object,
            _cacheServiceMock.Object);

    [Fact]
    public async Task GetWeatherAsync_WhenCacheHit_ReturnsCachedValue()
    {
        // Arrange
        var cachedWeather = new WeatherInfo
        {
            City = "Cairo",
            Temperature = 25,
            Condition = "Sunny"
        };

        _cacheServiceMock
            .Setup(c => c.GetAsync<WeatherInfo>(It.IsAny<string>()))
            .ReturnsAsync(cachedWeather);

        var service = CreateService();

        // Act
        var result = await service.GetWeatherAsync("Cairo");

        // Assert
        result.Should().BeEquivalentTo(cachedWeather);

        _weatherServiceMock.Verify(
            w => w.GetWeatherAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task GetWeatherAsync_WhenCacheMiss_CallsWeatherService_AndCachesResult()
    {
        // Arrange
        _cacheServiceMock
            .Setup(c => c.GetAsync<WeatherInfo>(It.IsAny<string>()))
            .ReturnsAsync((WeatherInfo?)null);

        var weatherFromService = new WeatherInfo
        {
            City = "Cairo",
            Temperature = 30,
            Condition = "Cloudy"
        };

        _weatherServiceMock
            .Setup(w => w.GetWeatherAsync("Cairo"))
            .ReturnsAsync(weatherFromService);

        var service = CreateService();

        // Act
        var result = await service.GetWeatherAsync("Cairo");

        // Assert
        result.Should().BeEquivalentTo(weatherFromService);

        _weatherServiceMock.Verify(
            w => w.GetWeatherAsync("Cairo"),
            Times.Once);

        _cacheServiceMock.Verify(
            c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<WeatherInfo>(),
                It.IsAny<TimeSpan>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_UsesSameCacheKey_ForSameCityIgnoringCase()
    {
        // Arrange
        WeatherInfo? cachedValue = null;

        _cacheServiceMock
            .Setup(c => c.GetAsync<WeatherInfo>(It.IsAny<string>()))
            .ReturnsAsync(() => cachedValue);

        _cacheServiceMock
            .Setup(c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<WeatherInfo>(),
                It.IsAny<TimeSpan>()))
            .Callback<string, WeatherInfo, TimeSpan>((_, value, __) =>
            {
                cachedValue = value;
            })
            .Returns(Task.CompletedTask);

        var weatherFromService = new WeatherInfo
        {
            City = "Cairo",
            Temperature = 28,
            Condition = "Windy"
        };

        _weatherServiceMock
            .Setup(w => w.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync(weatherFromService);

        var service = CreateService();

        // Act
        var first = await service.GetWeatherAsync("Cairo");
        var second = await service.GetWeatherAsync("cairo");

        // Assert
        first.Should().BeEquivalentTo(second);

        _weatherServiceMock.Verify(
            w => w.GetWeatherAsync(It.IsAny<string>()),
            Times.Once);
    }
}
