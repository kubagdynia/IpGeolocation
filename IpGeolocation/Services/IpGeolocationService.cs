using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using IpGeolocation.Models;
using Polly.Timeout;

namespace IpGeolocation.Services;

public class IpGeolocationService : IIpGeolocationService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IpGeolocationService> _logger;

    public IpGeolocationService(IHttpClientFactory clientFactory, ILogger<IpGeolocationService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }
    
    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ip)
    {
        string requestUri = $"https://ipapi.co/{ip}/json";
        
        var client = _clientFactory.CreateClient("ClientWithoutSSLValidation");
        client.DefaultRequestHeaders.Add("User-Agent", "ipapi.co/#c-sharp-v1.03");

        try
        {
            using HttpResponseMessage response = await client.GetAsync(requestUri);
            
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    IpGeolocationModel ipGeolocation = await response.Content.ReadFromJsonAsync<IpGeolocationModel>();
                    return ipGeolocation;
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
                _logger.LogError($"TooManyRequests: {response.ReasonPhrase}");
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
}