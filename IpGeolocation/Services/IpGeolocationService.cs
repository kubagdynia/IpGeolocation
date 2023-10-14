using IpGeolocation.Models;

namespace IpGeolocation.Services;

public class IpGeolocationService : IIpGeolocationService
{
    private readonly IpApiService _ipApiService;

    public IpGeolocationService(IpApiService ipApiService)
    {
        _ipApiService = ipApiService;
    }
    
    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
        => await _ipApiService.GetFullDataAsync(ipAddress);
    
    public async Task<string> GetCountryAsync(string ipAddress)
        => await _ipApiService.GetCountryAsync(ipAddress);
    
    public async Task<string> GetCityAsync(string ipAddress)
        => await _ipApiService.GetCityAsync(ipAddress);

    public async Task<string> GetTimezoneAsync(string ipAddress)
        => await _ipApiService.GetTimezoneAsync(ipAddress);

    public async Task<string> GetLanguagesAsync(string ipAddress)
        => await _ipApiService.GetLanguagesAsync(ipAddress);

    public async Task<string> GetCurrencyAsync(string ipAddress)
        => await _ipApiService.GetCurrencyAsync(ipAddress);

    public async Task<string> GetCurrencyNameAsync(string ipAddress)
        => await _ipApiService.GetCurrencyNameAsync(ipAddress);
}