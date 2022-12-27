namespace AdventOfCode._2022;

public class Day23 : Day<long, long>
{
    public override string Example => @"....#..
..###.#
#...#.#
.#...##
#.###..
##.#.##
.#..#..";

    [Test(ExpectedResult = 110)]
    public long One_Example() => One(Example);

    [Test(ExpectedResult = 55244)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 5031)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 2508)]
    public override long Two() => Two(Input);

    private static long One(string input)
    {
        return 0;
    }

    private static long Two(string input)
    {
        return 0;
    }
}
