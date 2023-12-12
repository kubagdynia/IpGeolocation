using CacheDrive.Services;
using IpGeolocation.Models;

namespace IpGeolocation.Services;

public class IpGeolocationService(IpApiService ipApiService, ICacheService cacheService) : IIpGeolocationService
{
    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress,
                out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel);
        }

        IpGeolocationModel ipGeolocationModel = await ipApiService.GetFullDataAsync(ipAddress);
        
        await cacheService.SetAsync(ipAddress, ipGeolocationModel);
        
        return await Task.FromResult(ipGeolocationModel); 
    }

    public async Task<string> GetCountryAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Country);
        }

        string cacheKey = $"country@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string country = await ipApiService.GetCountryAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, country);

        return await Task.FromResult(country);
    }

    public async Task<string> GetCountryCodeAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCode);
        }

        string cacheKey = $"country-code@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string countryCode = await ipApiService.GetCountryCodeAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, countryCode);

        return await Task.FromResult(countryCode);
    }

    public async Task<string> GetCountryCodeIso3Async(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CountryCodeIso3);
        }

        string cacheKey = $"country-code-iso3@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string countryCodeIso3 = await ipApiService.GetCountryCodeIso3Async(ipAddress);

        await cacheService.SetAsync(cacheKey, countryCodeIso3);

        return await Task.FromResult(countryCodeIso3);
    }

    public async Task<string> GetCityAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.City);
        }

        string cacheKey = $"city@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string city = await ipApiService.GetCityAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, city);

        return await Task.FromResult(city);
    }

    public async Task<string> GetContinentCodeAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.ContinentCode);
        }

        string cacheKey = $"continent-code@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string continentCode = await ipApiService.GetContinentCodeAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, continentCode);

        return await Task.FromResult(continentCode);
    }

    public async Task<string> GetTimezoneAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Timezone);
        }

        string cacheKey = $"timezone@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string timezone = await ipApiService.GetTimezoneAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, timezone);

        return await Task.FromResult(timezone);
    }

    public async Task<string> GetLanguagesAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Languages);
        }

        string cacheKey = $"languages@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string languages = await ipApiService.GetLanguagesAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, languages);

        return await Task.FromResult(languages);
    }

    public async Task<string> GetCurrencyAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Currency);
        }

        string cacheKey = $"currency@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string currency = await ipApiService.GetCurrencyAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, currency);

        return await Task.FromResult(currency);
    }

    public async Task<string> GetCurrencyNameAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.CurrencyName);
        }

        string cacheKey = $"currency-name@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string currencyName = await ipApiService.GetCurrencyNameAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, currencyName);

        return await Task.FromResult(currencyName);
    }

    public async Task<string> GetRegionAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Region);
        }

        string cacheKey = $"region@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string region = await ipApiService.GetRegionAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, region);

        return await Task.FromResult(region);
    }

    public async Task<string> GetRegionCodeAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.RegionCode);
        }

        string cacheKey = $"region-code@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string regionCode = await ipApiService.GetRegionCodeAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, regionCode);

        return await Task.FromResult(regionCode);
    }

    public async Task<string> GetUtcOffsetAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.UtcOffset);
        }

        string cacheKey = $"utc-offset@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string utcOffset = await ipApiService.GetUtcOffsetAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, utcOffset);

        return await Task.FromResult(utcOffset);
    }

    public async Task<string> GetOrgAsync(string ipAddress)
    {
        if (cacheService.TryGetValue(ipAddress, out IpGeolocationModel cachedIpGeolocationModel))
        {
            return await Task.FromResult(cachedIpGeolocationModel.Org);
        }

        string cacheKey = $"org@{ipAddress}";
        
        if (cacheService.TryGetValue(cacheKey, out string cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }
        
        string org = await ipApiService.GetOrgAsync(ipAddress);

        await cacheService.SetAsync(cacheKey, org);

        return await Task.FromResult(org);
    }
}