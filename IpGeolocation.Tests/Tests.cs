using CacheDrive.Services;
using IpGeolocation.Extensions;
using IpGeolocation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeolocation.Tests;

public class Tests
{
    private ServiceCollection _services;
    private ServiceProvider _serviceProvider;
    
    [OneTimeSetUp]
    public void Init()
    {
        _services = new ServiceCollection();

        var testConfiguration = new Dictionary<string, string>
        {
            {"IpGeolocationSettings:BaseAddress", "https://ipapi.co/"}, // default value, not need to be provided
            {"IpGeolocationSettings:UserAgent", "ipapi.co/#c-sharp-v1.04"}, // default value, not need to be provided
            {"CacheSettings:CacheEnabled", "true"},
            {"CacheSettings:CacheExpirationType", "Minutes"},
            {"CacheSettings:CacheExpiration", "60"},
            {"CacheSettings:CacheType", "MemoryAndFile"},
            {"CacheSettings:InitializeOnStartup", "true"},
            {"CacheSettings:FlushOnExit", "true"},
            {"CacheSettings:HashKeySalt", "Secret123Secret"}
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(testConfiguration).Build();
        _services.UseIpGeolocation(configuration);
        
        _serviceProvider = _services.BuildServiceProvider();
    }
    
    [OneTimeTearDown]
    public void Cleanup()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public async Task ArgumentException_Should_Be_Throw_When_IpAddress_Is_Empty()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();

        Assert.ThrowsAsync<ArgumentException>(async() => await ipGeolocationService.GetIpGeolocationAsync(""));
    }
    
    [Test]
    public async Task ArgumentNullException_Should_Be_Throw_When_IpAddress_Is_Null()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();

        Assert.ThrowsAsync<ArgumentNullException>(async() => await ipGeolocationService.GetIpGeolocationAsync(null));
    }
}