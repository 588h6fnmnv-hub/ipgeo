namespace ipgeo.UI.Components;

using ipgeo.UI;

public static class Panel
{
    // Box drawing characters as chars
    private const char TL = '┌';
    private const char TR = '┐';
    private const char BL = '└';
    private const char BR = '┘';
    private const char H = '─';
    private const char V = '│';
    private const char T = '┬';
    private const char B = '┴';
    private const char L = '├';
    private const char R = '┤';
    private const char X = '┼';
    
    private const char TL_D = '╔';
    private const char TR_D = '╗';
    private const char BL_D = '╚';
    private const char BR_D = '╝';
    private const char H_D = '═';
    private const char V_D = '║';
    private const char T_D = '╦';
    private const char B_D = '╩';
    private const char L_D = '╠';
    private const char R_D = '╣';
    private const char X_D = '╬';

    public static string Render(string title, string content, string? subtitle = null, string? footer = null, int width = 80)
    {
        var lines = content.Split('\n');
        var maxContentWidth = width - 4;
        
        var titleLine = $" {title} ";
        var titlePadding = (width - titleLine.Length) / 2;
        var leftPad = new string(' ', titlePadding);
        var rightPad = new string(' ', width - titlePadding - titleLine.Length);
        
        var topBorder = $"{Theme.Colors.PrimaryDim}{TL}{new string(H, width - 2)}{TR}{Theme.Colors.Reset}";
        var titleBorder = $"{Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset}{leftPad}{Theme.Colors.Primary}{Theme.Colors.Bold}{title}{Theme.Colors.Reset}{Theme.Colors.PrimaryDim}{rightPad}{V}{Theme.Colors.Reset}";
        
        var separator = $"{Theme.Colors.PrimaryDim}{L}{new string(H, width - 2)}{R}{Theme.Colors.Reset}";
        
        var contentLines = new List<string>();
        foreach (var line in lines)
        {
            var padded = AnsiHelper.PadRightVisual(line, maxContentWidth);
            contentLines.Add($"{Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset} {Theme.Colors.White}{padded}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset}");
        }
        
        var result = new List<string>
        {
            topBorder,
            titleBorder
        };
        
        if (!string.IsNullOrEmpty(subtitle))
        {
            var subLine = $" {subtitle} ".PadRight(width - 2);
            result.Add($"{Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset}{Theme.Colors.Gray}{subLine}{Theme.Colors.Reset}{Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset}");
            result.Add(separator);
        }
        else
        {
            result.Add(separator);
        }
        
        result.AddRange(contentLines);
        
        if (!string.IsNullOrEmpty(footer))
        {
            result.Add(separator);
            var footLine = $" {footer} ".PadRight(width - 2);
            result.Add($"{Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset}{Theme.Colors.Gray}{footLine}{Theme.Colors.Reset}{Theme.Colors.PrimaryDim}{V}{Theme.Colors.Reset}");
        }
        
        var bottomBorder = $"{Theme.Colors.PrimaryDim}{BL}{new string(H, width - 2)}{BR}{Theme.Colors.Reset}";
        result.Add(bottomBorder);
        
        return string.Join("\n", result);
    }
    
    public static string RenderSimple(string title, string content, int width = 70)
    {
        var lines = content.Split('\n');
        var maxContentWidth = width - 4;
        
        var topBorder = $"{Theme.Colors.PrimaryDim}┌{new string('─', width - 2)}┐{Theme.Colors.Reset}";
        var titleLine = $" {title} ";
        var titlePadding = (width - titleLine.Length) / 2;
        var leftPad = new string(' ', titlePadding);
        var rightPad = new string(' ', width - titlePadding - titleLine.Length);
        var titleBorder = $"{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}{leftPad}{Theme.Colors.Primary}{Theme.Colors.Bold}{title}{Theme.Colors.Reset}{rightPad}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}";
        
        var separator = $"{Theme.Colors.PrimaryDim}├{new string('─', width - 2)}┤{Theme.Colors.Reset}";
        
        var contentLines = new List<string>();
        foreach (var line in lines)
        {
            var padded = AnsiHelper.PadRightVisual(line, maxContentWidth);
            contentLines.Add($"{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.White}{padded}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}");
        }
        
        var bottomBorder = $"{Theme.Colors.PrimaryDim}└{new string('─', width - 2)}┘{Theme.Colors.Reset}";
        
        var all = new List<string> { topBorder, titleBorder, separator };
        all.AddRange(contentLines);
        all.Add(bottomBorder);
        
        return string.Join("\n", all.Select(l => $"{Theme.Colors.PrimaryDim}{l}{Theme.Colors.Reset}"));
    }
    
