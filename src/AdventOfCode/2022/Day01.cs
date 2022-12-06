namespace AdventOfCode._2022;

public class Day01 : Day<int, int>
{
    public override string Example => @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";

    [Test(ExpectedResult = 24000)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 67622)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 45000)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 201491)]
    public override int Two() => Two(Input);

    private static int One(string input) => input.GroupedLines().Select(c => c.Int32s().Sum()).Max();

    private static int Two(string input) => input.GroupedLines().Select(c => c.Int32s().Sum()).OrderByDescending(i => i).Take(3).Sum();
}
