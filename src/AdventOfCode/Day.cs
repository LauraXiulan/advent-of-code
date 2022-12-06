namespace AdventOfCode;

public abstract class Day<TOne, TTwo>
{
    public Day()
    {
        var year = GetType().FullName!.Split('.')[1][1..];
        Input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), $"../../../{year}/{GetType().Name}.txt"));
    }

    public string Input { get; }
    public abstract string Example { get; }

    public abstract TOne One();
    public abstract TTwo Two();
}