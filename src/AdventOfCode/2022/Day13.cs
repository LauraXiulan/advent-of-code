using System.Text.Json;

namespace AdventOfCode._2022;

public class Day13 : Day<int, int>
{
    public override string Example => @"
[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";

    [Test(ExpectedResult = 13)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 6395)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 29)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 24921)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => input.GroupedLines()
            .Select(line => line.Select(l => JsonSerializer.Deserialize<dynamic>(l)))
            .Select((line, index) => Compare(line.ToArray()!) == -1 ? index + 1 : 0).Sum();

    private static int Two(string input)
        => input.GroupedLines()
            .Select(line => line.Select(l => JsonSerializer.Deserialize<dynamic>(l)))
            .Select((line, index) => Compare(line.ToArray()!) == -1 ? index + 1 : 0).Sum();

    private static int Compare(params dynamic[] toCompare)
    {
        var current = toCompare[0];
        var other = toCompare[1];

        if (current.ValueKind is JsonValueKind.Number && other.ValueKind is JsonValueKind.Number)
        {
            return Math.Sign(int.Parse(current.ToString()) - int.Parse(other.ToString()));
        }

        if (current.ValueKind is JsonValueKind.Number)
        {
            return Compare(JsonSerializer.Deserialize<dynamic>($"[{int.Parse(current.ToString())}]"), other);
        }

        if (other.ValueKind is JsonValueKind.Number)
        {
            return Compare(current, JsonSerializer.Deserialize<dynamic>($"[{int.Parse(other.ToString())}]"));
        }

        var c = JsonSerializer.Deserialize<dynamic[]>(current);
        var o = JsonSerializer.Deserialize<dynamic[]>(other);

        if (c.Length == 0 && o.Length == 0)
        {
            return 0;
        }

        if (c.Length == 0)
        {
            return -1;
        }

        if (o.Length == 0)
        {
            return 1;
        }

        for (int i = 0; i < Math.Min(c.Length, o.Length); i++)
        {
            var result = Compare(c[i], o[i]);
            if (result != 0)
            {
                return result;
            }
        }

        return Math.Sign(c.Length - o.Length);
    }
}