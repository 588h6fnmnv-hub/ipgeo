namespace ipgeo.UI;

public static class AnsiHelper
{
    private static readonly System.Text.RegularExpressions.Regex AnsiRegex = 
        new(@"\x1b\[[0-9;]*m", System.Text.RegularExpressions.RegexOptions.Compiled);

    public static int VisualLength(string input)
    {
        if (string.IsNullOrEmpty(input)) return 0;
        var clean = AnsiRegex.Replace(input, "");
        return clean.Length;
    }

    public static string PadRightVisual(string input, int totalWidth)
    {
        if (string.IsNullOrEmpty(input)) return new string(' ', totalWidth);
        var visualLen = VisualLength(input);
        if (visualLen >= totalWidth) return input;
        return input + new string(' ', totalWidth - visualLen);
    }

    public static string TruncateVisual(string input, int maxWidth)
    {
        if (string.IsNullOrEmpty(input)) return "";
        var clean = AnsiRegex.Replace(input, "");
        if (clean.Length <= maxWidth) return input;
        
        // Need to truncate while preserving ANSI codes
        var result = new System.Text.StringBuilder();
        var visualLen = 0;
        var i = 0;
        
        while (i < input.Length && visualLen < maxWidth - 3)
        {
            if (input[i] == '\x1b' && i + 1 < input.Length && input[i + 1] == '[')
            {
                // ANSI escape sequence - copy it entirely
                var j = i + 2;
                while (j < input.Length && input[j] != 'm') j++;
                if (j < input.Length)
                {
                    result.Append(input.Substring(i, j - i + 1));
                    i = j + 1;
                }
                else
                {
                    result.Append(input[i]);
                    i++;
                }
            }
            else
            {
                result.Append(input[i]);
                visualLen++;
                i++;
            }
        }
        
        if (visualLen >= maxWidth - 3)
        {
            result.Append("...");
        }
        
        return result.ToString();
    }

    public static string PadRightVisual(string input, int totalWidth, char paddingChar = ' ')
    {
        if (string.IsNullOrEmpty(input)) return new string(' ', totalWidth);
        var visualLen = VisualLength(input);
        if (visualLen >= totalWidth) return input;
        return input + new string(' ', totalWidth - visualLen);
    }
}