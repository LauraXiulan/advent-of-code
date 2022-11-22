using AdventOfCode.Tooling;
using FluentAssertions;

namespace AdventOfCode._2021;

public class Day01_Part01
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
        //    199,
        //    200,
        //    208,
        //    210,
        //    200,
        //    207,
        //    240,
        //    269,
        //    260,
        //    263,
        //};
        var increments = 0;

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] > input[i - 1])
            {
                increments++;
            }
        }

        increments.Should().Be(1548);
    }
}
