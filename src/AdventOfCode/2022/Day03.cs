namespace AdventOfCode._2022;

public class Day03 : Day<int, int>
{
    public override string Example => "vJrwpWtwJgWrhcsFMMfFFhFp;jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL;PmmdzqPrVvPwwTWBwg;wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn;ttgJtRGJQctTZtZT;CrZsJsPPZsGzwwsLwLmpwMDw";

    [Test(ExpectedResult = 157)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 7737)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 70)]
    public int Two_example() => Two(Example);

    [Test(ExpectedResult = 2697)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => input.Lines(item =>
        {
            var splitted = item.SplitInHalf();
            var equalPart = splitted[0].Intersect(splitted[1]).First();

            return equalPart.CharToIntRepresentation();
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

            sum += equalPart.CharToIntRepresentation();
            index += 3;
        }

        return sum;
    }
}
