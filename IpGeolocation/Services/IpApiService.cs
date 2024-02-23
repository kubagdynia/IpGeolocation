using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using IpGeolocation.Exceptions;
using IpGeolocation.Models;
using Microsoft.Extensions.Logging;
using Polly.Timeout;

namespace IpGeolocation.Services;

internal class IpApiService
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
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            
            using var response = await _httpClient.GetAsync($"{ipAddress}/json");

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

        return await Task.FromResult<IpGeolocationModel>(null);
    }

    public Task<string> GetCityAsync(string ipAddress)
        => GetValueAsync(ipAddress, "city");
    
    public Task<string> GetContinentCodeAsync(string ipAddress) 
        => GetValueAsync(ipAddress, "continent_code");

    public Task<string> GetCountryAsync(string ipAddress)
        => GetValueAsync(ipAddress, "country");
    
    public Task<string> GetCountryNameAsync(string ipAddress)
        => GetValueAsync(ipAddress, "country_name");
    
    public Task<string> GetCountryCodeAsync(string ipAddress)
        => GetValueAsync(ipAddress, "country_code");
    
    public Task<string> GetCountryCodeIso3Async(string ipAddress)
        => GetValueAsync(ipAddress, "country_code_iso3");
    
    public Task<string> GetTimezoneAsync(string ipAddress)
        => GetValueAsync(ipAddress, "timezone");
    
    public Task<string> GetLanguagesAsync(string ipAddress)
        => GetValueAsync(ipAddress, "languages");
    
    public Task<string> GetCurrencyAsync(string ipAddress)
        => GetValueAsync(ipAddress, "currency");
    
    public Task<string> GetCurrencyNameAsync(string ipAddress)
        => GetValueAsync(ipAddress, "currency_name");
    
    public Task<string> GetRegionAsync(string ipAddress)
        => GetValueAsync(ipAddress, "region");
    
    public Task<string> GetRegionCodeAsync(string ipAddress)
        => GetValueAsync(ipAddress, "region_code");
    
    public Task<string> GetUtcOffsetAsync(string ipAddress)
        => GetValueAsync(ipAddress, "utc_offset");
    
    public Task<string> GetOrgAsync(string ipAddress)
        => GetValueAsync(ipAddress, "org");

    private async Task<string> GetValueAsync(string ipAddress, string fieldType)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            
            using var response = await _httpClient.GetAsync($"{ipAddress}/{fieldType}");

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