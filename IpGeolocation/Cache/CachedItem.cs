using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;

namespace IpGeolocation.Cache;

public class CachedItem
{
    public string Key { get; set; }
    
    public DateTime Cached { get; set; }
    
    public DateTime Expires { get; set; }
    
    public JsonElement Contents { get; set; }
    
    [JsonIgnore]
    public bool Dirty { get; set; }
    
    private object _unwrapped;
    
    internal static CachedItem FromCacheable<T>(T item, TimeSpan expiry, bool dirty = false) where T : ICacheable
    {
        return new CachedItem
        {
            Cached = DateTime.UtcNow,
            Expires = DateTime.UtcNow + expiry,
            Key = item.CacheKey,
            Contents = JsonSerializer.SerializeToElement(item, JsonOptions),
            Dirty = dirty
        };
    }
    
    internal bool Expired()
        => Expires < DateTime.UtcNow;

    internal void Invalidate()
    {
        _unwrapped = null;
        Dirty = true;
    }
    
    internal T Unwrap<T>() where T : ICacheable
    {
        _unwrapped ??= Contents.Deserialize<T>(JsonOptions);
        return (T)_unwrapped;
    }
    
    public static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}