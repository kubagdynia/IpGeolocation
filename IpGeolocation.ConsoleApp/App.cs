using IpGeolocation.Models;
using IpGeolocation.Services;

namespace IpGeolocation.ConsoleApp;

public class App
{
    private readonly IIpGeolocationService _ipGeolocationService;

    public App(IIpGeolocationService ipGeolocationService)
    {
        _ipGeolocationService = ipGeolocationService;
    }

    public async Task Run(Options options)
    {
        if (options.Full)
        {
            IpGeolocationModel result = await _ipGeolocationService.GetIpGeolocationAsync(options.IpAddress);
            
            if (result is not null)
            {
                Console.WriteLine($"IP: {result.Ip}");
                Console.WriteLine($"City: {result.City}");
                Console.WriteLine($"ContinentCode: {result.ContinentCode}");
                Console.WriteLine($"Country: {result.Country}");
                Console.WriteLine($"CountryCapital: {result.CountryCapital}");
                Console.WriteLine($"CountryCode: {result.CountryCode}");
                Console.WriteLine($"CountryCodeIso3: {result.CountryCodeIso3}");
                Console.WriteLine($"CountryName: {result.CountryName}");
                Console.WriteLine($"Currency: {result.Currency}");
                Console.WriteLine($"CurrencyName: {result.CurrencyName}");
                Console.WriteLine($"Region: {result.Region}");
                Console.WriteLine($"RegionCode: {result.RegionCode}");
                Console.WriteLine($"UtcOffset: {result.UtcOffset}");
                Console.WriteLine($"Org: {result.Org}");
            }
        }
        else
        {
            if (options.City)
            {
                string city = await _ipGeolocationService.GetCityAsync(options.IpAddress);
                Console.WriteLine($"City: {city}");
            }
            
            if (options.Region)
            {
                string region = await _ipGeolocationService.GetRegionAsync(options.IpAddress);
                Console.WriteLine($"Region: {region}");
            }
            
            if (options.Country)
            {
                string country = await _ipGeolocationService.GetCountryAsync(options.IpAddress);
                Console.WriteLine($"Country: {country}");
            }
        }
        
        
    }
}