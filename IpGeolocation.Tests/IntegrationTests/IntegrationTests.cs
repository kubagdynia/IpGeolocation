using FluentAssertions;
using IpGeolocation.Extensions;
using IpGeolocation.Models;
using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeolocation.Tests.IntegrationTests;

public class IntegrationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Service_Should_Return_Correct_FullData_For_Specific_Ip()
    {
        ServiceCollection services = new ServiceCollection();
        services.RegisterIpGeolocation();
        
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        IIpGeolocationService ipGeolocationService = serviceProvider.GetRequiredService<IIpGeolocationService>();

        IpGeolocationModel? ipResultData =  await ipGeolocationService.GetIpGeolocationAsync("8.8.8.8");
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
}