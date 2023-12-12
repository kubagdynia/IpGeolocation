using CommandLine;

namespace IpGeolocation.ConsoleApp;

public abstract class Options
{
    [Option('i', "ip", Required = true, HelpText = "IP Address")]
    public string IpAddress { get; set; }
}