using IpGeolocation.Models;

namespace IpGeolocation.Services;

public interface IIpGeolocationService
{
    Task<IpGeolocationModel> GetIpGeolocationAsync(string ip);
}