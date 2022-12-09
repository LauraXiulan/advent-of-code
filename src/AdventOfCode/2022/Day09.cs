namespace AdventOfCode._2022;

public class Day09 : Day<int, int>
{
    public override string Example => @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

    public static string ExampleTwo => @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";

    [Test(ExpectedResult = 13)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 6037)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 36)]
    public int Two_Example() => Two(ExampleTwo);

    [Test(ExpectedResult = 2485)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => TraverseGrid(input, 1);

    private static int Two(string input)
        => TraverseGrid(input, 9);

    private static int TraverseGrid(string input, int size)
    {
        var bridge = new RopeBridge(size);
        foreach (var line in input.Lines())
        {
            bridge.Move(line);
        }
        return bridge.Tails.Last().Distinct().Count();
    }
}

public record RopeBridge
{
    public RopeBridge(int size)
    {
        Tails = Enumerable.Range(0, size).Select(r => new List<string> { "200,250" }).ToArray();
    }

    public List<string> Head { get; } = new List<string> { "200,250" };
    public List<string>[] Tails { get; } = Array.Empty<List<string>>();

    public void Move(string input)
    {
        var head = Head.Last().Split(",").Int32s().ToArray();
        var x = head[0];
        var y = head[1];

        var splitted = input.Split(' ');

        for (int i = 0; i < int.Parse(splitted[1]); i++)
        {
            switch (splitted[0])
            {
                case "U":
                    y++;
                    break;
                case "D":
                    y--;
                    break;
                case "L":
                    x--;
                    break;
                case "R":
                    x++;
                    break;
                default:
                    break;
            }

            SetCoordinates(x, y);
        }

    }

    private void SetCoordinates(int x, int y)
    {
        Head.Add($"{x},{y}");

        var index = 0;
        var prev = Head.Last().Split(",").Int32s().ToArray(); ;
        while (index < Tails.Length)
        {
            var current = Tails[index].Last().Split(",").Int32s().ToArray();
            var distanceX = prev[0] - current[0];
            var distanceY = prev[1] - current[1];

            if (Math.Abs(distanceX) > 1 || Math.Abs(distanceY) > 1)
            {
                current[0] += Math.Sign(distanceX);
                current[1] += Math.Sign(distanceY);
                Tails[index].Add($"{current[0]},{current[1]}");
            }
            prev = current;
            index++;
        }
    }
}
