using CacheDrive.Configuration;
using IpGeolocation.Configuration;
using IpGeolocation.Extensions;
using IpGeolocation.Models;
using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

services.UseIpGeolocation(settings: new IpGeolocationSettings { CacheType = CacheType.MemoryAndFile });
services.AddTransient<App>();

var serviceProvider = services.BuildServiceProvider();

var ipGeolocation = await serviceProvider.GetService<App>()!.GetIpGeolocationAsync("8.8.8.8");

Console.WriteLine(ipGeolocation.CountryName);

serviceProvider.Dispose();

internal class App(IIpGeolocationService ipGeolocationService)
{
    public async Task<IpGeolocationModel> GetIpGeolocationAsync(string ipAddress)
        => await ipGeolocationService.GetIpGeolocationAsync(ipAddress);
}
