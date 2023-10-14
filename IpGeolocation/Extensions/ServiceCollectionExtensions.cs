using IpGeolocation.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace IpGeolocation.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterIpGeolocation(this IServiceCollection services)
    {
        services.AddHttpClient<IpApiService>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5));
        services.AddTransient<IIpGeolocationService, IpGeolocationService>();
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