namespace ipgeo.Models;

public class GeoResult
{
    public string Query { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public string Isp { get; set; } = string.Empty;
    public string Org { get; set; } = string.Empty;
    public string As { get; set; } = string.Empty;
    public bool Proxy { get; set; }
    public bool Hosting { get; set; }
}