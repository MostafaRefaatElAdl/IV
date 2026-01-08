namespace Inovola.Domain.Entities;

public class WeatherInfo
{
    public string City { get; set; } = null!;
    public decimal Temperature { get; set; }
    public string Condition { get; set; } = null!;
}
