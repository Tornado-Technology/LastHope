using System.Text.RegularExpressions;

namespace Content.Shared.Utilities;

public static class Address
{
    private static readonly ushort DefaultPort = 1212;
    private static readonly Regex IPv6Regex = new(@"\[(.*:.*:.*)](?::(\d+))?");
    
    public static void Parse(string address, out string ip, out ushort port, ushort? defaultPort = null)
    {
        defaultPort ??= DefaultPort;
        
        var match6 = IPv6Regex.Match(address);
        if (match6 != Match.Empty)
        {
            ip = match6.Groups[1].Value;
            if (!match6.Groups[2].Success)
            {
                port = defaultPort.Value;
                return;
            }

            if (!ushort.TryParse(match6.Groups[2].Value, out port))
                throw new ArgumentException("Not a valid port.");

            return;
        }

        // See if the IP includes a port.
        var split = address.Split(':');
        ip = address;
        port = defaultPort.Value;
        
        if (split.Length > 2)
            throw new ArgumentException("Not a valid Address.");

        // IP:port format.
        if (split.Length != 2)
            return;
        
        ip = split[0];
        if (ushort.TryParse(split[1], out port))
            return;
        
        throw new ArgumentException("Not a valid port.");
    }
}