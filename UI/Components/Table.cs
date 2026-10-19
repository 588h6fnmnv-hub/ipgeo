namespace ipgeo.UI.Components;

using ipgeo.UI;

public static class Table
{
    public static string Render(KeyValuePair<string, string>[] rows, int width = 70, bool showHeader = false, string? header1 = null, string? header2 = null)
    {
        var labelWidth = 20;
        var valueWidth = width - labelWidth - 5; // 3 for " │ " + 2 padding
        
        var lines = new List<string>();
        
        // Top border
        var topBorder = $"{Theme.Colors.PrimaryDim}{Theme.Box.TL}{new string(Theme.Box.H, labelWidth + 2)}{Theme.Box.T}{new string(Theme.Box.H, valueWidth + 2)}{Theme.Box.TR}{Theme.Colors.Reset}";
        
        // Header row
        if (showHeader && header1 != null && header2 != null)
        {
            var h1 = header1.PadRight(20);
            var h2 = header2.PadRight(width - 23);
            var headerLine = $"{Theme.Colors.Primary}{Theme.Colors.Bold} {h1}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.Primary}{Theme.Colors.Bold}{header2}{Theme.Colors.Reset}";
            var headerLine2 = $"{Theme.Colors.PrimaryDim}│ {Theme.Colors.Reset}{new string(' ', 19)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}{new string(Theme.Box.H, width - 23)}{Theme.Colors.Reset}";
            
            return string.Join("\n", new[]
            {
                $"{Theme.Colors.PrimaryDim}{Theme.Box.TL}{new string(Theme.Box.H, 22)}{Theme.Box.T}{new string(Theme.Box.H, width - 25)}{Theme.Box.TR}",
                $"{Theme.Colors.PrimaryDim}│ {Theme.Colors.Primary}{Theme.Colors.Bold}Property{Theme.Colors.Reset}{new string(' ', 11)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.Primary}{Theme.Colors.Bold}Value{Theme.Colors.Reset}{new string(' ', width - 29)}{Theme.Colors.PrimaryDim}│",
                $"{Theme.Colors.PrimaryDim}{Theme.Box.L}{new string(Theme.Box.H, 22)}{Theme.Box.X}{new string(Theme.Box.H, width - 25)}{Theme.Colors.PrimaryDim}{Theme.Box.R}",
            }.Select(l => $"{Theme.Colors.PrimaryDim}{l}{Theme.Colors.Reset}"));
        }
        
        var topBorder = $"{Theme.Colors.PrimaryDim}{Theme.Box.TL}{new string(Theme.Box.H, 22)}{Theme.Box.T}{new string(Theme.Box.H, width - 25)}{Theme.Box.TR}{Theme.Colors.Reset}";
        var header = $"{Theme.Colors.PrimaryDim}│ {Theme.Colors.Primary}{Theme.Colors.Bold}Property{Theme.Colors.Reset}{new string(' ', 11)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.Primary}{Theme.Colors.Bold}Value{Theme.Colors.Reset}{new string(' ', width - 29)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}";
        var separator = $"{Theme.Colors.PrimaryDim}{Theme.Box.L}{new string(Theme.Box.H, 22)}{Theme.Box.X}{new string(Theme.Box.H, width - 25)}{Theme.Box.R}{Theme.Colors.Reset}";
        
        var lines = new List<string>
        {
            topBorder,
            $"{Theme.Colors.PrimaryDim}│ {Theme.Colors.Primary}{Theme.Colors.Bold}Property{Theme.Colors.Reset}{new string(' ', 11)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.Primary}{Theme.Colors.Bold}Value{Theme.Colors.Reset}{new string(' ', width - 29)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}",
            separator
        };
        
        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            var key = row.Key.Length > 20 ? row.Key.Substring(0, 17) + "..." : row.Key;
            var value = row.Value;
            
            var keyPart = $" {key}".PadRight(22);
            var valueStr = value.Length > width - 25 ? value.Substring(0, width - 28) + "..." : value;
            var valuePart = $" {value}".PadRight(width - 25);
            
            var rowColor = i % 2 == 0 ? Theme.Colors.White : Theme.Colors.WhiteDim;
            var rowLine = $"{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {rowColor}{rows[i].Key.PadRight(20)}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.White}{rows[i].Value}{Theme.Colors.Reset}{new string(' ', Math.Max(0, width - 25 - rows[i].Value.Length))} {Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}";
            
            // Better formatting
            var keyFormatted = row.Key.PadRight(20);
            var val = row.Value.Length > width - 25 ? value.Substring(0, width - 28) + "..." : value;
            var valFormatted = val.PadRight(width - 25);
            
            var rowColor2 = i % 2 == 0 ? Theme.Colors.White : Theme.Colors.WhiteDim;
            lines.Add($"{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {rowColor2}{keyFormatted}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.White}{val}{Theme.Colors.Reset}{new string(' ', Math.Max(0, width - 25 - val.Length))} {Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}");
        }
        
