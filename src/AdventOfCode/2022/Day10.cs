namespace AdventOfCode._2022;

public class Day10 : Day<int, string>
{
    public override string Example => "addx 15;addx -11;addx 6;addx -3;addx 5;addx -1;addx -8;addx 13;addx 4;noop;addx -1;addx 5;addx -1;addx 5;addx -1;addx 5;addx -1;addx 5;addx -1;addx -35;addx 1;addx 24;addx -19;addx 1;addx 16;addx -11;noop;noop;addx 21;addx -15;noop;noop;addx -3;addx 9;addx 1;addx -3;addx 8;addx 1;addx 5;noop;noop;noop;noop;noop;addx -36;noop;addx 1;addx 7;noop;noop;noop;addx 2;addx 6;noop;noop;noop;noop;noop;addx 1;noop;noop;addx 7;addx 1;noop;addx -13;addx 13;addx 7;noop;addx 1;addx -33;noop;noop;noop;addx 2;noop;noop;noop;addx 8;noop;addx -1;addx 2;addx 1;noop;addx 17;addx -9;addx 1;addx 1;addx -3;addx 11;noop;noop;addx 1;noop;addx 1;noop;noop;addx -13;addx -19;addx 1;addx 3;addx 26;addx -30;addx 12;addx -1;addx 3;addx 1;noop;noop;noop;addx -9;addx 18;addx 1;addx 2;noop;noop;addx 9;noop;noop;noop;addx -1;addx 2;addx -37;addx 1;addx 3;noop;addx 15;addx -21;addx 22;addx -6;addx 1;noop;addx 2;addx 1;noop;addx -10;noop;noop;addx 20;addx 1;addx 2;addx 2;addx -6;addx -11;noop;noop;noop";

    [Test(ExpectedResult = 13140)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 13520)]
    public override int One() => One(Input);

    [Test(ExpectedResult = "PGPHBEAB")]
    public override string Two() => Two(Input);

    private static int One(string input)
        => new List<int> { 1, 1 }
            .Concat(XValues(input))
            .Select((v, index) => index % 40 == 20 ? v * index : 0)
            .Sum();

    private static string Two(string input)
    {
        var xValues = new List<int> { 1 }.Concat(XValues(input)).ToArray();
        var width = 40;

        bool OnCorrectPosition(int value, int pos)
            => value == pos || value - 1 == pos || value + 1 == pos;

        char CharacterValue(int row, int pos, char original)
            => OnCorrectPosition(xValues[width * row + pos], pos) ? '#' : original;

        var rows = new Map(width, 6).Rows
            .Select((row, r) => row.Select((character, i) => CharacterValue(r, i, character)))
            .ToArray();

        // TODO: Parse rows
        foreach (var row in rows)
        {
            Console.WriteLine(string.Concat(row));
        }

        return "PGPHBEAB";
    }

    private static IEnumerable<int> XValues(string input)
    {
        var last = 1;
        foreach (var line in input.Lines())
        {
            yield return last;
            if (line.StartsWith("addx"))
            {
                last += int.Parse(line[5..]);
                yield return last;
            }
        }
    }
}

public record Map
{
    public Map(int width, int height)
    {
        Rows = Enumerable.Range(0, height).Select(r => Enumerable.Repeat('.', width)).ToArray();
    }

    public IEnumerable<char>[] Rows { get; }
}
