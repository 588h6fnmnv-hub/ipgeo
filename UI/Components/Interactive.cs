namespace ipgeo.UI.Components;

using ipgeo.Models;
using ipgeo.UI;
using System.Collections.Generic;

public class InteractiveSession
{
    private readonly GeoClient _geoClient;
    private readonly List<string> _history = new();
    private readonly int _maxHistory = 50;
    private int _historyIndex = -1;
    private bool _running = true;
    private int _queryCount = 0;

    public InteractiveSession(GeoClient geoClient)
    {
        _geoClient = geoClient;
    }

    public async Task RunAsync()
    {
        ShowWelcome();
        
        while (_running)
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

                if (!System.Net.IPAddress.TryParse(input, out var ipAddr))
                {
                    RenderError($"Invalid IP address format: {input}");
                    continue;
                }

                // Check if private IP
                if (IsPrivateIp(System.Net.IPAddress.Parse(input)))
                {
                    await HandlePrivateIp(input);
                    continue;
                }

                await PerformLookup(input);
                _queryCount++;
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

        ShowGoodbye();
    }

    private void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine(Banner.RenderMain("INTERACTIVE MODE"));
        Console.WriteLine($"{Theme.Colors.GrayDim}  Type an IP address to look up, or :help for commands{Theme.Colors.Reset}");
        Console.WriteLine();
    }

    private void ShowGoodbye()
    {
        Console.WriteLine();
        Console.WriteLine($"{Theme.Colors.PrimaryDim}{new string('─', 70)}{Theme.Colors.Reset}");
        Console.WriteLine($"{Theme.Colors.Primary}{Theme.Icons.Ghost}  Session ended. Queries: {_queryCount}  {Theme.Icons.Ghost}{Theme.Colors.Reset}");
        Console.WriteLine($"{Theme.Colors.GrayDim}  Stay anonymous. Stay secure.{Theme.Colors.Reset}");
        Console.WriteLine();
    }

    private async Task<string> ReadLineAsync()
    {
        Console.Write($"{Theme.Colors.Primary}{Theme.Colors.Bold}ipgeo{Theme.Colors.Reset}{Theme.Colors.PrimaryDim} › {Theme.Colors.Reset}");
        
        var input = "";
        var cursorPos = 0;
        var buffer = new List<char>();
        
        while (true)
        {
            var key = Console.ReadKey(true);
            
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return new string(buffer.ToArray()).Trim();
            }
            else if (key.Key == ConsoleKey.Backspace && buffer.Count > 0)
            {
                buffer.RemoveAt(buffer.Count - 1);
                Console.Write("\b \b");
            }
            else if (key.Key == ConsoleKey.LeftArrow && cursorPos > 0)
            {
                cursorPos--;
                Console.Write("\b");
            }
            else if (key.Key == ConsoleKey.RightArrow && cursorPos < buffer.Count)
            {
                Console.Write(buffer[cursorPos]);
                cursorPos++;
            }
            else if (key.Key == ConsoleKey.UpArrow && _history.Count > 0)
            {
                _historyIndex = Math.Max(0, _historyIndex - 1);
                if (_historyIndex < _history.Count)
                {
                    buffer.Clear();
                    buffer.AddRange(_history[_historyIndex].ToCharArray());
                    cursorPos = buffer.Count;
                    Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                    Console.Write($"{Theme.Colors.Primary}{Theme.Colors.Bold}ipgeo{Theme.Colors.Reset}{Theme.Colors.PrimaryDim} › {Theme.Colors.Reset}{new string(buffer.ToArray())}");
                }
            }
            else if (key.Key == ConsoleKey.DownArrow && _history.Count > 0)
            {
                _historyIndex = Math.Min(_history.Count - 1, _historyIndex + 1);
                if (_historyIndex < _history.Count)
                {
                    buffer.Clear();
                    buffer.AddRange(_history[_historyIndex].ToCharArray());
                    cursorPos = buffer.Count;
                    Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                    Console.Write($"{Theme.Colors.Primary}{Theme.Colors.Bold}ipgeo{Theme.Colors.Reset}{Theme.Colors.PrimaryDim} › {Theme.Colors.Reset}{new string(buffer.ToArray())}");
                }
                else
                {
                    buffer.Clear();
                    cursorPos = 0;
                    Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                    Console.Write($"{Theme.Colors.Primary}{Theme.Colors.Bold}ipgeo{Theme.Colors.Reset}{Theme.Colors.PrimaryDim} › {Theme.Colors.Reset}");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                buffer.Insert(cursorPos, key.KeyChar);
                cursorPos++;
                Console.Write(key.KeyChar);
            }
        }
    }

    private async Task HandleCommand(string command)
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
                ShowWelcome();
                break;
            case ":quit":
            case ":q":
            case ":exit":
                _running = false;
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
            case ":whois":
                // Would need WhoisService
                RenderInfo("WHOIS", "WHOIS lookup not available in simple mode", 70);
                break;
            case ":dns":
                // Would need DnsLookupService
                RenderInfo("DNS", "DNS lookup not available in simple mode", 70);
                break;
            case ":theme":
                RenderInfo("THEMES", "Theme switching not yet implemented", 70);
                break;
            case ":stats":
                ShowStats();
                break;
            case ":help":
            default:
                if (cmd.StartsWith(":"))
                {
                    RenderError($"Unknown command: {cmd}. Type :help for available commands.");
                }
                break;
        }
    }

    private void ShowHelp()
    {
        var help = @"
COMMANDS:
  :help, :h       Show this help
  :history, :hist Show query history
  :clear, :cls    Clear screen
  :me             Detect and show your public IP
  :geo <IP>       Look up an IP address
  :stats          Show session statistics
  :clear          Clear screen
  :quit, :q, :exit Exit program

SHORTCUTS:
  ↑/↓             Navigate history
  ←/→             Move cursor
  Ctrl+C          Exit

EXAMPLES:
  8.8.8.8
  2001:4860:4860::8888
  :me
  :geo 1.1.1.1
";
        Console.WriteLine(Panel.Render("INTERACTIVE MODE HELP", help.Trim(), width: 70));
    }

    private void ShowHistory()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine($"{Theme.Prefix.Info} No history yet");
            return;
        }

        var lines = new List<string>();
        for (int i = 0; i < _history.Count; i++)
        {
            var prefix = i == _history.Count - 1 ? $"{Theme.Colors.Primary}►{Theme.Colors.Reset}" : "  ";
            lines.Add($"  {i + 1,3}. {_history[i]}");
        }
        
        Console.WriteLine(Panel.Render("QUERY HISTORY", string.Join("\n", lines), width: 70));
    }

    private void ShowStats()
    {
        var stats = $@"Total Queries: {_queryCount}
History Size: {_history.Count}
Session Started: {DateTime.Now:HH:mm:ss}
Mode: Interactive";
        
        Console.WriteLine(Panel.Render("SESSION STATS", stats.Trim(), width: 70));
    }

    private async Task HandleMyIp()
    {
        using var spinner = new Spinner("Detecting your public IP...");
        var myIp = await GetPublicIpAsync();
        
        if (myIp != null)
        {
            spinner.Stop($"Detected: {myIp}");
            Console.WriteLine();
            await PerformLookup(myIp);
        }
        else
        {
            spinner.Stop("Failed to detect public IP");
        }
    }

    private async Task PerformLookup(string ip)
    {
        using var spinner = new Spinner($"Looking up {ip}...");
        var result = await _geoClient.LookupAsync(ip);
        spinner.Stop($"Lookup complete: {result.Query}");
        
        Console.WriteLine();
        Console.WriteLine(ResultDisplay.Render(result));
        Console.WriteLine();
    }

    private async Task HandlePrivateIp(string input)
    {
        Console.WriteLine($"{Theme.Prefix.Warning} Private IP detected: {Theme.Colors.White}{Theme.Icons.Warning}{Theme.Colors.Reset}");
        
        using var spinner = new Spinner("Detecting your public IP...");
        var publicIp = await GetPublicIpAsync();
        
        if (publicIp != null)
        {
            spinner.Stop($"Your public IP: {publicIp}");
            Console.WriteLine();
            await PerformLookup(publicIp);
        }
        else
        {
            spinner.Stop("Failed to detect public IP");
        }
    }

    private void AddToHistory(string ip)
    {
        if (!_history.Contains(ip))
        {
            _history.Add(ip);
            if (_history.Count > _maxHistory)
                _history.RemoveAt(0);
        }
        _historyIndex = _history.Count;
    }

    private bool IsPrivateIp(System.Net.IPAddress ip)
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

    private async Task<string?> GetPublicIpAsync()
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

    private void RenderError(string message)
    {
        Console.WriteLine(Panel.RenderError(message, 70));
    }

    private void RenderInfo(string title, string message, int width)
    {
        Console.WriteLine(Panel.RenderInfo(title, message, 70));
    }
}