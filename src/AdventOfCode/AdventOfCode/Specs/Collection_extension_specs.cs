using FluentAssertions;

namespace Collection_extension_specs;

internal class With_step_size
{
    [Test]
    public void Takes_elements_with_certain_step_size()
    {
        var numbers = Enumerable.Range(0, 100);
        var steps = numbers.WithStepSize(33);
        steps.Should().BeEquivalentTo(new[] { 0, 33, 66, 99 });
    }
}
