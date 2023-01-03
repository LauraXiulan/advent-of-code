using FluentAssertions;

namespace Collection_extension_specs;

public class With_step_size
{
    [Test]
    public void Takes_elements_with_certain_step_size()
    {
        var numbers = Enumerable.Range(0, 100);
        var steps = numbers.WithStepSize(33);
        steps.Should().BeEquivalentTo(new[] { 0, 33, 66, 99 });
    }
}

public class ContainedIn
{
    [TestCase("1-4", "0-5", true)]
    [TestCase("1-4", "1-5", true)]
    [TestCase("1-4", "2-5", false)]
    public void Checks_if_one_collection_is_completely_contained_in_another(string first, string second, bool expected)
        => first.Int32sWithoutNegatives().ContainedIn(second.Int32sWithoutNegatives()).Should().Be(expected);
}

public class OverlapsWith
{
    [TestCase("1-4", "0-5", true)]
    [TestCase("1-4", "1-5", true)]
    [TestCase("2-5", "1-4", true)]
    [TestCase("1-4", "5-8", false)]
    public void Checks_if_one_collection_is_overlapping_with_another(string first, string second, bool expected)
        => first.Int32sWithoutNegatives().OverlapsWith(second.Int32sWithoutNegatives()).Should().Be(expected);
}