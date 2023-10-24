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

    public async Task<string> GetCountryCodeAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCountryCodeAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCode);
        }

        string cacheKey = $"CountryCode-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedCountryCode))
        {
            return await Task.FromResult(cachedCountryCode);
        }
        
        string countryCode = await _ipApiService.GetCountryCodeAsync(ipAddress);
        
        SetCache(cacheKey, countryCode);

        return await Task.FromResult(countryCode);
    }

    public async Task<string> GetCountryCodeIso3Async(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCountryCodeIso3Async(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCodeIso3);
        }

        string cacheKey = $"CountryCodeIso3-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedCountryCodeIso3))
        {
            return await Task.FromResult(cachedCountryCodeIso3);
        }
        
        string countryCodeIso3 = await _ipApiService.GetCountryCodeIso3Async(ipAddress);
        
        SetCache(cacheKey, countryCodeIso3);

        return await Task.FromResult(countryCodeIso3);
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

    public async Task<string> GetContinentCodeAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetContinentCodeAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.ContinentCode);
        }

        string cacheKey = $"ContinentCode-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedContinentCode))
        {
            return await Task.FromResult(cachedContinentCode);
        }
        
        string continentCode = await _ipApiService.GetContinentCodeAsync(ipAddress);
        
        SetCache(cacheKey, continentCode);

        return await Task.FromResult(continentCode);
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

    public async Task<string> GetRegionAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetRegionAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Region);
        }

        string cacheKey = $"Region-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedRegion))
        {
            return await Task.FromResult(cachedRegion);
        }
        
        string region = await _ipApiService.GetRegionAsync(ipAddress);
        
        SetCache(cacheKey, region);

        return await Task.FromResult(region);
    }

    public async Task<string> GetRegionCodeAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetRegionCodeAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.RegionCode);
        }

        string cacheKey = $"RegionCode-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedRegionCode))
        {
            return await Task.FromResult(cachedRegionCode);
        }
        
        string regionCode = await _ipApiService.GetRegionCodeAsync(ipAddress);
        
        SetCache(cacheKey, regionCode);

        return await Task.FromResult(regionCode);
    }

    public async Task<string> GetUtcOffsetAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetUtcOffsetAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.UtcOffset);
        }

        string cacheKey = $"UtcOffset-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedUtcOffset))
        {
            return await Task.FromResult(cachedUtcOffset);
        }
        
        string utcOffset = await _ipApiService.GetUtcOffsetAsync(ipAddress);
        
        SetCache(cacheKey, utcOffset);

        return await Task.FromResult(utcOffset);
    }

    public async Task<string> GetOrgAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetUtcOffsetAsync(ipAddress);
        }
        
        if (_memoryCache.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Org);
        }

        string cacheKey = $"Org-{ipAddress}";
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedOrg))
        {
            return await Task.FromResult(cachedOrg);
        }
        
        string org = await _ipApiService.GetOrgAsync(ipAddress);
        
        SetCache(cacheKey, org);

        return await Task.FromResult(org);
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