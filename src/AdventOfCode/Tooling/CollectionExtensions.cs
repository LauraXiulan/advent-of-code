namespace AdventOfCode.Tooling;

public static class CollectionExtensions
{
    public static IEnumerable<T> WithStepSize<T>(this IEnumerable<T> collection, int stepSize)
    {
        var index = 0;
        foreach (var item in collection)
        {
            if (index++ % stepSize == 0)
            {
                yield return item;
            }
        };
    }

    public static bool ContainedIn(this IEnumerable<int> collection, IEnumerable<int> other) => collection.First() >= other.First() && collection.Last() <= other.Last();
    public static bool OverlapsWith(this IEnumerable<int> collection, IEnumerable<int> other) => (collection.First() >= other.First() && collection.First() <= other.Last());
}
