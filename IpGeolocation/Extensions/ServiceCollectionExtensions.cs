using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeolocation.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterIpGeolocation(this IServiceCollection services)
    {
        services.AddTransient<IIpGeolocationService, IpGeolocationService>();
    }
}