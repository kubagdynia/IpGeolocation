using System.Net;
using IpGeolocation.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace IpGeolocation.Tests;

public static class TestHelper
{
    public static ServiceProvider CreateServiceProvider(string config = null, string responseContent = null)
    {
        var services = new ServiceCollection();


        services.AddHttpClient(Options.DefaultName).ConfigurePrimaryHttpMessageHandler( () => CreateFakeHttpMessageHandler(responseContent));

        if (config is null)
        {
            services.UseIpGeolocation(CreateFakeHttpMessageHandler(responseContent));
        }
        else
        {
            using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config));
            var configuration = new ConfigurationBuilder().AddJsonStream(mem).Build();
            
            services.UseIpGeolocation(CreateFakeHttpMessageHandler(responseContent), configuration);
        }
        
        return services.BuildServiceProvider();
    }

    private static HttpMessageHandler CreateFakeHttpMessageHandler(string responseContent)
    {
        var mockHttpResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse);
        
        return mockHttpMessageHandler.Object;
    }
    
    public static HttpClient CreateFakeHttpClient(string responseContent)
    {
        var mockHttpResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse);
        
        return new HttpClient(mockHttpMessageHandler.Object);
    }
}