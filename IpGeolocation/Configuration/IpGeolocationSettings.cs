using IpGeolocation.Cache;

namespace IpGeolocation.Configuration;

public class IpGeolocationSettings : CacheSettings
{
    public string BaseAddress { get; set; } = "https://ipapi.co/";
    public bool CacheEnabled { get; set; } = true;
}