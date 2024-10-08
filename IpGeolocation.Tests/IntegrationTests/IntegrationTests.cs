using CacheDrive.Services;
using FluentAssertions;
using IpGeolocation.Extensions;
using IpGeolocation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeolocation.Tests.IntegrationTests;

[Category("IntegrationTests")]
public class IntegrationTests
{
    private ServiceCollection _services;
    private ServiceProvider _serviceProvider;

    [OneTimeSetUp]
    public void Init()
    {
        _services = new ServiceCollection();

        var testConfiguration = new Dictionary<string, string>
        {
            {"IpGeolocationSettings:BaseAddress", "https://ipapi.co/"},
            {"CacheSettings:CacheEnabled", "true"},
            {"CacheSettings:CacheExpirationType", "Minutes"},
            {"CacheSettings:CacheExpiration", "60"},
            {"CacheSettings:CacheType", "MemoryAndFile"},
            {"CacheSettings:InitializeOnStartup", "true"},
            {"CacheSettings:FlushOnExit", "true"},
            {"CacheSettings:HashKeySalt", "Secret123Secret"},
            {"CacheSettings:ApiKey", ""}
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

    [Test, Order(1)]
    public async Task Service_Should_Return_Correct_FullData_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();
        
        var ipResultData = await ipGeolocationService.GetIpGeolocationAsync("8.8.8.8");
        ipResultData.Should().NotBeNull();

        ipResultData.City.Should().Be("Mountain View");
        ipResultData.ContinentCode.Should().Be("NA");
        ipResultData.Country.Should().Be("US");
        ipResultData.CountryCapital.Should().Be("Washington");
        ipResultData.CountryCode.Should().Be("US");
        ipResultData.CountryCodeIso3.Should().Be("USA");
        ipResultData.CountryName.Should().Be("United States");
        ipResultData.Currency.Should().Be("USD");
        ipResultData.CurrencyName.Should().Be("Dollar");
        ipResultData.Region.Should().Be("California");
        ipResultData.RegionCode.Should().Be("CA");
        //ipResultData.UtcOffset.Should().Be("-0700"); // commented out because it changes in time as part of the time change
        ipResultData.Org.Should().Be("GOOGLE");
        
        await cacheService.FlushAsync();
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Country_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();
        
        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();

        var result = await ipGeolocationService.GetCountryAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("US");
        
        await cacheService.FlushAsync();
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Country_Name_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();
        
        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();

        var result = await ipGeolocationService.GetCountryNameAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("United States");
        
        await cacheService.FlushAsync();
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_City_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetCityAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("Mountain View");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Currency_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetCurrencyAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("USD");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CurrencyName_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetCurrencyNameAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("Dollar");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Region_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetRegionAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("California");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_RegionCode_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetRegionCodeAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("CA");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_ContinentCode_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetContinentCodeAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("NA");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CountryCode_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetCountryCodeAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("US");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CountryCodeIso3_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetCountryCodeIso3Async("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("USA");
    }
    
    [Test]
    [Ignore("Ignore because UTC offset changes in time as part of the time change")]
    public async Task Service_Should_Return_Correct_UtcOffset_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetUtcOffsetAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("-0700");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Org_For_Specific_Ip()
    {
        var ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        var result = await ipGeolocationService.GetOrgAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("GOOGLE");
    }
}