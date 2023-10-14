using IpGeolocation.Models;

namespace IpGeolocation.Services;

public interface IIpGeolocationService
{
    Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress);
    Task<string> GetCountryAsync(string ipAddress);
    Task<string> GetCityAsync(string ipAddress);
    Task<string> GetTimezoneAsync(string ipAddress);
    Task<string> GetLanguagesAsync(string ipAddress);
    Task<string> GetCurrencyAsync(string ipAddress);
    Task<string> GetCurrencyNameAsync(string ipAddress);
}