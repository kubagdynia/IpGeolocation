namespace IpGeolocation.Cache;

public class CacheSettings
{
    public int CacheExpiration { get; set; } = 60;

    public CacheExpirationType CacheExpirationType { get; set; } = CacheExpirationType.Minutes;

    public CacheType CacheType { get; set; } = CacheType.Memory;
}

public enum CacheType
{
    Memory,
    MemoryAndFile
}

public enum CacheExpirationType
{
    Seconds,
    Minutes,
    Hours,
    Days,
    Never
}