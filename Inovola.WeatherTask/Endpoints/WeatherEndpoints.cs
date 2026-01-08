using Microsoft.AspNetCore.Authorization;
using Inovola.Application.Services;

public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/weather",
            async (string city, WeatherAppService service) =>
            {
                var result = await service.GetWeatherAsync(city);
                return Results.Ok(result);
            })
           .RequireAuthorization();
    }
}
