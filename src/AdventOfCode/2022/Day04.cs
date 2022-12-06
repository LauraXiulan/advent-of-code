namespace AdventOfCode._2022;

public class Day04 : Day<int, int>
{
    public override string Example => "2-4,6-8;2-3,4-5;5-7,7-9;2-8,3-7;6-6,4-6;2-6,4-8";

    [Test(ExpectedResult = 2)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 569)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 4)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 936)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => input.Lines(line => line.Split(',')).Select(line =>
        {
            var ranges = line.Int32s();
            var firstLine = ranges.Take(2);
            var secondLine = ranges.Skip(2);

            return firstLine.ContainedIn(secondLine) || secondLine.ContainedIn(firstLine) ? 1 : 0;
        }).Sum();

    private static int Two(string input)
        => input.Lines(line => line.Split(',')).Select(line =>
        {
            var ranges = line.Int32s();
            var firstLine = ranges.Take(2);
            var secondLine = ranges.Skip(2);

            return firstLine.OverlapsWith(secondLine) || secondLine.OverlapsWith(firstLine) ? 1 : 0;
        }).Sum();
}
