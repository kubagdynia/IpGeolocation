using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using IpGeolocation.Exceptions;
using IpGeolocation.Models;
using Microsoft.Extensions.Logging;
using Polly.Timeout;

namespace IpGeolocation.Services;

public class IpApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IpGeolocationService> _logger;

    public IpApiService(HttpClient httpClient, ILogger<IpGeolocationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.BaseAddress = new Uri("https://ipapi.co/");
        
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "ipapi.co/#c-sharp-v1.04");
    }

    public async Task<IpGeolocationModel> GetFullDataAsync(string ipAddress)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{ipAddress}/json");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<IpGeolocationModel>();
                }
                catch (NotSupportedException ex) // When content type is not valid
                {
                    _logger.LogError(ex, "The content type is not supported.");
                    throw;
                }
                catch (JsonException ex) // Invalid JSON
                {
                    _logger.LogError(ex, "Invalid JSON.");
                    throw;
                }
            }
        }
        catch (TimeoutRejectedException ex)
        {
            _logger.LogError(ex, "Timeout has occurred.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");
            throw;
        }

        return await Task.FromResult<IpGeolocationModel>(null);
    }

    public async Task<string> GetCityAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "city");
    }
    
    public async Task<string> GetContinentCodeAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "continent_code");
    }

    public async Task<string> GetCountryAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "country");
    }
    
    public async Task<string> GetCountryCodeAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "country_code");
    }
    
    public async Task<string> GetCountryCodeIso3Async(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "country_code_iso3");
    }
    
    public async Task<string> GetTimezoneAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "timezone");
    }
    
    public async Task<string> GetLanguagesAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "languages");
    }
    
    public async Task<string> GetCurrencyAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "currency");
    }
    
    public async Task<string> GetCurrencyNameAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "currency_name");
    }
    
    public async Task<string> GetRegionAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "region");
    }
    
    public async Task<string> GetRegionCodeAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "region_code");
    }
    
    public async Task<string> GetUtcOffsetAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "utc_offset");
    }
    
    public async Task<string> GetOrgAsync(string ipAddress)
    {
        return await GetSpecificFieldAsync(ipAddress, "org");
    }

    private async Task<string> GetSpecificFieldAsync(string ipAddress, string fieldType)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{ipAddress}/{fieldType}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadAsStringAsync();
                }
                catch (NotSupportedException ex) // When content type is not valid
                {
                    _logger.LogError(ex, "The content type is not supported.");
                    throw;
                }
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new QuotaExceededException(response.ReasonPhrase);
            }
        }
        catch (TimeoutRejectedException ex)
        {
            _logger.LogError(ex, "Timeout has occurred.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");
            throw;
        }

        return await Task.FromResult<string>(null);
    }
}