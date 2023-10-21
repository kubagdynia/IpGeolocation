using FluentAssertions;
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
    public void GlobalSetup()
    {
        _services = new ServiceCollection();
        _services.RegisterIpGeolocation();
        _serviceProvider = _services.BuildServiceProvider();
    }

    [Test]
    public async Task Service_Should_Return_Correct_FullData_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        IpGeolocationModel ipResultData =  await ipGeolocationService.GetIpGeolocationAsync("8.8.8.8");
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
        ipResultData.UtcOffset.Should().Be("-0700");
        ipResultData.Org.Should().Be("GOOGLE");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Country_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string country =  await ipGeolocationService.GetCountryAsync("8.8.8.8");
        country.Should().NotBeNull();
        country.Should().Be("US");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_City_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string city =  await ipGeolocationService.GetCityAsync("8.8.8.8");
        city.Should().NotBeNull();
        city.Should().Be("Mountain View");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_Currency_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string currency =  await ipGeolocationService.GetCurrencyAsync("8.8.8.8");
        currency.Should().NotBeNull();
        currency.Should().Be("USD");
    }
    
    [Test]
    public async Task Service_Should_Return_Correct_CurrencyName_For_Specific_Ip()
    {
        IIpGeolocationService ipGeolocationService = _serviceProvider.GetRequiredService<IIpGeolocationService>();

        string currencyName =  await ipGeolocationService.GetCurrencyNameAsync("8.8.8.8");
        currencyName.Should().NotBeNull();
        currencyName.Should().Be("Dollar");
    }
}