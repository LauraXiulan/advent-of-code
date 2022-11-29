namespace AdventOfCode.Tooling;

public static class Text
{
    private const StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public static IReadOnlyList<string> Lines(this string str, StringSplitOptions options = SplitOptions)
        => str.Split(new[] { "\r\n", "\n", ";" }, options);

    public static IEnumerable<T> Lines<T>(this string str, Func<string, T> selector) => str.Lines().Select(selector);
}
