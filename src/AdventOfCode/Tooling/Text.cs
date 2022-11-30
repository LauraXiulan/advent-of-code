using System.Text.RegularExpressions;

namespace AdventOfCode.Tooling;

public static partial class Text
{
    private const StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public static IReadOnlyList<string> Lines(this string str, StringSplitOptions options = SplitOptions)
        => str.Split(new[] { "\r\n", "\n", ";" }, options);

    public static IEnumerable<T> Lines<T>(this string str, Func<string, T> selector) => str.Lines().Select(selector);

    public static IEnumerable<string[]> GroupedLines(this string str, StringSplitOptions options = SplitOptions)
    {
        var buffer = new List<string>();
        foreach (var line in str.Lines(default))
        {
            if (string.IsNullOrEmpty(line))
            {
                if (buffer.Any())
                {
                    yield return buffer.ToArray();
                    buffer.Clear();
                }
            }
            else
            {
                buffer.Add(options.HasFlag(StringSplitOptions.TrimEntries) ? line.Trim() : line);
            }
        }

        if (buffer.Any())
        {
            yield return buffer.ToArray();
        }
    }

    public static IEnumerable<int> Int32s(this string str)
        => NotADigit().Replace(str, ";").Lines(int.Parse);

    [GeneratedRegex("[^\\d]")]
    private static partial Regex NotADigit();
}
