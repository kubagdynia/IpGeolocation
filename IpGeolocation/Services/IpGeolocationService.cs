using CacheDrive.Services;
using IpGeolocation.Models;

namespace IpGeolocation.Services;

internal class IpGeolocationService : IIpGeolocationService
{
    private readonly IpApiService _ipApiService;
    private readonly ICacheService _cacheService;

    public IpGeolocationService(IpApiService ipApiService, ICacheService cacheService)
    {
        _ipApiService = ipApiService;
        _cacheService = cacheService;
    }

    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel);
        }

        var ipGeolocationModel = await _ipApiService.GetFullDataAsync(ipAddress);
        
        await _cacheService.SetAsync(ipAddress, ipGeolocationModel);
        
        return await Task.FromResult(ipGeolocationModel); 
    }

    public async Task<string> GetCountryAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Country);
        }

        var cacheKey = $"country@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var country = await _ipApiService.GetCountryAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, country);

        return await Task.FromResult(country);
    }
    
    public async Task<string> GetCountryNameAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryName);
        }

        var cacheKey = $"country-name@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var countryName = await _ipApiService.GetCountryNameAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, countryName);

        return await Task.FromResult(countryName);
    }

    public async Task<string> GetCountryCodeAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCode);
        }

        var cacheKey = $"country-code@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var countryCode = await _ipApiService.GetCountryCodeAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, countryCode);

        return await Task.FromResult(countryCode);
    }

    public async Task<string> GetCountryCodeIso3Async(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCodeIso3);
        }

        var cacheKey = $"country-code-iso3@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var countryCodeIso3 = await _ipApiService.GetCountryCodeIso3Async(ipAddress);

        await _cacheService.SetAsync(cacheKey, countryCodeIso3);

        return await Task.FromResult(countryCodeIso3);
    }

    public async Task<string> GetCityAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.City);
        }

        var cacheKey = $"city@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var city = await _ipApiService.GetCityAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, city);

        return await Task.FromResult(city);
    }

    public async Task<string> GetContinentCodeAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.ContinentCode);
        }

        var cacheKey = $"continent-code@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var continentCode = await _ipApiService.GetContinentCodeAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, continentCode);

        return await Task.FromResult(continentCode);
    }

    public async Task<string> GetTimezoneAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Timezone);
        }

        var cacheKey = $"timezone@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var timezone = await _ipApiService.GetTimezoneAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, timezone);

        return await Task.FromResult(timezone);
    }

    public async Task<string> GetLanguagesAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Languages);
        }

        var cacheKey = $"languages@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var languages = await _ipApiService.GetLanguagesAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, languages);

        return await Task.FromResult(languages);
    }

    public async Task<string> GetCurrencyAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Currency);
        }

        var cacheKey = $"currency@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var currency = await _ipApiService.GetCurrencyAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, currency);

        return await Task.FromResult(currency);
    }

    public async Task<string> GetCurrencyNameAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CurrencyName);
        }

        var cacheKey = $"currency-name@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var currencyName = await _ipApiService.GetCurrencyNameAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, currencyName);

        return await Task.FromResult(currencyName);
    }

    public async Task<string> GetRegionAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Region);
        }

        var cacheKey = $"region@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var region = await _ipApiService.GetRegionAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, region);

        return await Task.FromResult(region);
    }

    public async Task<string> GetRegionCodeAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.RegionCode);
        }

        var cacheKey = $"region-code@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var regionCode = await _ipApiService.GetRegionCodeAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, regionCode);

        return await Task.FromResult(regionCode);
    }

    public async Task<string> GetUtcOffsetAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.UtcOffset);
        }

        var cacheKey = $"utc-offset@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var utcOffset = await _ipApiService.GetUtcOffsetAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, utcOffset);

        return await Task.FromResult(utcOffset);
    }

    public async Task<string> GetOrgAsync(string ipAddress)
    {
        if (_cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Org);
        }

        var cacheKey = $"org@{ipAddress}";
        
        if (_cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        var org = await _ipApiService.GetOrgAsync(ipAddress);

        await _cacheService.SetAsync(cacheKey, org);

        return await Task.FromResult(org);
    }
}