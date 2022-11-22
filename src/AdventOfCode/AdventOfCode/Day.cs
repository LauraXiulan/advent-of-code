namespace AdventOfCode;

public abstract class Day
{
    public Day()
    {
        var year = GetType().FullName!.Split('.')[1][1..];
        Input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), $"../../../{year}/{GetType().Name}.txt"));
    }

    public string Input { get; }
    public abstract string Example { get; }

    public abstract int One();
    public abstract int One_Example();
    public abstract int Two();
    public abstract int Two_Example();
}