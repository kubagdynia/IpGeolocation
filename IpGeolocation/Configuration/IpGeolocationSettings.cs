using CacheDrive.Configuration;

namespace IpGeolocation.Configuration;

public class IpGeolocationSettings : CacheSettings
{
    public string BaseAddress { get; set; } = "https://ipapi.co/";
}