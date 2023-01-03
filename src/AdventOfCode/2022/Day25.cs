using FluentAssertions;
using System.Data;

namespace AdventOfCode._2022;

public class Day25 : Day<string, string>
{
    public override string Example => "1=-0-2;12111;2=0=;21;2=01;111;20012;112;1=-1=;1-12;12;1=;122";

    [Test(ExpectedResult = "2=-1=0")]
    public string One_Example() => LongToSnafu(Example.Lines(Sum).Sum());

    [Test(ExpectedResult = "2-=2-0=-0-=0200=--21")]
    public override string One() => LongToSnafu(Input.Lines(Sum).Sum());

    public override string Two() => throw new NotImplementedException();

    private static readonly Dictionary<char, int> Digits = new()
    {
        { '2', 2 }, { '1', 1 }, { '0', 0 }, { '-', -1 }, { '=', -2 },
    };

    internal static long SnafuToLong(char input, int position) => (long)(Digits[input] * Math.Pow(5, position));
    internal static long Sum(string input) => input.Reverse().Select(SnafuToLong).Reverse().Sum();
    internal static string LongToSnafu(long number)
    {
        var snafu = "";
        var contains = 0L;
        var prev = 0L;
        var next = 1L;

        while (number > next + contains)
        {
            contains += 2 * prev;
            prev = next;
            next *= 5;
        }

        while (prev > 0)
        {
            next = prev;
            prev /= 5;

            var digit = number >= 0 ? (number + contains) / next : (number - contains) / next;
            contains -= 2 * prev;

            var part = Digits.First(d => d.Value == digit).Key;
            snafu += part;
            number -= digit * next;
        }

        return snafu;
    }
}

public class Day25Tests
{
    [TestCase('1', 0, 1L)]
    [TestCase('2', 0, 2L)]
    [TestCase('=', 0, -2L)]
    [TestCase('-', 0, -1L)]
    [TestCase('1', 1, 5L)]
    [TestCase('2', 1, 10L)]
    [TestCase('=', 1, -10L)]
    [TestCase('-', 1, -5L)]
    public void SnafuToLong(char input, int position, long expected) => Day25.SnafuToLong(input, position).Should().Be(expected);

    [TestCase("1-0---0", 12345)]
    [TestCase("1121-1110-1=0", 314159265)]
    public void Sum(string input, long expected) => Day25.Sum(input).Should().Be(expected);

    [TestCase(4890, "2=-1=0")]
    [TestCase(314159265, "1121-1110-1=0")]
    public void LongToSnafu(long input, string expected) => Day25.LongToSnafu(input).Should().Be(expected);
}
