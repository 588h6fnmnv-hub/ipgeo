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
            $"{Theme.Colors.PrimaryDim}{new string(Theme.Box.H, 10)}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Country{Theme.Colors.Reset}     {Theme.Colors.White}{result.Country} ({result.CountryCode}){Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Region{Theme.Colors.Reset}      {Theme.Colors.White}{result.RegionName} ({result.Region}){Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}City{Theme.Colors.Reset}        {Theme.Colors.White}{result.City}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Coordinates{Theme.Colors.Reset}  {Theme.Colors.Cyan}{result.Lat:F4}° N, {result.Lon:F4}° W{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Timezone{Theme.Colors.Reset}    {Theme.Colors.White}{result.Timezone}{Theme.Colors.Reset}",
            $"",
            $"{Theme.Colors.Primary}{Theme.Colors.Bold}▸ NETWORK{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}{new string(Theme.Box.H, 9)}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}ISP{Theme.Colors.Reset}           {Theme.Colors.White}{result.Isp}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Organization{Theme.Colors.Reset} {Theme.Colors.White}{result.Org}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}ASN{Theme.Colors.Reset}          {Theme.Colors.White}{result.As}{Theme.Colors.Reset}",
            $"",
            $"{Theme.Colors.Primary}{Theme.Colors.Bold}▸ SECURITY{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}{new string(Theme.Box.H, 10)}{Theme.Colors.Reset}",
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
            lines.Add($"{Theme.Colors.Cyan}{Theme.Icons.Location} Map: {Theme.Colors.CyanDim}{mapUrl}{Theme.Colors.Reset}");
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

    public static string RenderDnsResult(DnsInfo result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            return Panel.RenderError($"DNS Lookup Failed: {result.Error}", 70);
        }

        if (!result.HasData)
        {
            return Panel.RenderInfo("DNS LOOKUP", "No DNS records found for this IP", 70);
        }

        var lines = new List<string>();
        
        if (!string.IsNullOrEmpty(result.PtrRecord))
        {
            lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}PTR Record{Theme.Colors.Reset}  {Theme.Colors.Cyan}{result.PtrRecord}{Theme.Colors.Reset}");
        }
        
        if (result.ARecords?.Length > 0)
        {
            lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}A Records{Theme.Colors.Reset}   {Theme.Colors.Cyan}{string.Join(", ", result.ARecords)}{Theme.Colors.Reset}");
        }
        
        if (result.AaaaRecords?.Length > 0)
        {
            lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}AAAA Records{Theme.Colors.Reset} {Theme.Colors.Cyan}{string.Join(", ", result.AaaaRecords)}{Theme.Colors.Reset}");
        }

        return Panel.Render("DNS LOOKUP RESULT", string.Join("\n", lines), width: 70);
    }

    public static string RenderWhoisResult(WhoisInfo result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            return Panel.RenderError($"WHOIS Lookup Failed: {result.Error}", 70);
        }

        if (!result.HasData)
        {
            return Panel.RenderInfo("WHOIS LOOKUP", "No WHOIS data available for this IP", 70);
        }

        var lines = new List<string>();

        if (result.IsPrivate)
        {
            lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Type{Theme.Colors.Reset}      {Theme.Colors.Amber}Private IP Range{Theme.Colors.Reset}");
            lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Range{Theme.Colors.Reset}      {Theme.Colors.Amber}{result.PrivateRangeInfo}{Theme.Colors.Reset}");
            lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Registry{Theme.Colors.Reset}   {Theme.Colors.Gray}IANA (Reserved){Theme.Colors.Reset}");
        }
        else
        {
            if (!string.IsNullOrEmpty(result.NetworkName))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Network{Theme.Colors.Reset}    {Theme.Colors.White}{result.NetworkName}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.NetworkHandle))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Handle{Theme.Colors.Reset}     {Theme.Colors.White}{result.NetworkHandle}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.NetworkRange))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Range{Theme.Colors.Reset}      {Theme.Colors.White}{result.NetworkRange}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.OrgName))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Organization{Theme.Colors.Reset} {Theme.Colors.White}{result.OrgName}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.Country))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Country{Theme.Colors.Reset}    {Theme.Colors.White}{result.Country}{Theme.Colors.Reset}");
            if (result.RegistrationDate.HasValue)
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Registered{Theme.Colors.Reset}  {Theme.Colors.White}{result.RegistrationDate.Value:yyyy-MM-dd}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.AbuseEmail))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Abuse Contact{Theme.Colors.Reset} {Theme.Colors.Red}{result.AbuseEmail}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.AbusePhone))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Abuse Phone{Theme.Colors.Reset} {Theme.Colors.Red}{result.AbusePhone}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.TechEmail))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Tech Contact{Theme.Colors.Reset} {Theme.Colors.Cyan}{result.TechEmail}{Theme.Colors.Reset}");
            if (!string.IsNullOrEmpty(result.Source))
                lines.Add($"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Source{Theme.Colors.Reset}      {Theme.Colors.Gray}{result.Source}{Theme.Colors.Reset}");
        }

        return Panel.Render("WHOIS / RDAP RESULT", string.Join("\n", lines), width: 70);
    }

    public static string RenderPhoneInfo(PhoneInfo info)
    {
        var lines = new List<string>
        {
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Input{Theme.Colors.Reset}          {Theme.Colors.White}{info.InputNumber}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Parsed{Theme.Colors.Reset}         {Theme.Colors.White}{info.ParsedNumber}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}International{Theme.Colors.Reset} {Theme.Colors.White}{info.InternationalFormat}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}National{Theme.Colors.Reset}        {Theme.Colors.White}{info.NationalFormat}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}E.164{Theme.Colors.Reset}           {Theme.Colors.White}{info.E164Format}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Country{Theme.Colors.Reset}        {Theme.Colors.White}{info.CountryName} ({info.RegionCode}){Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Country Code{Theme.Colors.Reset}  {Theme.Colors.White}+{info.CountryCode}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}National Number{Theme.Colors.Reset} {Theme.Colors.White}{info.NationalNumber}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Carrier{Theme.Colors.Reset}        {Theme.Colors.Cyan}{info.Carrier}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Location{Theme.Colors.Reset}       {Theme.Colors.White}{info.Location}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Timezone(s){Theme.Colors.Reset}    {Theme.Colors.Cyan}{info.TimeZones}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Type{Theme.Colors.Reset}           {Theme.Colors.White}{info.NumberType}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Valid{Theme.Colors.Reset}          {(info.IsValid ? $"{Theme.Colors.Primary}{Theme.Icons.Check} Yes" : $"{Theme.Colors.Red}{Theme.Icons.Cross} No")}{Theme.Colors.Reset}",
            $"{Theme.Prefix.Bullet} {Theme.Colors.PrimaryDim}Possible{Theme.Colors.Reset}      {(info.IsPossible ? $"{Theme.Colors.Primary}{Theme.Icons.Check} Yes" : $"{Theme.Colors.Red}{Theme.Icons.Cross} No")}{Theme.Colors.Reset}",
        };

        return Panel.Render("PHONE NUMBER LOOKUP", string.Join("\n", lines), width: 70);
    }

    public static string RenderUsernameResult(UsernameResult result)
    {
        var lines = new List<string>();
        
        foreach (var kvp in result.Results.OrderBy(x => x.Key))
        {
            var status = kvp.Value;
            var isFound = !status.StartsWith("NOT FOUND", StringComparison.OrdinalIgnoreCase);
            var icon = isFound ? $"{Theme.Colors.Primary}{Theme.Icons.Check}" : $"{Theme.Colors.RedDim}{Theme.Icons.Cross}";
            var color = isFound ? Theme.Colors.Primary : Theme.Colors.RedDim;
            
            lines.Add($"  {icon}{Theme.Colors.Reset} {Theme.Colors.White}{kvp.Key.PadRight(15)}{Theme.Colors.Reset} {color}{status}{Theme.Colors.Reset}");
        }
        
        lines.Insert(0, "");
        lines.Add("");
        lines.Add($"{Theme.Colors.GrayDim}Found: {Theme.Colors.Primary}{result.FoundCount}{Theme.Colors.GrayDim} / {result.TotalCount}{Theme.Colors.Reset}");
        
        return Panel.Render("USERNAME TRACKER RESULT", string.Join("\n", lines), width: 70);
    }
}