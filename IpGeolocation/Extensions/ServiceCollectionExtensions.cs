using CacheDrive.Configuration;
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
    public static IServiceCollection UseIpGeolocation(this IServiceCollection services, IpGeolocationSettings settings = null)
    {
        return UseIpGeolocation(services, null, null, settings);
    }
    
    public static IServiceCollection UseIpGeolocation(this IServiceCollection services, CacheType cacheType)
    {
        return UseIpGeolocation(services, settings: new IpGeolocationSettings { CacheType = cacheType });
    }
    
    public static IServiceCollection UseIpGeolocation(this IServiceCollection services,
        IConfiguration configuration = null,
        string configurationSectionName = null,
        IpGeolocationSettings settings = null)
    {
        services.AddHttpClient<IpApiService>()
            .AddPolicyHandler(GetRetryPolicy(retryCount: 1, seconds: 5))
            .AddPolicyHandler(GetTimeoutPolicy(timeoutSeconds: 5));
        
        return RegisterAllItems(services, configuration, configurationSectionName, settings);
    }

    public static IServiceCollection UseIpGeolocation(this IServiceCollection services,
        HttpMessageHandler httpMessageHandler,
        IConfiguration configuration = null,
        string configurationSectionName = null,
        IpGeolocationSettings settings = null)
    {
        services.AddHttpClient<IpApiService>()
            .AddPolicyHandler(GetRetryPolicy(retryCount: 1, seconds: 5))
            .AddPolicyHandler(GetTimeoutPolicy(timeoutSeconds: 5))
            .ConfigurePrimaryHttpMessageHandler(() => httpMessageHandler);

        return RegisterAllItems(services, configuration, configurationSectionName, settings);
    }

    private static IServiceCollection RegisterAllItems(
        IServiceCollection services,
        IConfiguration configuration = null,
        string configurationSectionName = null,
        IpGeolocationSettings settings = null)
    {
        services.AddTransient<IIpGeolocationService, IpGeolocationService>();

        if (string.IsNullOrEmpty(configurationSectionName))
        {
            configurationSectionName = IpGeolocationSettings.AppConfigSectionName;
        }

        services.RegisterCacheDrive(configuration, configurationSectionName: configurationSectionName, settings);
        
        if (configuration is not null)
        {
            if (settings is not null)
            {
                services.Configure<IpGeolocationSettings>(opt =>
                {
                    if (!string.IsNullOrEmpty(settings.BaseAddress))
                    {
                        opt.BaseAddress = settings.BaseAddress;
                    }

                    if (!string.IsNullOrEmpty(settings.UserAgent))
                    {

                        opt.UserAgent = settings.UserAgent;
                    }
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
                    if (!string.IsNullOrEmpty(settings.BaseAddress))
                    {
                        opt.BaseAddress = settings.BaseAddress;
                    }

                    if (!string.IsNullOrEmpty(settings.UserAgent))
                    {

                        opt.UserAgent = settings.UserAgent;
                    }
                });
            }
            else
            {
                // Register default settings
                services.Configure<IpGeolocationSettings>(_ => { });
            }
        }

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, int seconds)
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(retryCount, _ => TimeSpan.FromSeconds(seconds));

    static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int timeoutSeconds)
        => Policy.TimeoutAsync<HttpResponseMessage>(timeoutSeconds);
}