        var bottomBorder = $"{Theme.Colors.PrimaryDim}{Theme.Box.BL}{new string(Theme.Box.H, 22)}{Theme.Box.B}{new string(Theme.Box.H, width - 25)}{Theme.Box.BR}{Theme.Colors.Reset}";
        
        var all = new List<string>
        {
            topBorder,
            $"{Theme.Colors.PrimaryDim}│ {Theme.Colors.Primary}{Theme.Colors.Bold}Property{Theme.Colors.Reset}{new string(' ', 11)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.Primary}{Theme.Colors.Bold}Value{Theme.Colors.Reset}{new string(' ', width - 29)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}",
            separator
        };
        all.AddRange(lines);
        lines.Add(bottomBorder);
        
        return string.Join("\n", 
            new[] { topBorder }.Select(l => $"{Theme.Colors.PrimaryDim}{l}{Theme.Colors.Reset}")
            .Concat(new[] { $"{Theme.Colors.PrimaryDim}│ {Theme.Colors.Primary}{Theme.Colors.Bold}Property{Theme.Colors.Reset}{new string(' ', 11)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset} {Theme.Colors.Primary}{Theme.Colors.Bold}Value{Theme.Colors.Reset}{new string(' ', width - 29)}{Theme.Colors.PrimaryDim}│{Theme.Colors.Reset}" })
            .Concat(new[] { separator })
            .Concat(lines)
            .Concat(new[] { bottomBorder }));
    }
    
    public static string RenderSimple(KeyValuePair<string, string>[] rows, int width = 70)
    {
        var lines = new List<string>();
        
        foreach (var row in rows)
        {
            var key = row.Key.PadRight(22);
            var value = row.Value.Length > width - 25 ? row.Value.Substring(0, width - 28) + "..." : row.Value;
            
            var line = $"{Theme.Colors.PrimaryDim}{Theme.Icons.ArrowSmall}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}{row.Key.PadRight(22)}{Theme.Colors.Reset} {Theme.Colors.White}{row.Value}{Theme.Colors.Reset}";
            // Just use simple format
            lines.Add($"{Theme.Colors.PrimaryDim}{Theme.Icons.ArrowSmall}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}{row.Key.PadRight(20)}{Theme.Colors.Reset} {Theme.Colors.White}{row.Value}{Theme.Colors.Reset}");
        }
        
        return string.Join("\n", lines);
    }
    
    public static string RenderTwoColumn(KeyValuePair<string, string>[] rows, int labelWidth = 22, int width = 70)
    {
        var lines = new List<string>();
        
        foreach (var row in rows)
        {
            var key = row.Key.Length > labelWidth - 2 ? row.Key.Substring(0, labelWidth - 5) + "..." : row.Key;
            var keyPart = $" {row.Key}".PadRight(labelWidth);
            var value = row.Value.Length > width - labelWidth - 4 ? row.Value.Substring(0, width - labelWidth - 7) + "..." : row.Value;
            
            var line = $"{Theme.Colors.PrimaryDim}{Theme.Icons.ArrowSmall}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}{key}{new string(' ', Math.Max(0, labelWidth - row.Key.Length))}{Theme.Colors.Reset} {Theme.Colors.White}{row.Value}{Theme.Colors.Reset}";
            lines.Add(line);
        }
        
        return string.Join("\n", lines);
    }
    
    public static string RenderSectioned(Dictionary<string, KeyValuePair<string, string>[]> sections, int width = 70)
    {
        var allLines = new List<string>();
        bool first = true;
        
        foreach (var section in sections)
        {
            if (!first)
            {
                allLines.Add("");
            }
            first = false;
            
            // Section header
            allLines.Add($"{Theme.Colors.Primary}{Theme.Colors.Bold}▸ {section.Key}{Theme.Colors.Reset}");
            allLines.Add($"{Theme.Colors.PrimaryDim}{new string(Theme.Box.H, section.Key.Length + 2)}{Theme.Colors.Reset}");
            
            var rows = section.Value;
            foreach (var row in rows)
            {
                var line = $"  {Theme.Colors.PrimaryDim}{Theme.Icons.ArrowSmall}{Theme.Colors.Reset} {Theme.Colors.PrimaryDim}{row.Key.PadRight(22)}{Theme.Colors.Reset} {Theme.Colors.White}{row.Value}{Theme.Colors.Reset}";
                allLines.Add(line);
            }
        }
        
        return string.Join("\n", allLines);
    }
}