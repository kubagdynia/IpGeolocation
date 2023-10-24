using IpGeolocation.Models;

namespace IpGeolocation.Services;

public interface IIpGeolocationService
{
    /// <summary>
    /// Returns the complete location information for an IP address
    /// </summary>
    Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress);
    
    /// <summary>
    /// Country code (2 letter, ISO 3166-1 alpha-2)
    /// </summary>
    Task<string> GetCountryAsync(string ipAddress);
    
    /// <summary>
    /// Country code (2 letter, ISO 3166-1 alpha-2)
    /// </summary>
    Task<string> GetCountryCodeAsync(string ipAddress);
    
    /// <summary>
    /// Country code (3 letter, ISO 3166-1 alpha-3)
    /// </summary>
    Task<string> GetCountryCodeIso3Async(string ipAddress);
    
    /// <summary>
    /// City name
    /// </summary>
    Task<string> GetCityAsync(string ipAddress);
    
    /// <summary>
    /// Continent code
    /// </summary>
    Task<string> GetContinentCodeAsync(string ipAddress);
    
    /// <summary>
    /// Timezone (IANA format i.e. “Area/Location”)
    /// </summary>
    Task<string> GetTimezoneAsync(string ipAddress);
    
    /// <summary>
    /// Languages spoken (comma separated 2 or 3 letter ISO 639 code with optional hyphen separated country suffix)
    /// </summary>
    Task<string> GetLanguagesAsync(string ipAddress);
    
    /// <summary>
    /// Currency code (ISO 4217)
    /// </summary>
    Task<string> GetCurrencyAsync(string ipAddress);
    
    /// <summary>
    /// Currency name
    /// </summary>
    Task<string> GetCurrencyNameAsync(string ipAddress);
    
    /// <summary>
    /// Region name (administrative division)
    /// </summary>
    Task<string> GetRegionAsync(string ipAddress);
    
    /// <summary>
    /// Region code
    /// </summary>
    Task<string> GetRegionCodeAsync(string ipAddress);
    
    /// <summary>
    /// UTC offset (with daylight saving time) as +HHMM or -HHMM (HH is hours, MM is minutes)
    /// </summary>
    Task<string> GetUtcOffsetAsync(string ipAddress);
    
    /// <summary>
    /// Organization name
    /// </summary>
    Task<string> GetOrgAsync(string ipAddress);
}