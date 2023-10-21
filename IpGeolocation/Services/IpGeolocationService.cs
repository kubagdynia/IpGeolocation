using IpGeolocation.Models;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeolocation.Services;

public class IpGeolocationService : IIpGeolocationService
{
    private readonly IpApiService _ipApiService;
    private readonly IMemoryCache _memoryCache;

    public IpGeolocationService(IpApiService ipApiService, IMemoryCache memoryCache)
    {
        _ipApiService = ipApiService;
        _memoryCache = memoryCache;
    }

    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
    {
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel);
        }

        IpGeolocationModel ipGeolocationModel = await _ipApiService.GetFullDataAsync(ipAddress);

        _memoryCache.Set(ipAddress, ipGeolocationModel, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        
        return ipGeolocationModel; 
    }
    
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