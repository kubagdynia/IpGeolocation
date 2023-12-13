using CacheDrive.Extensions;
using IpGeolocation.Configuration;
using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace IpGeolocation.Extensions;

public static class ServiceCollectionExtensions
{
    public static void UseIpGeolocation(this IServiceCollection services, IpGeolocationSettings settings = null)
    {
        UseIpGeolocation(services, null, null, settings);
    }
    
    public static void UseIpGeolocation(this IServiceCollection services,
        IConfiguration configuration = null,
        string configurationSectionName = null,
        IpGeolocationSettings settings = null)
    {
        services.AddHttpClient<IpApiService>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5));
        services.AddTransient<IIpGeolocationService, IpGeolocationService>();
        
        services.RegisterCacheDrive(configuration, configurationSectionName: "CacheSettings");

        if (configuration is not null)
        {
            if (string.IsNullOrEmpty(configurationSectionName))
            {
                configurationSectionName = "IpGeolocationSettings";
            }
            
            if (settings is not null)
            {
                services.Configure<IpGeolocationSettings>(opt =>
                {
                    opt.BaseAddress = settings.BaseAddress;
                });
            }
            else
            {
                services.Configure<IpGeolocationSettings>(configuration.GetSection(configurationSectionName));
            }
        }
        else
        {
            if (settings is not null)
            {
                services.Configure<IpGeolocationSettings>(opt =>
                {
                    opt.BaseAddress = settings.BaseAddress;
                });
            }
            else
            {
                // Register default settings
                services.Configure<IpGeolocationSettings>(_ => { });
            }
        }
    }
    
    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int seconds = 5)
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(1, _ => TimeSpan.FromSeconds(seconds));

    static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int seconds = 5)
        => Policy.TimeoutAsync<HttpResponseMessage>(seconds,
            TimeoutStrategy.Optimistic, onTimeoutAsync: (_, _, _, _) => Task.CompletedTask);
}