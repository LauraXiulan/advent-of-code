using System.Collections;

namespace AdventOfCode.App;

public class Exercises : IEnumerable<Exercise>
{
    private readonly List<Exercise> _items= new();
    public int Count => _items.Count;

    public static Exercises Load(IEnumerable<string> choices)
    {
        return new();
    }

    public IEnumerator<Exercise> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
