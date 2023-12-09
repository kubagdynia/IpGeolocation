using CacheDrive.Services;
using FluentAssertions;
using IpGeolocation.Configuration;
using IpGeolocation.Extensions;
using IpGeolocation.Models;
using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeolocation.Tests.IntegrationTests;

public class IntegrationTests
{
    private ServiceCollection _services;
    private ServiceProvider _serviceProvider;

    [OneTimeSetUp]
    public void Init()
    {
        _services = new ServiceCollection();
        _services.RegisterIpGeolocation(new IpGeolocationSettings());
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
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        ICacheService cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();
        
        IpGeolocationModel ipResultData = await ipGeolocationService.GetIpGeolocationAsync("8.8.8.8");
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
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();
        
        ICacheService cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.InitializeAsync();

        string result = await ipGeolocationService.GetCountryAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("US");
        
        await cacheService.FlushAsync();
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_City_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetCityAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("Mountain View");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Currency_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetCurrencyAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("USD");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CurrencyName_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetCurrencyNameAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("Dollar");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Region_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetRegionAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("California");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_RegionCode_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetRegionCodeAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("CA");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_ContinentCode_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetContinentCodeAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("NA");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CountryCode_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetCountryCodeAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("US");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CountryCodeIso3_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetCountryCodeIso3Async("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("USA");
    }
    
    [Test]
    [Ignore("Ignore because UTC offset changes in time as part of the time change")]
    public async Task Service_Should_Return_Correct_UtcOffset_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetUtcOffsetAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("-0700");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Org_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string result = await ipGeolocationService.GetOrgAsync("8.8.8.8");
        result.Should().NotBeNull();
        result.Should().Be("GOOGLE");
    }
}