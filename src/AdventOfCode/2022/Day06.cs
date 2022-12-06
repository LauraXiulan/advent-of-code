namespace AdventOfCode._2022;

public class Day06 : Day<int, int>
{
    public override string Example => "mjqjpqmgbljsphdztnvjfqwrcgsmlb";

    [Test(ExpectedResult = 7)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1757)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 19)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 2950)]
    public override int Two() => Two(Input);

    private static int One(string input) => FindDistinct(input, 4);
    private static int Two(string input) => FindDistinct(input, 14);

    private static int FindDistinct(string input, int distinct)
        => Enumerable.Range(0, input.Length).First(index => AllDistinct(input, distinct, index)) + distinct;

    private static bool AllDistinct(string input, int distinct, int index)
    {
        var list = new HashSet<char>();
        return input.Skip(index).Take(distinct).All(list.Add);
    }
}
