namespace IpGeolocation.Configuration;

public class IpGeolocationSettings
{
    public string BaseAddress { get; set; } = "https://ipapi.co/";
    public bool CacheEnabled { get; set; } = true;
    public int CacheExpirationTimeInMinutes { get; set; } = 1;
}