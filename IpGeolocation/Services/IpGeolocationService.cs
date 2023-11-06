using IpGeolocation.Cache;
using IpGeolocation.Configuration;
using IpGeolocation.Models;
using Microsoft.Extensions.Options;

namespace IpGeolocation.Services;

public class IpGeolocationService : IIpGeolocationService
{
    private readonly IpApiService _ipApiService;
    private readonly ICacheService _cacheService;
    private readonly IpGeolocationSettings _ipGeolocationSettings;

    public IpGeolocationService(
        IpApiService ipApiService,
        ICacheService cacheService,
        IOptions<IpGeolocationSettings> ipGeolocationSettings)
    {
        _ipApiService = ipApiService;
        _cacheService = cacheService;
        _ipGeolocationSettings = ipGeolocationSettings?.Value;
    }

    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetFullDataAsync(ipAddress);
        }

        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel);
        }

        IpGeolocationModel ipGeolocationModel = await _ipApiService.GetFullDataAsync(ipAddress);
        
        await _cacheService.SetAsync(ipGeolocationModel);
        
        return await Task.FromResult(ipGeolocationModel); 
    }

    public async Task<string> GetCountryAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCountryAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Country);
        }

        string cacheKey = $"country@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string country = await _ipApiService.GetCountryAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, country));

        return await Task.FromResult(country);
    }

    public async Task<string> GetCountryCodeAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCountryCodeAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCode);
        }

        string cacheKey = $"country-code@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string countryCode = await _ipApiService.GetCountryCodeAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, countryCode));

        return await Task.FromResult(countryCode);
    }

    public async Task<string> GetCountryCodeIso3Async(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCountryCodeIso3Async(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCodeIso3);
        }

        string cacheKey = $"country-code-iso3@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string countryCodeIso3 = await _ipApiService.GetCountryCodeIso3Async(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, countryCodeIso3));

        return await Task.FromResult(countryCodeIso3);
    }

    public async Task<string> GetCityAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCityAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.City);
        }

        string cacheKey = $"city@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string city = await _ipApiService.GetCityAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, city));

        return await Task.FromResult(city);
    }

    public async Task<string> GetContinentCodeAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetContinentCodeAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.ContinentCode);
        }

        string cacheKey = $"continent-code@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string continentCode = await _ipApiService.GetContinentCodeAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, continentCode));

        return await Task.FromResult(continentCode);
    }

    public async Task<string> GetTimezoneAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetTimezoneAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Timezone);
        }

        string cacheKey = $"timezone@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string timezone = await _ipApiService.GetTimezoneAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, timezone));

        return await Task.FromResult(timezone);
    }

    public async Task<string> GetLanguagesAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetLanguagesAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Languages);
        }

        string cacheKey = $"languages@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string languages = await _ipApiService.GetLanguagesAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, languages));

        return await Task.FromResult(languages);
    }

    public async Task<string> GetCurrencyAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCurrencyAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Currency);
        }

        string cacheKey = $"currency@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string currency = await _ipApiService.GetCurrencyAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, currency));

        return await Task.FromResult(currency);
    }

    public async Task<string> GetCurrencyNameAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetCurrencyNameAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CurrencyName);
        }

        string cacheKey = $"currency-name@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string currencyName = await _ipApiService.GetCurrencyNameAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, currencyName));

        return await Task.FromResult(currencyName);
    }

    public async Task<string> GetRegionAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetRegionAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Region);
        }

        string cacheKey = $"region@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string region = await _ipApiService.GetRegionAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, region));

        return await Task.FromResult(region);
    }

    public async Task<string> GetRegionCodeAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetRegionCodeAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.RegionCode);
        }

        string cacheKey = $"region-code@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string regionCode = await _ipApiService.GetRegionCodeAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, regionCode));

        return await Task.FromResult(regionCode);
    }

    public async Task<string> GetUtcOffsetAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetUtcOffsetAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.UtcOffset);
        }

        string cacheKey = $"utc-offset@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string utcOffset = await _ipApiService.GetUtcOffsetAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, utcOffset));

        return await Task.FromResult(utcOffset);
    }

    public async Task<string> GetOrgAsync(string ipAddress)
    {
        if (!CacheEnabled())
        {
            return await _ipApiService.GetOrgAsync(ipAddress);
        }
        
        if (_cacheService.TryGetValue(IpGeolocationModel.GetCacheKey(ipAddress),
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Org);
        }

        string cacheKey = $"org@{ipAddress}";
        
        if (_cacheService.TryGetValue(SpecificField.GetCacheKey(cacheKey),
                out SpecificField cachedSpecificField))
        {
            return await Task.FromResult(cachedSpecificField.Value);
        }
        
        string org = await _ipApiService.GetOrgAsync(ipAddress);

        await _cacheService.SetAsync(SpecificField.Create(cacheKey, org));

        return await Task.FromResult(org);
    }

    private bool CacheEnabled()
        => _ipGeolocationSettings.CacheEnabled;
}