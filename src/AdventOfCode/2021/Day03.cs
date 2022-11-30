using System;

namespace AdventOfCode._2021;

public class Day03 : Day
{
    public override string Example => "00100;11110;10110;10111;10101;01111;00111;11100;10000;11001;00010;01010";

    [Test(ExpectedResult = 198)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 3549854)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 230)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 3765399)]
    public override long Two() => Two(Input);

    private static int One(string input)
    {
        string[] binaries = input.Lines().ToArray();
        var gamma = "";
        var epsilon = "";
        var index = 0;

        while (index < binaries[0].Length)
        {
            var sum = binaries.SumBinaryStringArray(index);
            var oneMostOccuring = sum > (binaries.Length / 2);
            gamma += oneMostOccuring ? "1" : "0";
            epsilon += oneMostOccuring ? "0" : "1";

            index++;
        }

        return gamma.BinaryStringToInt() * epsilon.BinaryStringToInt();
    }

    private static int Two(string input)
    {
        var oxygenRating = input.Lines().ToArray();
        var scrubberRating = oxygenRating;
        var index = 0;

        while (index < oxygenRating[0].Length)
        {
            if (oxygenRating.Length > 1)
            {
                oxygenRating = oxygenRating
                    .Where(item => item[index].ToString() == MostOccuring(oxygenRating, index))
                    .ToArray();
            }

            if (scrubberRating.Length > 1)
            {
                scrubberRating = scrubberRating
                    .Where(item => item[index].ToString() != MostOccuring(scrubberRating, index))
                    .ToArray();
            }

            index++;
        }

        return oxygenRating.First().BinaryStringToInt() * scrubberRating.First().BinaryStringToInt();
    }

    private static string MostOccuring(string[] array, int index)
        => array.SumBinaryStringArray(index) >= ((decimal)array.Length / 2) ? "1" : "0";
}
