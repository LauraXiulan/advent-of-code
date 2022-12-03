using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2022;

public class Day03 : Day
{
    public override string Example => "vJrwpWtwJgWrhcsFMMfFFhFp;jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL;PmmdzqPrVvPwwTWBwg;wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn;ttgJtRGJQctTZtZT;CrZsJsPPZsGzwwsLwLmpwMDw";

    [Test(ExpectedResult = 157)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 7737)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 70)]
    public int Two_example() => Two(Example);

    [Test(ExpectedResult = 2697)]
    public override long Two() => Two(Input);

    private static int One(string input)
        => input.Lines(item =>
        {
            var firstPart = item[..(item.Length / 2)];
            var secondPart = item[(item.Length / 2)..];
            var equalPart = firstPart.Intersect(secondPart).First();

            return char.IsLower(equalPart) ? equalPart - 96 : equalPart - 64 + 26;
        }).Sum();

    private static int Two(string input)
    {
        var rucksacks = input.Lines().ToArray();
        var sum = 0;
        var index = 0;

        while (index < rucksacks.Length)
        {
            var group = rucksacks.Skip(index).Take(3);
            var equalPart = group.First().Intersect(group.ElementAt(1)).Intersect(group.Last()).First();

            sum += char.IsLower(equalPart) ? equalPart - 96 : equalPart - 64 + 26;
            index += 3;
        }

        return sum;
    }
}
