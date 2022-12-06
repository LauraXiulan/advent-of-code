namespace AdventOfCode._2022;

public class Day02 : Day<int, int>
{
    public override string Example => @"A Y
B X
C Z";

    [Test(ExpectedResult = 15)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 14163)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 12)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 12091)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => input.Lines(s => s switch
        {
            "A X" => 1 + 3,
            "A Y" => 2 + 6,
            "A Z" => 3 + 0,
            "B X" => 1 + 0,
            "B Y" => 2 + 3,
            "B Z" => 3 + 6,
            "C X" => 1 + 6,
            "C Y" => 2 + 0,
            "C Z" => 3 + 3,
            _ => 0
        }).Sum();

    private static int Two(string input)
        => input.Lines(s => s switch
        {
            "A X" => 3 + 0,
            "A Y" => 1 + 3,
            "A Z" => 2 + 6,
            "B X" => 1 + 0,
            "B Y" => 2 + 3,
            "B Z" => 3 + 6,
            "C X" => 2 + 0,
            "C Y" => 3 + 3,
            "C Z" => 1 + 6,
            _ => 0
        }).Sum();
}
