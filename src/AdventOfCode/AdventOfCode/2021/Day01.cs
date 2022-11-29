namespace AdventOfCode._2021;

public class Day01 : Day
{
    public override string Example => "199;200;208;210;200;207;240;269;260;263";

    [Test(ExpectedResult = 7)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1548)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 5)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 1589)]
    public override long Two() => Two(Input);

    private static int One(string input)
    {
        var numbers = input.Lines(int.Parse).ToArray();
        var increments = 0;

        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                increments++;
            }
        }

        return increments;
    }
    private static int Two(string input)
    {
        var numbers = input.Lines(int.Parse).ToArray();
        var increments = 0;

        for (int i = 3; i < numbers.Length; i++)
        {
            var sumPrev = numbers[i - 1] + numbers[i - 2] + numbers[i - 3];
            var sumCurr = numbers[i] + numbers[i - 1] + numbers[i - 2];

            if (sumCurr > sumPrev)
            {
                increments++;
            }
        }

        return increments;
    }
}
