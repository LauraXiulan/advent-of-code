namespace AdventOfCode._2022;

public class Day08 : Day<int, int>
{
    public override string Example => "30373;25512;65332;33549;35390";

    [Test(ExpectedResult = 21)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1717)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 8)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 321975)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var lines = input.Lines().ToArray();
        var grid = new Grid(lines);
        var sum = (lines.Length - 1) * 4;

        for (int r = 1; r < lines.Length - 1; r++)
        {
            var row = grid.Row(r);
            for (int c = 1; c < row.Length - 1; c++)
            {
                var height = row[c];

                var col = grid.Col(c);

                var rowOne = row[..c];
                var rowTwo = row[(c + 1)..];

                var columnOne = col[..r];
                var columnTwo = col[(r + 1)..];

                var isVisible = rowOne.All(r => r < height)
                    || rowTwo.All(r => r < height)
                    || columnOne.All(r => r < height)
                    || columnTwo.All(r => r < height);

                if (isVisible)
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    private static int Two(string input)
    {
        var lines = input.Lines().ToArray();
        var grid = new Grid(lines);
        List<int> scores = new();

        for (int r = 1; r < lines.Length - 1; r++)
        {
            var row = grid.Row(r);
            for (int c = 1; c < row.Length - 1; c++)
            {
                var height = row[c];

                var col = grid.Col(c);

                var rowOne = string.Concat(row[..c].Reverse());
                var rowTwo = row[(c + 1)..];

                var columnOne = string.Concat(col[..r].Reverse());
                var columnTwo = col[(r + 1)..];

                int ScenicScore(string line)
                {
                    var sum = 0;
                    foreach (var item in line.Select(s => int.Parse(s.ToString())))
                    {
                        if (item < int.Parse(height.ToString()))
                        {
                            sum++;
                        }
                        else
                        {
                            sum++;
                            break;
                        }
                    }
                    return sum;
                }

                var one = ScenicScore(rowOne);
                var two = ScenicScore(rowTwo);
                var three = ScenicScore(columnOne);
                var four = ScenicScore(columnTwo);

                var scenicScore = one * two * three * four;

                scores.Add(scenicScore);
            }
        }

        return scores.Max();
    }
}

public sealed record Grid
{
    private readonly string[] Input;

    public Grid(string[] input)
    {
        Input = input;
    }

    public string Row(int index) => Input[index];
    public string Col(int index) => string.Concat(Input.Select(s => s[index]).ToArray());
}
