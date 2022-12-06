using FluentAssertions;

namespace Text_specs;

public class Int32s
{
    [TestCase("-3-", 3)]
    [TestCase("move 3 from 5 to 2", 3, 5, 2)]
    public void Takes_integers_out_of_a_string(string input, params int[] numbers)
        => input.Int32s().Should().BeEquivalentTo(numbers);

    [TestCase(@"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000", 5, 1000, 2000, 3000)]
    public void Takes_integers_out_of_a_collection(string collection, int count, params int[] numbers)
    {
        var ints = collection.GroupedLines().Select(l => l.Int32s());
        ints.Should().HaveCount(count);
        ints.First().Should().BeEquivalentTo(numbers);
    }
}

public class Lines
{
    [TestCase("199;200;208;210;200;207;240;269;260;263", 10)]
    [TestCase(@"199
200
208
210
200
207
240
269
260
263", 10)]
    public void Splits_string_into_read_only_list(string input, int count)
        => input.Lines().Should().HaveCount(count);

    [TestCase("199;200;208;210;200;207;240;269;260;263")]
    public void Executes_selector_on_splitted_string(string input)
        => input.Lines(int.Parse).Should().AllBeOfType(typeof(int));
}

public class GroupedLines
{
    [TestCase(@"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000", 5)]
    public void Splits_groups_of_strings(string input, int count)
        => input.GroupedLines().Should().HaveCount(count);

    [TestCase(@"1000
2000
3000



4000

5000
6000



7000
8000
9000

10000", 5)]
    public void Removes_empty_lines(string input, int count)
        => input.GroupedLines().Should().HaveCount(count);
}

public class SumBinaryStringArray
{
    [TestCase(2, 0, "00100", "11110", "10110")]
    [TestCase(1, 1, "00100", "11110", "10110")]
    [TestCase(3, 2, "00100", "11110", "10110")]
    public void Sums_entries_of_a_binary_string_array(int expected, int index, string one, string two, string three)
        => new List<string> { one, two, three }.ToArray().SumBinaryStringArray(index).Should().Be(expected);
}

public class BinaryStringToInt
{
    [TestCase("11110", 30)]
    [TestCase("10110", 22)]
    [TestCase("00100", 4)]
    public void Converts_binary_string_to_int(string binary, int integer)
        => binary.BinaryStringToInt().Should().Be(integer);
}

public class SplitInHalf
{
    [TestCase("MQSHJMWNHN", "MQSHJ", "MWNHN")]
    public void Splits_a_string_into_two_equal_parts(string input, string first, string second)
        => input.SplitInHalf().Should().BeEquivalentTo(new[] { first, second });
}

public class CharToIntRepresentation
{
    [TestCase("a", 1)]
    [TestCase("A", 27)]
    [TestCase("r", 18)]
    [TestCase("R", 44)]
    public void Transforms_alphabetical_characters_to_an_integer_representation(char character, int expected)
        => character.CharToIntRepresentation().Should().Be(expected);
}
