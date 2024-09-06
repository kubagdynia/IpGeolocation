using CacheDrive.Configuration;
using IpGeolocation.Configuration;
using IpGeolocation.Extensions;
using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;

// create services
var services = new ServiceCollection();

// add and register
services.UseIpGeolocation(settings: new IpGeolocationSettings
    { CacheType = CacheType.MemoryAndFile, CacheExpirationType = CacheExpirationType.Minutes, CacheExpiration = 60});

// build service provider
var serviceProvider = services.BuildServiceProvider();

// get service and call method to get geolocation
var ipGeolocation = await serviceProvider.GetRequiredService<IIpGeolocationService>().GetIpGeolocationAsync("8.8.8.8");

// print geolocation data 
Console.WriteLine(ipGeolocation.CountryName); // United States

// dispose service provider to release resources
serviceProvider.Dispose();
