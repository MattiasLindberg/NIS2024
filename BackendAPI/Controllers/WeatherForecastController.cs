using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : Controller
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static int _lowTemp = -20;
    private static int _highTemp = 30;

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get weather using RBAC for authorization.
    /// Both Reader and Contributor can access.
    /// </summary>
    [Authorize(Roles = "Reader,Contributor")]
    [HttpGet(Name = "GetWeatherRBAC")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(_lowTemp, _highTemp),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Set weather using RBAC for authorization.
    /// Only Contributor can perform this action.
    /// </summary>
    [Authorize(Roles = "Contributor")]
    [HttpPost(Name = "SetWeatherRBAC")]
    public void Post(int lowTemp, int highTemp)
    {
        _lowTemp = lowTemp;
        _highTemp = highTemp;
    }

    /// <summary>
    /// Get weather using no authorization.
    /// </summary>
    [HttpPatch(Name = "GetWeatherAnonymous")]
    public IEnumerable<WeatherForecast> Patch()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(_lowTemp, _highTemp),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Set weather.
    /// No authorization, but we require that the caller is authenticated.
    /// Scenario is that the user doesn't have the Reader or the Contributor role,
    /// but ther user can still authenticate against the App Registration.
    /// </summary>
    [Authorize]
    [HttpPut(Name = "SetWeatherAuthorized")]
    public void Put(int lowTemp, int highTemp)
    {
        _lowTemp = lowTemp;
        _highTemp = highTemp;
    }
}
