using CommandLine;
using IpGeolocation.ConsoleApp;
using IpGeolocation.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

var services = new ServiceCollection();

ConfigureServices(services);

string ipAddress = CheckCommandLineOptions(args);

// create service provider
var serviceProvider = services.BuildServiceProvider();

// entry to run app
// ReSharper disable once PossibleNullReferenceException
await serviceProvider.GetService<App>()!.Run(ipAddress);

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
    
    AddHttpClient(serviceCollection);
    
    // add and register services:
    services.RegisterIpGeolocation();
    
    // add app
    services.AddTransient<App>();
}

static void AddHttpClient(IServiceCollection services)
{
    //services.AddHttpClient("ClientWithoutSSLValidation", (_, client) => { client.Timeout = TimeSpan.FromSeconds(10); })
    services.AddHttpClient("ClientWithoutSSLValidation")
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                // Disable SSL certificate validation
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        })
        .AddPolicyHandler(GetRetryPolicy(1))
        .AddPolicyHandler(GetTimeoutPolicy(1));

    services.AddHttpClient("HttpClient")
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5));
}
    
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int seconds = 5)
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .Or<TimeoutRejectedException>()
        .WaitAndRetryAsync(1, _ => TimeSpan.FromSeconds(seconds));

static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int seconds = 5)
    => Policy.TimeoutAsync<HttpResponseMessage>(seconds,
        TimeoutStrategy.Optimistic, onTimeoutAsync: (_, _, _, _) => Task.CompletedTask);

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