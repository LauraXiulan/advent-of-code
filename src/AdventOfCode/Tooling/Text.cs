﻿using System.Text.RegularExpressions;

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

    public static int SumBinaryStringArray(this string[] array, int index) => array.Sum(i => int.Parse(i[index].ToString()));

    public static int BinaryStringToInt(this string binary) => Convert.ToInt32(binary, 2);

    public static IEnumerable<int> Int32s(this string str) => NotADigit().Split(str).Where(s => s.Any()).Select(int.Parse);
    public static IEnumerable<int> Int32s(this IEnumerable<string> stringSet) => stringSet.SelectMany(str => NotADigit().Split(str).Where(s => s.Any()).Select(int.Parse));

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex NotADigit();

    public static IReadOnlyList<string> SplitInHalf(this string str) => str.Insert(str.Length / 2, ";").Lines();

    // a-z = 1-26 A-Z = 27-52
    public static int CharToIntRepresentation(this char character) => char.IsLower(character) ? character - 96 : character - 64 + 26;
}
