namespace AdventOfCode._2021;

public class Day02 : Day
{
    public override string Example => "forward 5;down 5;forward 8;up 3;down 8;forward 2";

    [Test(ExpectedResult = 150)]
    public override int One_Example() => One(Example);

    [Test(ExpectedResult = 2215080)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 900)]
    public override int Two_Example() => Two(Example);

    [Test(ExpectedResult = 1864715580)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var actions = input.Lines().ToArray();
        var horizontal = 0;
        var depth = 0;

        for (int i = 0; i < actions.Length; i++)
        {
            var current = actions[i];
            var number = int.Parse(current[^1..]);

            if (current.Contains("forward"))
            {
                horizontal += number;
            }
            else if (current.Contains("down"))
            {
                depth += number;
            }
            else if (current.Contains("up"))
            {
                depth -= number;
            }
        }

        return depth * horizontal;
    }

    private static int Two(string input)
    {
        var actions = input.Lines().ToArray();
        var horizontal = 0;
        var depth = 0;
        var aim = 0;

        for (int i = 0; i < actions.Length; i++)
        {
            var current = actions[i];
            var number = int.Parse(current[^1..]);

            if (current.Contains("forward"))
            {
                horizontal += number;
                depth += aim * number;
            }
            else if (current.Contains("down"))
            {
                aim += number;
            }
            else if (current.Contains("up"))
            {
                aim -= number;
            }
        }

        return depth * horizontal;
    }
}
