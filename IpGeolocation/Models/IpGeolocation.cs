using System.Text.Json.Serialization;
using IpGeolocation.Cache;

namespace IpGeolocation.Models;

public class IpGeolocationModel : ICacheable
{
    /// <summary>
    /// Public (external) IP address (same as URL ip), e.g. "8.8.8.8"
    /// </summary>
    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    /// <summary>
    /// City name, e.g. "Mountain View"
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// Region name (administrative division), e.g. "California"
    /// </summary>
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// Region code, e.g. "CA"
    /// </summary>
    [JsonPropertyName("region_code")]
    public string RegionCode { get; set; }

    /// <summary>
    /// Country code (2 letter, ISO 3166-1 alpha-2), e.g. "US"
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; }

    /// <summary>
    /// Country code (2 letter, ISO 3166-1 alpha-2), e.g. "US"
    /// </summary>
    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }

    /// <summary>
    /// Country code (3 letter, ISO 3166-1 alpha-3), e.g. "USA"
    /// </summary>
    [JsonPropertyName("country_code_iso3")]
    public string CountryCodeIso3 { get; set; }

    /// <summary>
    /// Short country name, e.g. "United States"
    /// </summary>
    [JsonPropertyName("country_name")]
    public string CountryName { get; set; }

    /// <summary>
    /// Capital of the country, e.g. "Washington"
    /// </summary>
    [JsonPropertyName("country_capital")]
    public string CountryCapital { get; set; }

    /// <summary>
    /// Country specific TLD (top-level domain), e.g. ".us"
    /// </summary>
    [JsonPropertyName("country_tld")]
    public string CountryTld { get; set; }

    /// <summary>
    /// Area of the country (in sq km), e.g. 9629091.0
    /// </summary>
    [JsonPropertyName("country_area")]
    public float CountryArea { get; set; }

    /// <summary>
    /// Population of the country, e.g. 327167434
    /// </summary>
    [JsonPropertyName("country_population")]
    public float CountryPopulation { get; set; }

    /// <summary>
    /// Continent code, e.g. "NA"
    /// </summary>
    [JsonPropertyName("continent_code")]
    public string ContinentCode { get; set; }

    /// <summary>
    /// whether IP address belongs to a country that is a member of the European Union (EU), e.g. false
    /// </summary>
    [JsonPropertyName("in_eu")]
    public bool InEu { get; set; }

    /// <summary>
    /// Postal code / zip code, e.g. "94043"
    /// </summary>
    [JsonPropertyName("postal")]
    public string Postal { get; set; }

    /// <summary>
    /// Latitude, e.g. 37.42301 
    /// </summary>
    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }

    /// <summary>
    /// Longitude, e.g. -122.083352
    /// </summary>
    [JsonPropertyName("longitude")]
    public decimal Longitude { get; set; }

    /// <summary>
    /// Timezone (IANA format i.e. “Area/Location”), e.g. "America/Los_Angeles"
    /// </summary>
    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    /// <summary>
    /// Timezone (IANA format i.e. “Area/Location”), e.g. "America/Los_Angeles"
    /// </summary>
    [JsonPropertyName("utc_offset")]
    public string UtcOffset { get; set; }

    /// <summary>
    /// Country calling code (dial in code, comma separated), e.g. "+1"
    /// </summary>
    [JsonPropertyName("country_calling_code")]
    public string CountryCallingCode { get; set; }

    /// <summary>
    /// Currency code (ISO 4217), e.g. "USD"
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Currency name, e.g. "Dollar"
    /// </summary>
    [JsonPropertyName("currency_name")]
    public string CurrencyName { get; set; }

    /// <summary>
    /// Languages spoken (comma separated 2 or 3 letter ISO 639 code with optional hyphen separated country suffix), e.g. "en-US,es-US,haw,fr"
    /// </summary>
    [JsonPropertyName("languages")]
    public string Languages { get; set; }

    /// <summary>
    /// Autonomous system number, e.g. "AS15169"
    /// </summary>
    [JsonPropertyName("asn")]
    public string Asn { get; set; }

    /// <summary>
    /// Organization name, e.g. "GOOGLE"
    /// </summary>
    [JsonPropertyName("org")]
    public string Org { get; set; }

    [JsonIgnore]
    public string CacheKey
        => GetCacheKey(Ip);
    
    public static string GetCacheKey(string ip)
        => $"ip-geolocation@{ip}";
}