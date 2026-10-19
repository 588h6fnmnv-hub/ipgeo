namespace ipgeo.UI.Components;

using ipgeo.UI;

public static class Banner
{
    public static string RenderMain(string? subtitle = null, int width = 70)
    {
        var art = new[]
        {
            "    ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄",
            "    █░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
            "    █  ██████╗ ██╗   ██╗███████╗███████╗███████╗██████╗ ███████╗  █",
            "    █  ██╔══██╗██║   ██║██╔════╝██╔════╝██╔════╝██╔══██╗██╔════╝  █",
            "    █  ██████╔╝██║   ██║█████╗  ███████╗█████╗  ██████╔╝█████╗    █",
            "    █  ██╔══██╗██║   ██║██╔══╝  ╚════██║██╔══╝  ██╔══██╗██╔══╝    █",
            "    █  ██║  ██║╚██████╔╝███████╗███████║███████╗██║  ██║███████╗  █",
            "    █  ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝╚══════╝  █",
            "    █░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
            "    ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀"
        };

        var lines = new List<string>();
        
        // Top border
        lines.Add($"{Theme.Colors.PrimaryDim}{new string('═', 70)}{Theme.Colors.Reset}");
        
        // ASCII Art
        foreach (var line in art)
        {
            lines.Add($"{Theme.Colors.Primary}{line}{Theme.Colors.Reset}");
        }
        
        // Separator
        lines.Add($"{Theme.Colors.PrimaryDim}{new string('═', 70)}{Theme.Colors.Reset}");
        
        // Subtitle
        if (!string.IsNullOrEmpty(subtitle))
        {
            var centered = subtitle.PadLeft((70 + subtitle.Length) / 2).PadRight(70);
            lines.Add($"{Theme.Colors.Cyan}{Theme.Colors.Dim}{centered}{Theme.Colors.Reset}");
        }
        else
        {
            var tagline = "  IP GEOLOCATION & NETWORK INTELLIGENCE  ";
            var centered = tagline.PadLeft((70 + tagline.Length) / 2).PadRight(70);
            lines.Add($"{Theme.Colors.Cyan}{Theme.Colors.Dim}{centered}{Theme.Colors.Reset}");
        }
        
        // Bottom border
        lines.Add($"{Theme.Colors.PrimaryDim}{new string('═', 70)}{Theme.Colors.Reset}");
        
        // Tagline
        lines.Add($"{Theme.Colors.GrayDim}{Theme.Colors.Dim}  ►  CODE BY HUNXBYTS  ◄  FOR INDIAN USERS  ◄  GHOST MODE ENABLED  {Theme.Colors.Reset}");
        lines.Add("");
        
        return string.Join("\n", lines);
    }

    public static string RenderCompact(string title, string? subtitle = null, int width = 70)
    {
        var lines = new List<string>();
        
        var topBorder = $"{Theme.Colors.PrimaryDim}╔{new string('═', width - 2)}╗{Theme.Colors.Reset}";
        var titleLine = $" {title} ";
        var padding = (width - titleLine.Length) / 2;
        var leftPad = new string(' ', padding);
        var rightPad = new string(' ', width - padding - titleLine.Length);
        
        var lines2 = new List<string>
        {
            $"{Theme.Colors.PrimaryDim}╔{new string('═', width - 2)}╗{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}║{Theme.Colors.Reset}{leftPad}{Theme.Colors.Primary}{Theme.Colors.Bold}{title}{Theme.Colors.Reset}{rightPad}{Theme.Colors.PrimaryDim}║{Theme.Colors.Reset}",
            $"{Theme.Colors.PrimaryDim}╠{new string('═', width - 2)}╣{Theme.Colors.Reset}"
        };
        
        if (!string.IsNullOrEmpty(subtitle))
        {
            var subLine = $" {subtitle} ".PadRight(width - 2);
            var contentLine = $"{Theme.Colors.PrimaryDim}║{Theme.Colors.Reset}{Theme.Colors.GrayDim}{subLine}{Theme.Colors.Reset}{Theme.Colors.PrimaryDim}║{Theme.Colors.Reset}";
            lines2.Insert(2, $"{Theme.Colors.PrimaryDim}║{Theme.Colors.Reset}{Theme.Colors.GrayDim}{new string(' ', width - 2)}{Theme.Colors.PrimaryDim}║{Theme.Colors.Reset}");
        }
        
        return string.Join("\n", lines2);
    }

    public static string RenderSection(string title, int width = 70)
    {
        var line = $"{Theme.Colors.Primary}{Theme.Icons.ArrowSmall} {title}{Theme.Colors.Reset}";
        var underline = $"{Theme.Colors.PrimaryDim}{new string('─', title.Length + 2)}{Theme.Colors.Reset}";
        return $"{line}\n{underline}";
    }

    public static string RenderMini(string text, string? icon = null, ConsoleColor? color = null)
    {
        var colorCode = color.HasValue ? GetAnsiColor(color.Value) : Theme.Colors.Primary;
        var iconStr = icon ?? Theme.Icons.ArrowSmall;
        return $"{colorCode}{icon} {text}{Theme.Colors.Reset}";
    }

    private static string GetAnsiColor(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Green => "\x1b[38;5;46m",
            ConsoleColor.Cyan => "\x1b[38;5;51m",
            ConsoleColor.Yellow => "\x1b[38;5;226m",
            ConsoleColor.Red => "\x1b[38;5;196m",
            ConsoleColor.Magenta => "\x1b[38;5;201m",
            ConsoleColor.Blue => "\x1b[38;5;51m",
            ConsoleColor.White => "\x1b[38;5;255m",
            ConsoleColor.Gray => "\x1b[38;5;245m",
            ConsoleColor.DarkGray => "\x1b[38;5;240m",
            _ => "\x1b[38;5;46m"
        };
    }
}