namespace AdventOfCode.App;

public sealed class Exercise
{
    public Exercise(string name, object answer)
    {
        Name = name;
        Answer = answer;
    }

    public string Name { get; }
    public object Answer { get; }
}
