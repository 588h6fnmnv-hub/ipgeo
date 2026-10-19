namespace ipgeo.Services;

using ipgeo.Models;
using System.Net.Http.Json;
using System.Text.Json;

public class GeoClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://ip-api.com/json/";
    private const string Fields = "status,message,country,countryCode,region,regionName,city,lat,lon,timezone,isp,org,as,proxy,hosting,query";

    public GeoClient()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<GeoResult> LookupAsync(string ip)
    {
        var url = $"{BaseUrl}{ip}?fields={Fields}";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                return new GeoResult
                {
                    Query = ip,
                    Status = "fail",
                    Message = $"HTTP error: {response.StatusCode}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<GeoResult>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new GeoResult { Query = ip, Status = "fail", Message = "Empty response" };
        }
        catch (HttpRequestException ex)
        {
            return new GeoResult
            {
                Query = ip,
                Status = "fail",
                Message = $"Network error: {ex.Message}"
            };
        }
        catch (TaskCanceledException)
        {
            return new GeoResult
            {
                Query = ip,
                Status = "fail",
                Message = "Request timeout"
            };
        }
        catch (JsonException ex)
        {
            return new GeoResult
            {
                Query = ip,
                Status = "fail",
                Message = $"Parse error: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new GeoResult
            {
                Query = ip,
                Status = "fail",
                Message = $"Unexpected error: {ex.Message}"
            };
        }
    }
}