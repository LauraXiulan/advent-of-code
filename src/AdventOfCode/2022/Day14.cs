using System.Linq;
using Text_specs;
using Point = System.Drawing.Point;

namespace AdventOfCode._2022;

public class Day14 : Day<int, int>
{
    public override string Example => "498,4 -> 498,6 -> 496,6;503,4 -> 502,4 -> 502,9 -> 494,9";

    [Test(ExpectedResult = 24)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1513)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 93)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 24921)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var sand = new Point(500, 0);
        var lines = input.Lines();

        var rocks = Rocks(lines);

        var queue = new Queue<Point>();
        queue.Enqueue(sand);
        var count = 0;

        while (queue.Any())
        {
            var grain = queue.Dequeue();
            var bottom = new Point(grain.X, grain.Y + 1);

            if (rocks.Contains(bottom))
            {
                var left = new Point(grain.X - 1, grain.Y + 1);
                if (rocks.Contains(left))
                {
                    var right = new Point(grain.X + 1, grain.Y + 1);
                    if (rocks.Contains(new Point(grain.X + 1, grain.Y + 1)))
                    {
                        rocks.Add(grain);
                        count++;
                        queue.Enqueue(sand);
                        continue;
                    }
                    else
                    {
                        if (OutOfBounds(right, rocks))
                        {
                            break;
                        }
                        queue.Enqueue(right);
                        continue;
                    }
                }
                else
                {
                    if (OutOfBounds(left, rocks))
                    {
                        break;
                    }
                    queue.Enqueue(left);
                    continue;
                }
            }
            else
            {
                if (OutOfBounds(bottom, rocks))
                {
                    break;
                }
                queue.Enqueue(bottom);
                continue;
            }
        }
        return count;
    }

    private static int Floor { get; set; } = 0;
    private static int Two(string input)
    {
        var sand = new Point(500, 0);
        var lines = input.Lines();

        var rocks = Rocks(lines);
        Floor = rocks.Max(r => r.Y) + 2;
        //var xMax = rocks.Max(r => r.X);
        //var xMin = rocks.Min(r => r.X);
        //var points = Enumerable.Range(xMin, xMax - xMin + 1).Select(r => new Point(r, yMax + 2));
        //foreach (var point in points)
        //{
        //    rocks.Add(point);
        //}

        var queue = new Queue<Point>();
        queue.Enqueue(sand);
        var count = 0;

        while (queue.Any())
        {
            var grain = queue.Dequeue();
            var bottom = new Point(grain.X, grain.Y + 1);

            if (rocks.Contains(new Point(500, 0)))
            {
                break;
            }

            if (IsFloor(bottom))
            {
                rocks.Add(grain);
                queue.Enqueue(sand);
                count++;
            }
            else
            {
                // Go down
                if (rocks.Contains(bottom))
                {
                    //Go left and down
                    var left = new Point(grain.X - 1, grain.Y + 1);
                    if (rocks.Contains(left))
                    {
                        //Go right and down
                        var right = new Point(grain.X + 1, grain.Y + 1);
                        if (rocks.Contains(right))
                        {
                            //Rocks everywhere
                            rocks.Add(grain);
                            queue.Enqueue(sand);
                            count++;
                        }
                        else
                        {
                            queue.Enqueue(right);
                        }
                    }
                    else
                    {
                        queue.Enqueue(left);
                    }
                }
                else
                {
                    queue.Enqueue(bottom);
                }
            }
        }
        return count;
    }

    private static HashSet<Point> Rocks(IReadOnlyCollection<string> lines)
    {
        var rocks = new HashSet<Point>();
        foreach (var line in lines)
        {
            var parsed = line.Split(" -> ");
            for (int i = 0; i < parsed.Length - 1; i++)
            {
                var index = parsed[i].IndexOf(",");
                var nextIndex = parsed[i + 1].IndexOf(",");

                var point = new Point(int.Parse(parsed[i][..index]), int.Parse(parsed[i][(index + 1)..]));
                var nextPoint = new Point(int.Parse(parsed[i + 1][..index]), int.Parse(parsed[i + 1][(index + 1)..]));

                if (nextPoint.X - point.X == 0)
                {
                    var lowest = Math.Min(point.Y, nextPoint.Y);
                    var points = Enumerable.Range(lowest, Math.Abs(nextPoint.Y - point.Y) + 1).Select(r => new Point(point.X, r));
                    foreach (var item in points)
                    {
                        rocks.Add(item);
                    }
                }
                else
                {
                    var lowest = Math.Min(point.X, nextPoint.X);
                    var points = Enumerable.Range(lowest, Math.Abs(nextPoint.X - point.X) + 1).Select(r => new Point(r, point.Y));
                    foreach (var item in points)
                    {
                        rocks.Add(item);
                    }
                }
            }
        }
        return rocks;
    }

    private static bool OutOfBounds(Point point, HashSet<Point> rocks) => point.X < rocks.Min(r => r.X) || point.Y > rocks.Max(r => r.Y);
    private static bool IsFloor(Point point) => point.Y >= Floor;
}
