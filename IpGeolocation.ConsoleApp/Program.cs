using CommandLine;
using IpGeolocation.ConsoleApp;
using IpGeolocation.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Options = IpGeolocation.ConsoleApp.Options;

var services = new ServiceCollection();

ConfigureServices(services);

string ipAddress = CheckCommandLineOptions(args);

// create service provider
var serviceProvider = services.BuildServiceProvider();

// entry to run app
// ReSharper disable once PossibleNullReferenceException
await serviceProvider.GetService<App>()!.Run(ipAddress);

serviceProvider.Dispose();

void ConfigureServices(IServiceCollection serviceCollection)
{
    // build config
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
    
    // defining Serilog configs
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    
    // configure logging
    serviceCollection.AddLogging(builder =>
    { 
        builder.AddSerilog();
    });
    
    // add and register services:
    services.RegisterIpGeolocation(configuration);
    
    // add app
    services.AddTransient<App>();
}

static string CheckCommandLineOptions(string[] args)
{
    string ipAddr = null;
    Parser.Default.ParseArguments<Options>(args)
        .WithParsed(opt =>
        {
            ipAddr = opt.IpAddress;
        })
        .WithNotParsed(_ =>
        {
            // in case of parameter parsing errors or using help option close the application
            Environment.Exit(0);
        });
    return ipAddr;
}