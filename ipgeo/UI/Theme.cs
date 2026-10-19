namespace ipgeo.UI;

public static class Theme
{
    // Dark/Hacker mode color palette
    public static class Colors
    {
        // Primary hacker green palette
        public const string Primary = "\x1b[38;5;46m";      // Bright green
        public const string PrimaryDim = "\x1b[38;5;28m";   // Dim green
        public const string PrimaryBright = "\x1b[38;5;82m"; // Bright green
        
        // Accent colors
        public const string Cyan = "\x1b[38;5;51m";         // Bright cyan
        public const string CyanDim = "\x1b[38;5;37m";      // Dim cyan
        
        // Amber/Orange for warnings/highlights
        public const string Amber = "\x1b[38;5;214m";       // Amber
        public const string AmberDim = "\x1b[38;5;172m";    // Dim amber
        public const string Orange = "\x1b[38;5;208m";      // Orange
        
        // Red for errors
        public const string Red = "\x1b[38;5;196m";         // Bright red
        public const string RedDim = "\x1b[38;5;124m";      // Dim red
        
        // Grays for muted text
        public const string Gray = "\x1b[38;5;240m";        // Medium gray
        public const string GrayDark = "\x1b[38;5;236m";    // Dark gray
        public const string GrayDim = "\x1b[38;5;238m";     // Dim gray
        public const string GrayLight = "\x1b[38;5;245m";   // Light gray
        
        // White
        public const string White = "\x1b[38;5;255m";       // Pure white
        public const string WhiteDim = "\x1b[38;5;250m";    // Dim white
        
        // Reset
        public const string Reset = "\x1b[0m";
        public const string Bold = "\x1b[1m";
        public const string Dim = "\x1b[2m";
        public const string Underline = "\x1b[4m";
        public const string Blink = "\x1b[5m";
        public const string Reverse = "\x1b[7m";
        public const string Hidden = "\x1b[8m";
    }

    // Unicode box drawing characters
    public static class Box
    {
        // Single line
        public const string TL = "┌";
        public const string TR = "┐";
        public const string BL = "└";
        public const string BR = "┘";
        public const string H = "─";
        public const string V = "│";
        public const string T = "┬";
        public const string B = "┴";
        public const string L = "├";
        public const string R = "┤";
        public const string X = "┼";
        
        // Double line
        public const string TL_D = "╔";
        public const string TR_D = "╗";
        public const string BL_D = "╚";
        public const string BR_D = "╝";
        public const string H_D = "═";
        public const string V_D = "║";
        public const string T_D = "╦";
        public const string B_D = "╩";
        public const string L_D = "╠";
        public const string R_D = "╣";
        public const string X_D = "╬";
        
        // Rounded
        public const string TL_R = "╭";
        public const string TR_R = "╮";
        public const string BL_R = "╰";
        public const string BR_R = "╯";
        public const string H_R = "─";
        public const string V_R = "│";
    }

    // Icons for hacker theme
    public static class Icons
    {
        public const string Arrow = "▶";
        public const string ArrowSmall = "▸";
        public const string Bullet = "●";
        public const string BulletEmpty = "○";
        public const string Check = "✓";
        public const string Cross = "✗";
        public const string Warning = "⚠";
        public const string Info = "ℹ";
        public const string Location = "📍";
        public const string Globe = "🌐";
        public const string Server = "🖧";
        public const string Lock = "🔒";
        public const string Unlock = "🔓";
        public const string Satellite = "📡";
        public const string Shield = "🛡";
        public const string Skull = "☠";
        public const string Ghost = "👻";
        public const string Terminal = "⌨";
        public const string Cursor = "▊";
        public const string Pulse = "◉";
        public const string Wave = "≋";
        public const string Bolt = "⚡";
        public const string Gear = "⚙";
        public const string Search = "🔍";
        public const string Target = "🎯";
        public const string Radar = "📡";
        public const string Satellite2 = "🛰";
        public const string Bug = "🐛";
        public const string Code = "💻";
        public const string Data = "💾";
        public const string Network = "🌐";
        public const string Fire = "🔥";
        public const string Ice = "❄";
    }

    // Spinner frames for hacker theme
    public static readonly string[] SpinnerFrames = 
    {
        "▹▹▹▹▹",
        "▸▹▹▹▹",
        "▹▸▹▹▹",
        "▹▹▸▹▹",
        "▹▹▹▸▹",
        "▹▹▹▹▸",
        "▹▹▹▹▹",
    };

    public static readonly string[] SpinnerFrames2 = 
    {
        "⠋",
        "⠙",
        "⠹",
        "⠸",
        "⠼",
        "⠴",
        "⠦",
        "⠧",
        "⠇",
        "⠏",
    };

    public static readonly string[] SpinnerHacker = 
    {
        "[⣾]",
        "[⣽]",
        "[⣻]",
        "[⢿]",
        "[⡿]",
        "[⣟]",
        "[⣯]",
        "[⣷]",
    };

    public static readonly string[] SpinnerBrackets = 
    {
        "[    ]",
        "[=   ]",
        "[==  ]",
        "[=== ]",
        "[====]",
        "[ ===]",
        "[  ==]",
        "[   =]",
        "[    ]",
    };

    public static readonly string[] SpinnerHacker2 = 
    {
        "⣷",
        "⣯",
        "⣟",
        "⡿",
        "⢿",
        "⣻",
        "⣽",
        "⣾",
    };

// Progress bar styles
    public static class ProgressBar
    {
        public const char Fill = '█';
        public const char FillDim = '░';
        public const char Left = '[';
        public const char Right = ']';
        public const char LeftR = '╢';
        public const char RightR = '╟';
    }

    // Status prefixes
    public static class Prefix
    {
        public static string Success => $"{Colors.Primary}{Icons.Check}{Colors.Reset} ";
        public static string Error => $"{Colors.Red}{Icons.Cross}{Colors.Reset} ";
        public static string Warning => $"{Colors.Amber}{Icons.Warning}{Colors.Reset} ";
        public static string Info => $"{Colors.Cyan}{Icons.Info}{Colors.Reset} ";
        public static string Loading => $"{Colors.Cyan}{Icons.Radar}{Colors.Reset} ";
        public static string Arrow => $"{Colors.Primary}{Icons.ArrowSmall}{Colors.Reset} ";
        public static string Bullet => $"{Colors.PrimaryDim}{Icons.Bullet}{Colors.Reset} ";
    }
}