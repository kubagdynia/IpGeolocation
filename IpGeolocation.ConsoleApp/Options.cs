using CommandLine;

namespace IpGeolocation.ConsoleApp;

public class Options
{
    [Option('i', "ip", Required = true, HelpText = "IP Address")]
    public string IpAddress { get; set; }
    
    [Option('f', "full", Required = false, HelpText = "Full")]
    public bool Full { get; set; }
    
    [Option('c', "city", Required = false, HelpText = "City")]
    public bool City { get; set; }
    
    [Option('r', "region", Required = false, HelpText = "Region")]
    public bool Region { get; set; }
    
    [Option('t', "country", Required = false, HelpText = "Country")]
    public bool Country { get; set; }
    
    [Option('n', "countryname", Required = false, HelpText = "Country Name")]
    public bool CountryName { get; set; }
}