using IpGeolocation.Services;

namespace IpGeolocation.ConsoleApp;

public class App
{
    private readonly IIpGeolocationService _ipGeolocationService;

    public App(IIpGeolocationService ipGeolocationService)
    {
        _ipGeolocationService = ipGeolocationService;
    }

    public async Task Run(string[] args)
    {
        var result = await _ipGeolocationService.GetIpGeolocationAsync("8.8.8.8");

        if (result is not null)
        {
            Console.WriteLine(result.City);
        }
    }
}