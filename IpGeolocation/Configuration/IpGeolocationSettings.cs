using CacheDrive.Configuration;

namespace IpGeolocation.Configuration;

public class IpGeolocationSettings : CacheSettings
{
    /// <summary>
    /// The name of the configuration section in the appsettings.json file.
    /// </summary>
    public const string AppConfigSectionName = "IpGeolocationSettings";
    
    /// <summary>
    /// The base address of the API.
    /// </summary>
    public string BaseAddress { get; set; } = "https://ipapi.co/";

    /// <summary>
    /// The user agent to be used in the request header.
    /// </summary>
    public string UserAgent { get; set; } = "ipapi.co/#c-sharp-v1.04";
    
    /// <summary>
    /// API key for the paid version of the ipapi.co API.
    /// Default value is an empty string, which means that the free version of the ipapi.co API will be used.
    /// </summary>
    public string ApiKey { get; set; }
}