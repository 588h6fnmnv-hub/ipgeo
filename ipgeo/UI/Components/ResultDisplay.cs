namespace ipgeo.UI.Components;

using ipgeo.Models;
using ipgeo.UI;

public static class ResultDisplay
{
    public static string Render(GeoResult result, bool showMapLink = true, int width = 70)
    {
        if (result.Status.Equals("fail", StringComparison.OrdinalIgnoreCase))
        {
            return Panel.RenderError($"Lookup failed for {result.Query}: {result.Message}", 70);
        }

        var lines = new List<string>();
        
        // Main info section
        lines.AddRange(new[]
        {
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}IP Address{Theme.Colors.Reset}  {Theme.Colors.White}{result.Query}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Status{Theme.Colors.Reset}      {Theme.Prefix.Success}{result.Status}{Theme.Colors.Reset}",
            $"",
            $"{Theme.Colors.Primary}{Theme.Colors.Bold}▸ LOCATION{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}{new string('─', 10)}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Country{Theme.Colors.Reset}     {Theme.Colors.White}{result.Country} ({result.CountryCode}){Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Region{Theme.Colors.Reset}      {Theme.Colors.White}{result.RegionName} ({result.Region}){Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}City{Theme.Colors.Reset}        {Theme.Colors.White}{result.City}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Coordinates{Theme.Colors.Reset}  {Theme.Colors.Cyan}{result.Lat:F4}° N, {result.Lon:F4}° W{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Timezone{Theme.Colors.Reset}    {Theme.Colors.White}{result.Timezone}{Theme.Colors.Reset}",
            $"",
            $"{Theme.Colors.Primary}{Theme.Colors.Bold}▸ NETWORK{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}{new string('─', 9)}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}ISP{Theme.Colors.Reset}           {Theme.Colors.White}{result.Isp}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Organization{Theme.Colors.Reset} {Theme.Colors.White}{result.Org}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}ASN{Theme.Colors.Reset}          {Theme.Colors.White}{result.As}{Theme.Colors.Reset}",
            $"",
            $"{Theme.Colors.Primary}{Theme.Colors.Bold}▸ SECURITY{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}{new string('─', 9)}{Theme.Colors.Reset}",
        });

        // Security flags with colored indicators
        var proxyStatus = result.Proxy ? $"{Theme.Colors.Red}{Theme.Icons.Cross} Yes{Theme.Colors.Reset}" : $"{Theme.Colors.Primary}{Theme.Icons.Check}{Theme.Colors.Reset} No";
        var hostingStatus = result.Hosting ? $"{Theme.Colors.Amber}{Theme.Icons.Warning} Yes{Theme.Colors.Reset}" : $"{Theme.Colors.Primary}{Theme.Icons.Check}{Theme.Colors.Reset} No";
        
        lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Proxy/VPN{Theme.Colors.Reset}  {proxyStatus}");
        lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Hosting{Theme.Colors.Reset}  {hostingStatus}");
        
        // Map link
        if (showMapLink && result.Lat != 0 && result.Lon != 0)
        {
            lines.Add("");
            var mapUrl = $"https://maps.google.com/?q={result.Lat:F4},{result.Lon:F4}";
            lines.Add($"{Theme.Colors.Cyan}{Theme.Icons.Location} Map: {Theme.Colors.CyanDim}{Theme.Icons.Globe}{Theme.Colors.Reset} {Theme.Colors.CyanDim}https://maps.google.com/?q={result.Lat:F4},{result.Lon:F4}{Theme.Colors.Reset}");
        }

        return Panel.Render("IP GEOLOCATION RESULT", string.Join("\n", lines), width: 70);
    }

    public static string RenderCompact(GeoResult result, int width = 70)
    {
        if (result.Status.Equals("fail", StringComparison.OrdinalIgnoreCase))
        {
            return $"{Theme.Prefix.Error} {Theme.Colors.Red}Lookup failed: {result.Message}{Theme.Colors.Reset}";
        }

        var lines = new[]
        {
            $"{Theme.Prefix.Success} {Theme.Colors.White}{result.Query}{Theme.Colors.Reset} → {Theme.Colors.Cyan}{result.City}, {result.RegionName}, {result.Country}{Theme.Colors.Reset}",
            $"{Theme.Colors.GrayDim}  ISP: {result.Isp} | ASN: {result.As} | TZ: {result.Timezone}{Theme.Colors.Reset}",
            $"{Theme.Colors.GrayDim}  Coords: {result.Lat:F4}, {result.Lon:F4} | Proxy: {(result.Proxy ? "Yes" : "No")} | Hosting: {(result.Hosting ? "Yes" : "No")}{Theme.Colors.Reset}"
        };

        return string.Join("\n", lines);
    }

    public static string RenderPrivateIpWarning(string inputIp, string publicIp)
    {
        return Panel.RenderWarning(
            "PRIVATE IP DETECTED",
            $"Input IP: {inputIp}\n" +
            $"Type: RFC 1918 Private Address\n" +
            $"Action: Auto-detected public IP → {publicIp}\n" +
            $"Note: Private IPs have no geographic location",
            70
        );
    }

    public static string RenderPrivateIpInfo(string inputIp)
    {
        return Panel.RenderInfo(
            "PRIVATE IP ADDRESS",
            $"Input: {inputIp}\n" +
            $"Type: RFC 1918 Private Address (No public geolocation)\n" +
            $"Note: Use --me to detect your public IP",
            70
        );
    }

    public static string RenderPrivateIpFallback(string inputIp, string publicIp)
    {
        var lines = new List<string>
        {
            $"{Theme.Prefix.Warning} Private IP detected: {inputIp}",
            $"{Theme.Prefix.Info} Auto-detected public IP: {publicIp}",
            $"",
            $"{Theme.Colors.GrayDim}Private IPs have no geographic location. Showing location for your public IP.{Theme.Colors.Reset}"
        };
        return Panel.RenderWarning("PRIVATE IP FALLBACK", string.Join("\n", lines), 70);
    }
}