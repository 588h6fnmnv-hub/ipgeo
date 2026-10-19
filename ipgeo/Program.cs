using ipgeo.Models;
using ipgeo.Services;
using ipgeo.UI;
using ipgeo.UI.Components;
using ipgeo.UI.Components; // For Panel, ResultDisplay, Banner, Spinner
using System.Net;
using System.Collections.Generic;

var geoClient = new GeoClient();

// Parse args
string? ipArg = null;
bool autoDetect = false;
bool showHelp = false;
bool interactive = false;

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--me":
        case "-m":
            autoDetect = true;
            break;
        case "--help":
        case "-h":
            showHelp = true;
            break;
        case "--menu":
        case "-i":
            interactive = true;
            break;
        default:
            if (!args[i].StartsWith("-"))
                ipArg = args[i];
            break;
    }
}

if (showHelp)
{
    ShowHelp();
    return;
}

// Auto-detect public IP
if (autoDetect)
{
    using var spinner = new Spinner("Detecting your public IP...");
    var myIp = await GetPublicIpAsync();
    if (myIp != null)
    {
        spinner.Stop($"Your IP: {myIp}");
        Console.WriteLine();
        await PerformLookup(myIp);
        return;
    }
    Console.WriteLine($"{Theme.Prefix.Error} Failed to detect public IP.");
    return;
}

// Single-shot mode: ipgeo 8.8.8.8
if (ipArg != null)
{
    if (IPAddress.TryParse(ipArg, out var ipAddr))
    {
        // Check if private IP and auto-fallback to public IP
        if (IsPrivateIp(ipAddr))
        {
            Console.WriteLine($"{Theme.Prefix.Warning} Private IP detected ({ipArg}) - looking up your public IP...");
            var publicIp = await GetPublicIpAsync();
            if (publicIp != null)
            {
                Console.WriteLine($"Your public IP: {publicIp}");
                Console.WriteLine();
                await PerformLookup(publicIp);
                return;
            }
            Console.WriteLine($"{Theme.Prefix.Error} Failed to detect public IP.");
            return;
        }
        await PerformLookup(ipArg);
        return;
    }
    Console.WriteLine("Invalid IP address format.");
    return;
}

// Interactive mode (default)
await RunInteractiveMode();

async Task PerformLookup(string ip)
{
    using var spinner = new Spinner($"Looking up {ip}...");
    var result = await geoClient.LookupAsync(ip);
    spinner.Stop($"Lookup complete: {ip}");
    
    Console.WriteLine();
    Console.WriteLine(ResultDisplay.Render(result));
    Console.WriteLine();
}

