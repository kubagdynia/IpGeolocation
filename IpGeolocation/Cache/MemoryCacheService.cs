using System.Collections.Concurrent;
using System.Text.Json;

namespace IpGeolocation.Cache;

public class MemoryCacheService : ICacheService
{
    internal readonly ConcurrentDictionary<string, CachedItem> Storage = new();
    
    public bool HasItem(string key)
    {
        if (Storage.TryGetValue(key, out var cachedItem))
        {
            return !cachedItem.Expired();
        }

        return false;
    }

    public bool TryGetValue<T>(string key, out T value) where T : ICacheable
    {
        if (!Storage.TryGetValue(key, out var cachedItem))
        {
            value = default;
            return false;
        }
        
        if (cachedItem.Expired())
        {
            DeleteAsync(cachedItem);
            value = default;
            return false;
        }
        
        value = cachedItem.Unwrap<T>();
        return true;
    }

    public async Task<T> GetAsync<T>(string key) where T : ICacheable
    {
        var cachedItem = Get(key);

        if (cachedItem == null)
            return default;
        
        if (cachedItem.Expired())
        {
            await DeleteAsync(cachedItem);
            return default; 
        }

        return cachedItem.Unwrap<T>();
    }

    public void Set<T>(T item, int expirySeconds = 0) where T : ICacheable
    {
        var length = expirySeconds == 0 ? TimeSpan.FromSeconds(3600) : TimeSpan.FromSeconds(expirySeconds);
        
        if (Storage.TryGetValue(item.CacheKey, out var cached))
        {
            cached.Contents = JsonSerializer.SerializeToElement(item, CachedItem.JsonOptions);
            cached.Cached = DateTime.UtcNow;
            cached.Expires = cached.Cached.Add(length);
            cached.Invalidate();
            return;
        }
        
        var cachedItem = CachedItem.FromCacheable(item, length, true);
        Set(cachedItem);
    }
    
    public Task SetAsync<T>(T item, int expirySeconds = 0) where T : ICacheable
    {
        var cached = Get(item);

        var length = expirySeconds == 0 ? TimeSpan.FromSeconds(3600) : TimeSpan.FromSeconds(expirySeconds);

        if (cached != null)
        {
            cached.Contents = JsonSerializer.SerializeToElement(item, CachedItem.JsonOptions);
            cached.Cached = DateTime.UtcNow;
            cached.Expires = cached.Cached.Add(length);
            cached.Invalidate();
            return Task.CompletedTask;
        }

        var cachedItem = CachedItem.FromCacheable(item, length, true);

        return SetAsync(cachedItem);
    }
    
    private void Set(CachedItem cachedItem)
    {
        Storage.AddOrUpdate(cachedItem.Key, _ => cachedItem, (_, _) => cachedItem);
    }

    internal Task SetAsync(CachedItem cachedItem)
    {
        Storage.AddOrUpdate(cachedItem.Key, _ => cachedItem, (_, _) => cachedItem);
        return Task.CompletedTask;
    }

    public bool Delete(string key)
        => key is not null && Storage.TryRemove(key, out _);
    
    public Task<bool> DeleteAsync(string key)
        => key is null ? Task.FromResult(false) : Task.FromResult(Storage.TryRemove(key, out _));
    
    private bool Delete(CachedItem item)
        => Delete(item.Key);
    
    private Task<bool> DeleteAsync(CachedItem item)
        => DeleteAsync(item.Key);

    public void DeletePrefix(string prefix)
    {
        var toDeleted = new List<string>();
        
        foreach (var pair in Storage)
        {
            if (pair.Key.StartsWith(prefix))
            {
                toDeleted.Add(pair.Key);
            }
        }

        foreach (var key in toDeleted)
        {
            Delete(key);
        }
    }
    
    public Task DeletePrefixAsync(string prefix)
    {
        var toDeleted = new List<string>();
        
        foreach (var pair in Storage)
        {
            if (pair.Key.StartsWith(prefix))
            {
                toDeleted.Add(pair.Key);
            }
        }

        foreach (var key in toDeleted)
        {
            DeleteAsync(key);
        }

        return Task.CompletedTask;
    }

    public virtual Task FlushAsync()
        => Task.CompletedTask;

    public virtual Task InitializeAsync()
        => Task.CompletedTask;
    
    private CachedItem Get(ICacheable item)
        => Get(item.CacheKey);

    private CachedItem Get(string key)
        => Storage.TryGetValue(key, out var cachedItem) ? cachedItem : null;
}