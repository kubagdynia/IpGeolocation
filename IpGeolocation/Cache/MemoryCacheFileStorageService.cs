using System.Text.Json;
using System.Text.RegularExpressions;
using IpGeolocation.Configuration;
using Microsoft.Extensions.Options;

namespace IpGeolocation.Cache;

public class MemoryCacheFileStorageService : MemoryCacheService
{
    public MemoryCacheFileStorageService(IOptions<IpGeolocationSettings> ipGeolocationSettings)
        : base(ipGeolocationSettings)
    {
        CreateCacheDirectory();
    }
    
    public override async Task FlushAsync()
    {
        foreach ((string key, CachedItem item) in Storage)
        {
            if (!item.Dirty || item.Expired())
                continue;

            var cachedItem = JsonSerializer.Serialize(item, CachedItem.JsonOptions);

            var safeFilename = SafeFilenameRegex().Replace(key, string.Empty);
            
            var path = CachePath($"{safeFilename}.json");

            await using StreamWriter sw = File.CreateText(path);
            await sw.WriteLineAsync(cachedItem);
            item.Dirty = false;
        }
    }

    public override async Task InitializeAsync()
    {
        var dirs = Directory.GetFiles(GetCacheDirectory(), "*.json");
        
        foreach (var file in dirs)
        {
            var jsonString = await File.ReadAllTextAsync(file);

            var cachedItem = JsonSerializer.Deserialize<CachedItem>(jsonString, CachedItem.JsonOptions);

            if (cachedItem is null) return;

            if (cachedItem.Expired())
            {
                File.Delete(file);
                continue;
            }

            await SetAsync(cachedItem);
        }
    }
    
    private Regex _safeFilenameRegex;
    
    private Regex SafeFilenameRegex()
    {
        if (_safeFilenameRegex is null)
        {
            var regSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            _safeFilenameRegex = new Regex($"[{Regex.Escape(regSearch)}]");
        }

        return _safeFilenameRegex;
    }
    
    private string CachePath(string fileName)
        => Path.Combine(Environment.CurrentDirectory, "cache", fileName);

    private string GetCacheDirectory()
        => Path.Combine(Environment.CurrentDirectory, "cache");
    
    private void CreateCacheDirectory()
    {
        var folderName = GetCacheDirectory();
        if (!string.IsNullOrEmpty(folderName) && !Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
        }
    }
}