async Task RunInteractiveMode()
{
    Console.Clear();
    Console.WriteLine(Banner.RenderMain("INTERACTIVE MODE"));
    Console.WriteLine($"{Theme.Colors.GrayDim}  Type an IP address to look up, or :help for commands{Theme.Colors.Reset}");
    Console.WriteLine();

    var geoClientLocal = new GeoClient();
    var history = new List<string>();
    int queryCount = 0;
    bool running = true;

    while (true)
    {
        try
        {
            var input = await ReadLineAsync();
            
            if (string.IsNullOrWhiteSpace(input))
                continue;

            // Handle commands
            if (input.StartsWith(":"))
            {
                await HandleCommand(input.Trim());
                continue;
            }

            if (input.Equals("q", StringComparison.OrdinalIgnoreCase) || 
                input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (!IPAddress.TryParse(input, out var ipAddr))
            {
                RenderError($"Invalid IP address format: {input}");
                continue;
            }

            // Check if private IP and auto-fallback to public IP
            if (IsPrivateIp(ipAddr))
            {
                Console.WriteLine($"{Theme.Prefix.Warning} Private IP detected ({input}) - looking up your public IP...");
                var publicIp = await GetPublicIpAsync();
                if (publicIp != null)
                {
                    Console.WriteLine($"Your public IP: {publicIp}");
                    Console.WriteLine();
                    await PerformLookup(publicIp);
                    Console.WriteLine();
                    continue;
                }
                Console.WriteLine("Failed to detect public IP.");
                Console.WriteLine();
                continue;
            }
            
            await PerformLookup(input);
        }
        catch (OperationCanceledException)
        {
            break;
        }
        catch (Exception ex)
        {
            RenderError($"Error: {ex.Message}");
        }
    }

    Console.WriteLine();
    Console.WriteLine($"{Theme.Colors.PrimaryDim}{new string('═', 70)}{Theme.Colors.Reset}");
    Console.WriteLine($"{Theme.Colors.Primary}{Theme.Icons.Ghost}  Session ended.  {Theme.Icons.Ghost}{Theme.Colors.Reset}");
    Console.WriteLine($"{Theme.Colors.GrayDim}  Stay anonymous. Stay secure.{Theme.Colors.Reset}");
    Console.WriteLine();
}

async Task<string> ReadLineAsync()
{
    Console.Write($"{Theme.Colors.Primary}{Theme.Colors.Bold}ipgeo{Theme.Colors.Reset}{Theme.Colors.PrimaryDim} › {Theme.Colors.Reset}");
    return Console.ReadLine()?.Trim() ?? "";
}

async Task HandleCommand(string command)
{
    var parts = command.Split(' ', 2);
    var cmd = parts[0].ToLowerInvariant();
    var arg = parts.Length > 1 ? parts[1] : "";

    switch (cmd)
    {
        case ":help":
        case ":h":
            ShowHelp();
            break;
        case ":history":
        case ":hist":
            ShowHistory();
            break;
        case ":clear":
        case ":cls":
            Console.Clear();
            Console.WriteLine(Banner.RenderMain("INTERACTIVE MODE"));
            Console.WriteLine($"{Theme.Colors.GrayDim}  Type an IP address to look up, or :help for commands{Theme.Colors.Reset}");
            Console.WriteLine();
            break;
        case ":quit":
        case ":q":
        case ":exit":
            Environment.Exit(0);
            break;
        case ":me":
            await HandleMyIp();
            break;
        case ":geo":
            if (!string.IsNullOrEmpty(arg))
            {
                await PerformLookup(arg);
            }
            else
            {
                RenderError("Usage: :geo <IP>");
            }
            break;
        case ":stats":
            ShowStats();
            break;
        default:
            if (cmd.StartsWith(":"))
            {
                RenderError($"Unknown command: {cmd}. Type :help for available commands.");
            }
            break;
    }
}

async Task HandleMyIp()
{
    using var spinner = new Spinner("Detecting your public IP...");
    var myIp = await GetPublicIpAsync();
    
    if (myIp != null)
    {
        spinner.Stop($"Your IP: {myIp}");
        Console.WriteLine();
        await PerformLookup(myIp);
    }
    else
    {
        spinner.Stop("Failed to detect public IP");
    }
}

void ShowHistory()
{
    Console.WriteLine($"{Theme.Prefix.Info} History tracking not yet implemented in this version");
}

void ShowStats()
{
    var stats = $@"Session Active: Yes
Mode: Interactive
Theme: Dark/Hacker";
    
    Console.WriteLine(Panel.Render("SESSION STATS", stats.Trim(), width: 70));
}

async Task<string?> GetPublicIpAsync()
{
    using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
    try
    {
        var services = new[] 
        { 
            "https://api.ipify.org",
            "https://ifconfig.me/ip",
            "https://icanhazip.com"
        };
        
        foreach (var service in services)
        {
            try
            {
                var ip = await http.GetStringAsync(service);
                return ip.Trim();
            }
            catch { }
        }
    }
    catch { }
    return null;
}

bool IsPrivateIp(IPAddress ip)
{
    var bytes = ip.GetAddressBytes();
    if (bytes.Length != 4) return false;
    if (bytes[0] == 10) return true;
    if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return true;
    if (bytes[0] == 192 && bytes[1] == 168) return true;
    if (bytes[0] == 127) return true;
    if (bytes[0] == 169 && bytes[1] == 254) return true;
    return false;
}

void RenderError(string message)
{
    Console.WriteLine(Panel.RenderError(message, 70));
}

void RenderInfo(string title, string message, int width)
{
    Console.WriteLine(Panel.RenderInfo(title, message, width));
}

void ShowHelp()
{
    Console.WriteLine("ipgeo - Terminal IP Geolocator");
    Console.WriteLine();
    Console.WriteLine("USAGE:");
    Console.WriteLine("  ipgeo [IP_ADDRESS]        Look up a specific IP address");
    Console.WriteLine("  ipgeo --me                Detect and look up your public IP");
    Console.WriteLine("  ipgeo                     Start interactive mode");
    Console.WriteLine("  ipgeo --help              Show this help");
    Console.WriteLine();
    Console.WriteLine("EXAMPLES:");
    Console.WriteLine("  ipgeo 8.8.8.8");
    Console.WriteLine("  ipgeo 2001:4860:4860::8888");
    Console.WriteLine("  ipgeo --me");
    Console.WriteLine();
    Console.WriteLine("DATA SOURCE: ip-api.com (free, no API key required)");
}