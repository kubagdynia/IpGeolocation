# IpGeolocation

[![CI](https://img.shields.io/github/actions/workflow/status/kubagdynia/IpGeolocation/dotnet.yml?branch=main)](https://github.com/kubagdynia/IpGeolocation/actions?query=branch%3Amain) [![NuGet Version](https://img.shields.io/nuget/v/JK.IpGeolocation.svg?style=flat)](https://www.nuget.org/packages/JK.IpGeolocation/)

IpGeolocation is a simple library that allows you to get geolocation data based on IP address. It uses ipapi.co API to get the data. The library is written in C# and is available as a NuGet package. 


### Project structure

- IpGeolocation - IpGeolocation library
- IpGeolocation.ConsoleApp - console application with example use of the IpGeolocation library
- IpGeolocation.ConsoleApp.WithoutConfig - console application with example use of the IpGeolocation library without configuration
- IpGeolocation.Tests - unit tests

### Installation

You can install the library via NuGet package manager:

```
Install-Package JK.IpGeolocation
```

or .NET CLI:

```
dotnet add package JK.IpGeolocation
```

or just copy into the project file to reference the package:

```
<PackageReference Include="JK.IpGeolocation" Version="0.4.2" />
```

### Usage

To use the library you need to create an instance of the `IpGeolocationService` class and call the `GetGeolocationData` method with the IP address as an argument. The method returns an instance of the `GeolocationData` class with the geolocation data.

```csharp
// create services
var services = new ServiceCollection();

// add and register
services.UseIpGeolocation(settings: new IpGeolocationSettings { CacheType = CacheType.MemoryAndFile });

// build service provider
var serviceProvider = services.BuildServiceProvider();

// get service and call method to get geolocation
var ipGeolocation = await serviceProvider.GetRequiredService<IIpGeolocationService>().GetIpGeolocationAsync("8.8.8.8");

// print geolocation data 
Console.WriteLine(ipGeolocation.CountryName); // United States

// dispose service provider to release resources
serviceProvider.Dispose();
```

### Configuration

The library allows you to configure the cache type and the cache expiration time. You can do this by creating an instance of the `IpGeolocationSettings` class and passing
it to the `UseIpGeolocation` method.

```csharp
services.UseIpGeolocation(settings: services.UseIpGeolocation(settings: new IpGeolocationSettings
    { CacheType = CacheType.MemoryAndFile, CacheExpirationType = CacheExpirationType.Minutes, CacheExpiration = 60 });
```

### Cache types

The library supports two cache types:

- `Memory` - cache data in memory
- `MemoryAndFile` - cache data in memory and file

### Cache expiration types

The library supports two cache expiration types:

- `Seconds` - cache expiration time in seconds
- `Minutes` - cache expiration time in minutes
- `Hours` - cache expiration time in hours
- `Days` - cache expiration time in days
- `Never` - cache never expires

### Cache

More information about the cache can be found in the [CacheDrive](https://github.com/kubagdynia/CacheDrive) library.

### Usage with configuration file

The library allows you to use configuration files to configure the library. You can do this by adding the configuration section to the `appsettings.json` file.

```json
{
  "IpGeolocationSettings": {
    "BaseAddress": "https://ipapi.co/",
    "CacheEnabled": true,
    "CacheFolderName": "cache",
    "CacheExpirationType": "Minutes",
    "CacheExpiration": 60,
    "CacheType": "MemoryAndFile",
    "InitializeOnStartup": true,
    "FlushOnExit": true,
    "HashKeySalt": "123s123",
    "ApiKey": ""
  }
}
```

- BaseAddress - Base address of the ipapi.co API
- CacheEnabled - Determines whether cache is enabled. Default value is true.
- CacheFolderName - Cache folder name. Default value is "cache".
- CacheExpirationType - In what units do we specify cache expiration. Default value is Minutes.
- CacheExpiration - After what time the objects in the cache will expire. Based on CacheExpirationType. Default value is 60.
- CacheType - Method of storing the cache. Memory - only in memory. MemoryAndFile - In memory while the application is running and in files after the application is closed. Default value is Memory.
- InitializeOnStartup - Determines whether the cache should be initialized at startup. Default value is true.
- FlushOnExit - Determines whether the cache should be flushed to a files when the application is closed. Default value is true.
- HashKeySalt - Salt, which will be added to the key hash. Default value is an empty string, which means that adding salt is disabled.
- ApiKey - API key for the paid version of the ipapi.co API. Default value is an empty string, which means that the free version of the ipapi.co API will be used.

Then you can use the `UseIpGeolocation` method to add and register the library with the configuration.

```csharp
// create services
var services = new ServiceCollection();

// build config
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// add and register services:
services.UseIpGeolocation(configuration);

// build service provider
var serviceProvider = services.BuildServiceProvider();

// get service and call method to get geolocation
var ipGeolocation = await serviceProvider.GetRequiredService<IIpGeolocationService>().GetIpGeolocationAsync("8.8.8.8");

// print geolocation data 
Console.WriteLine(ipGeolocation.CountryName); // United States

// dispose service provider to release resources
serviceProvider.Dispose();
```

### How to use paid version of the ipapi.co API

If you want to use the paid version of the ipapi.co API, you need to provide the API key in the configuration file or in the `IpGeolocationSettings` class.

```json
{
  "IpGeolocationSettings": {
    "ApiKey": "your_api_key"
  }
}
```

If you leave the `ApiKey` property empty or do not provide it, the library will use the free version of the ipapi.co API.

### Code Examples

- IpGeolocation.ConsoleApp - console application with example use of the IpGeolocation library
  https://github.com/kubagdynia/IpGeolocation/tree/main/IpGeolocation.ConsoleApp
- IpGeolocation.ConsoleApp.WithoutConfig - console application with example use of the IpGeolocation library without configuration
  https://github.com/kubagdynia/IpGeolocation/tree/main/IpGeolocation.ConsoleApp.WithoutConfig

### License

The library is available under the [MIT](https://opensource.org/licenses/MIT) license.