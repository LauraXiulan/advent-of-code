namespace AdventOfCode._2022;

public class Day19 : Day<int, int>
{
    public override string Example => @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

    [Test(ExpectedResult = 33)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 4364)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 58)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 2508)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        return 0;
    }

    private static int Two(string input)
    {
        return 0;
    }
}
