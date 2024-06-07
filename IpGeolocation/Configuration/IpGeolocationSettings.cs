using CacheDrive.Configuration;

namespace IpGeolocation.Configuration;

public class IpGeolocationSettings : CacheSettings
{
    public const string AppConfigSectionName = "IpGeolocationSettings";
    
    public string BaseAddress { get; set; } = "https://ipapi.co/";

    public string UserAgent { get; set; } = "ipapi.co/#c-sharp-v1.04";
}