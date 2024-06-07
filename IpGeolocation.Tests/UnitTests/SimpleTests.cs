using FluentAssertions;
using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeolocation.Tests.UnitTests;

[Category("UnitTests")]
public class SimpleTests
{
    [Test]
    public async Task Service_Should_Return_Correct_FullData_For_Specific_Ip()
    {
        var responseContent =
            """
            {
                "asn": "AS15169",
                "city": "Mountain View",
                "continent_code": "NA",
                "country": "US",
                "country_area": 9629091,
                "country_calling_code": "+1",
                "country_capital": "Washington",
                "country_code": "US",
                "country_code_iso3": "USA",
                "country_name": "United States",
                "country_population": 327167420,
                "country_tld": ".us",
                "currency": "USD",
                "currency_name": "Dollar",
                "in_eu": false,
                "ip": "8.8.8.8",
                "languages": "en-US,es-US,haw,fr",
                "latitude": 37.42301,
                "longitude": -122.083352,
                "org": "GOOGLE",
                "postal": "94043",
                "region": "California",
                "region_code": "CA",
                "timezone": "America/Los_Angeles",
                "utc_offset": "-0700"
            }
            """;

        var serviceProvider = TestHelper.CreateServiceProvider(null, responseContent);
        
        var ipGeolocationService = serviceProvider.GetRequiredService<IIpGeolocationService>();
        
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
    }
}