using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using IpGeolocation.Configuration;
using IpGeolocation.Exceptions;
using IpGeolocation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Timeout;

namespace IpGeolocation.Services;

internal class IpApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IpGeolocationService> _logger;
    private readonly string _apiKey;
    
    private static readonly HttpStatusCode[] FailedResponsesStatusCodes =
        { HttpStatusCode.TooManyRequests, HttpStatusCode.Forbidden };

    public IpApiService(HttpClient httpClient, ILogger<IpGeolocationService> logger, IOptions<IpGeolocationSettings> settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri(settings.Value.BaseAddress);
        
        // Prepare the API key for the query string
        _apiKey = string.IsNullOrEmpty(settings.Value.ApiKey)
            ? string.Empty
            : $"?{GlobalConfig.ApiKeyPropertyName}={settings.Value.ApiKey}";
        
        _httpClient.DefaultRequestHeaders.Add("User-Agent", settings.Value.UserAgent);
    }

    public async Task<IpGeolocationModel> GetFullDataAsync(string ipAddress)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            
            using var response = await _httpClient.GetAsync($"{ipAddress}/json{_apiKey}");

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
            
            await HandleFailedResponsesAsync(response);
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
            
            using var response = await _httpClient.GetAsync($"{ipAddress}/{fieldType}{_apiKey}");

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

            await HandleFailedResponsesAsync(response);
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
    private async Task HandleFailedResponsesAsync(HttpResponseMessage response)
    {
        if (FailedResponsesStatusCodes.Contains(response.StatusCode))
        {
            try
            {
                ApiError apiError = await response.Content.ReadFromJsonAsync<ApiError>();

                if (apiError is { Error: true })
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.TooManyRequests:
                            throw new QuotaExceededException(response.ReasonPhrase);
                        case HttpStatusCode.Forbidden when apiError.Reason != null && apiError.Reason.Equals("invalid key", StringComparison.OrdinalIgnoreCase):
                            throw new InvalidKeyException(apiError.Message);
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON.");
            }
        }
    }
}

internal record ApiError(bool Error, string Reason, string Message);