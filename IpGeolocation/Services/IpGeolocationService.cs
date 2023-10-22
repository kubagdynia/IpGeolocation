using IpGeolocation.Configuration;
using IpGeolocation.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IpGeolocation.Services;

public class IpGeolocationService : IIpGeolocationService
{
    private readonly IpApiService _ipApiService;
    private readonly IMemoryCache _memoryCache;
    private readonly IpGeolocationSettings _ipGeolocationSettings;

    public IpGeolocationService(IpApiService ipApiService, IMemoryCache memoryCache,
        IOptions<IpGeolocationSettings> ipGeolocationSettings)
    {
        _ipApiService = ipApiService;
        _memoryCache = memoryCache;
        _ipGeolocationSettings = ipGeolocationSettings?.Value;
    }

    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetFullDataAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel);
        }

        IpGeolocationModel ipGeolocationModel = await _ipApiService.GetFullDataAsync(ipAddress);
        
        SetCache(ipAddress, ipGeolocationModel);
        
        return await Task.FromResult(ipGeolocationModel); 
    }

    public async Task<string> GetCountryAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCountryAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Country);
        }

        string cacheKey = $"Country-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedCountry))
        {
            return await Task.FromResult(cachedCountry);
        }
        
        string country = await _ipApiService.GetCountryAsync(ipAddress);
        
        SetCache(cacheKey, country);

        return await Task.FromResult(country);
    }

    public async Task<string> GetCityAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCityAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.City);
        }

        string cacheKey = $"City-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedCity))
        {
            return await Task.FromResult(cachedCity);
        }
        
        string city = await _ipApiService.GetCityAsync(ipAddress);
        
        SetCache(cacheKey, city);

        return await Task.FromResult(city);
    }

    public async Task<string> GetTimezoneAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetTimezoneAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Timezone);
        }

        string cacheKey = $"Timezone-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedTimezone))
        {
            return await Task.FromResult(cachedTimezone);
        }
        
        string timezone = await _ipApiService.GetTimezoneAsync(ipAddress);
        
        SetCache(cacheKey, timezone);

        return await Task.FromResult(timezone);
    }

    public async Task<string> GetLanguagesAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetLanguagesAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Languages);
        }

        string cacheKey = $"Languages-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedLanguages))
        {
            return await Task.FromResult(cachedLanguages);
        }
        
        string languages = await _ipApiService.GetLanguagesAsync(ipAddress);
        
        SetCache(cacheKey, languages);

        return await Task.FromResult(languages);
    }

    public async Task<string> GetCurrencyAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCurrencyAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Currency);
        }

        string cacheKey = $"Currency-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedCurrency))
        {
            return await Task.FromResult(cachedCurrency);
        }
        
        string currency = await _ipApiService.GetCurrencyAsync(ipAddress);
        
        SetCache(cacheKey, currency);

        return await Task.FromResult(currency);
    }

    public async Task<string> GetCurrencyNameAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCurrencyNameAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CurrencyName);
        }

        string cacheKey = $"CurrencyName-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedCurrencyName))
        {
            return await Task.FromResult(cachedCurrencyName);
        }
        
        string currencyName = await _ipApiService.GetCurrencyNameAsync(ipAddress);
        
        SetCache(cacheKey, currencyName);

        return await Task.FromResult(currencyName);
    }
    
    private void SetCache(string cacheKey, object value)
    {
        _memoryCache.Set(cacheKey, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_ipGeolocationSettings.CacheExpirationTimeInMinutes)
        });
    }

    private bool CacheEnabled()
        => _ipGeolocationSettings.CacheEnabled;
}