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
    private const string IpGeolocationSettingsName = "IpGeolocationSettings";
    
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
            .AddPolicyHandler(GetTimeoutPolicy());
        services.AddTransient<IIpGeolocationService, IpGeolocationService>();

        if (string.IsNullOrEmpty(configurationSectionName))
        {
            configurationSectionName = IpGeolocationSettingsName;
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
    }
    
    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int seconds = 5)
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(1, _ => TimeSpan.FromSeconds(seconds));

    static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int seconds = 5)
        => Policy.TimeoutAsync<HttpResponseMessage>(seconds);
}