    public static string RenderError(string message, int width = 70)
    {
        var lines = message.Split('\n');
        var maxContentWidth = width - 4;
        
        var topBorder = $"\x1b[38;5;124m╔{new string('═', width - 2)}╗\x1b[0m";
        var titleLine = $" ✗ ERROR ";
        var titlePadding = (width - titleLine.Length) / 2;
        var leftPad = new string(' ', titlePadding);
        var rightPad = new string(' ', width - titlePadding - titleLine.Length);
        var titleBorder = $"\x1b[38;5;124m║\x1b[0m\x1b[38;5;196m\x1b[1m{titleLine}\x1b[0m\x1b[38;5;124m{rightPad}║\x1b[0m";
        
        var separator = $"\x1b[38;5;124m╠{new string('═', width - 2)}╣\x1b[0m";
        
        var contentLines = new List<string>();
        foreach (var line in lines)
        {
            var padded = AnsiHelper.PadRightVisual(line, maxContentWidth);
            contentLines.Add($"\x1b[38;5;124m║\x1b[0m \x1b[38;5;196m{padded}\x1b[0m \x1b[38;5;124m║\x1b[0m");
        }
        
        var bottomBorder = $"\x1b[38;5;124m╚{new string('═', width - 2)}╝\x1b[0m";
        
        var all = new List<string> { topBorder, titleBorder, separator };
        all.AddRange(contentLines);
        all.Add(bottomBorder);
        
        return string.Join("\n", all.Select(l => $"\x1b[38;5;124m{l}\x1b[0m"));
    }
    
    public static string RenderWarning(string title, string content, int width = 70)
    {
        var lines = content.Split('\n');
        var maxContentWidth = width - 4;
        
        var topBorder = $"\x1b[38;5;172m┌{new string('─', width - 2)}┐\x1b[0m";
        var titleLine = $" ⚠ {title} ";
        var titlePadding = (width - titleLine.Length) / 2;
        var leftPad = new string(' ', titlePadding);
        var rightPad = new string(' ', width - titlePadding - titleLine.Length);
        var titleBorder = $"\x1b[38;5;172m│\x1b[0m\x1b[38;5;214m\x1b[1m{titleLine}\x1b[0m\x1b[38;5;172m{rightPad}│\x1b[0m";
        
        var separator = $"\x1b[38;5;172m├{new string('─', width - 2)}┤\x1b[0m";
        
        var contentLines = new List<string>();
        foreach (var line in lines)
        {
            var padded = AnsiHelper.PadRightVisual(line, maxContentWidth);
            contentLines.Add($"\x1b[38;5;172m│\x1b[0m \x1b[38;5;214m{padded}\x1b[0m \x1b[38;5;172m│\x1b[0m");
        }
        
        var bottomBorder = $"\x1b[38;5;172m└{new string('─', width - 2)}┘\x1b[0m";
        
        var all = new List<string> { topBorder, titleBorder, separator };
        all.AddRange(contentLines);
        all.Add(bottomBorder);
        
        return string.Join("\n", all.Select(l => $"\x1b[38;5;172m{l}\x1b[0m"));
    }
    
    public static string RenderInfo(string title, string content, int width = 70)
    {
        var lines = content.Split('\n');
        var maxContentWidth = width - 4;
        
        var topBorder = $"\x1b[38;5;37m┌{new string('─', width - 2)}┐\x1b[0m";
        var titleLine = $" ℹ {title} ";
        var titlePadding = (width - titleLine.Length) / 2;
        var leftPad = new string(' ', titlePadding);
        var rightPad = new string(' ', width - titlePadding - titleLine.Length);
        var titleBorder = $"\x1b[38;5;37m│\x1b[0m\x1b[38;5;51m\x1b[1m{titleLine}\x1b[0m\x1b[38;5;37m{rightPad}│\x1b[0m";
        
        var separator = $"\x1b[38;5;37m├{new string('─', width - 2)}┤\x1b[0m";
        
        var contentLines = new List<string>();
        foreach (var line in lines)
        {
            var padded = AnsiHelper.PadRightVisual(line, maxContentWidth);
            contentLines.Add($"\x1b[38;5;37m│\x1b[0m \x1b[38;5;51m{padded}\x1b[0m \x1b[38;5;37m│\x1b[0m");
        }
        
        var bottomBorder = $"\x1b[38;5;37m└{new string('─', width - 2)}┘\x1b[0m";
        
        var all = new List<string> { topBorder, titleBorder, separator };
        all.AddRange(contentLines);
        all.Add(bottomBorder);
        
        return string.Join("\n", all.Select(l => $"\x1b[38;5;37m{l}\x1b[0m"));
    }
}