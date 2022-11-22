using AdventOfCode.Tooling;
using FluentAssertions;

namespace AdventOfCode._2021;

public class Day01Part02
{
    [Test]
    public void DepthIncrements()
    {
        var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "../../../2021/Day01.txt"))
            .Lines()
            .Select(int.Parse)
            .ToArray();

        //var input = new[]
        //{
        //    199,//A
        //    200,//A //B
        //    208,//A //B //C
        //    210, //B //C
        //    200, //C
        //    207,
        //    240,
        //    269,
        //    260,
        //    263,
        //};
        var increments = 0;

        for (int i = 3; i < input.Length; i++)
        {
            var sumPrev = input[i -1] + input[i -2] + input[i -3];
            var sumCurr = input[i] + input[i - 1] + input[i - 2];

            if (sumCurr > sumPrev)
            {
                increments++;
            }
        }

        increments.Should().Be(1589);
    }
}
