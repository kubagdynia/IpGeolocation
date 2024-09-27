using System.Web;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace IpGeolocation.ConsoleApp.Extensions;

public class UriQueryParametersMaskingEnricher : ILogEventEnricher
{
    private const string DefaultMaskValue = "******";
    private const string SensitiveKeywordsSection = "Logging:SensitiveData:Keywords";
    private const string MaskValue = "Logging:SensitiveData:Mask";
    
    private readonly string _maskValue;
    private readonly string[] _parametersToMask;
    
    public UriQueryParametersMaskingEnricher(string maskValue, params string[] parametersToMask)
    {
        _maskValue = maskValue ?? DefaultMaskValue;
        _parametersToMask = parametersToMask ?? [];
    }
    
    /// <summary>
    /// appsettings example: "Logging": { "SensitiveData": { "Keywords": [ "key", "password" ], "Mask": "****" } }
    /// </summary>
    /// <param name="configuration">IConfiguration</param>
    public UriQueryParametersMaskingEnricher(IConfiguration configuration)
    {
        _maskValue = configuration.GetValue(MaskValue, DefaultMaskValue);
        _parametersToMask = configuration.GetSection(SensitiveKeywordsSection).Get<string[]>() ?? [];
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (!logEvent.Properties.TryGetValue("Uri", out var uriValue) || uriValue is not ScalarValue uriScalar) return;
        
        var uriString = uriScalar.Value as string;
        
        if (!string.IsNullOrEmpty(uriString) && Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out var uri))
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query); // parse query string
            
            foreach (var parameter in _parametersToMask)
            {
                if (query[parameter] != null)
                {
                    query[parameter] = _maskValue; // replace value with mask
                }
            }
            
            uriBuilder.Query = query.ToString() ?? string.Empty; // update query string
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Uri", uriBuilder.Uri.ToString()));
        }
    }
}