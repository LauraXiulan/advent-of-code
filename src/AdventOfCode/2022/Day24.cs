using System.Drawing;

namespace AdventOfCode._2022;

public class Day24 : Day<long, long>
{
    public override string Example => @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#";

    [Test(ExpectedResult = 18)]
    public long One_Example() => One(Example);

    [Test(ExpectedResult = 55244)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 5031)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 2508)]
    public override long Two() => Two(Input);

    private static long One(string input)
    {
        return Move(Grid.Parse(input.Lines().ToArray()));
    }

    private static long Two(string input)
    {
        return 0;
    }

    private static long Move(Grid grid)
    {
        var queue = new Queue<Point>();
        queue.Enqueue(grid.Start);
        var count = 0;

        while (queue.Any())
        {
            count++;
            var length = queue.Count;
            for (int i = 0; i < length; i++)
            {
                var current = queue.Dequeue();
                if (current == grid.End) return count;
                Grid.Blizzards = Grid.Blizzards.Select(b => (MoveInDirection(b.Item1, b.Item2, grid.Walls), b.Item2)).ToList();
                foreach (var neighbor in grid.Neighbors(current))
                {
                    if (!grid.OutOfBounds(neighbor) && !grid.Walls.Contains(neighbor) && !Grid.Blizzards.Any(b => b.Item1 == neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return long.MaxValue;
    }

    private static Point MoveInDirection(Point current, string direction, HashSet<Point> walls)
    {
        if (direction == "R")
        {
            current.Y++;
            if (walls.Contains(current))
            {
                current.Y = 1;
            }
            return current;
        }
        else if (direction == "L")
        {
            current.Y--;
            if (walls.Contains(current))
            {
                current.Y = walls.Max(w => w.Y) - 1;
            }
            return current;
        }
        else if (direction == "U")
        {
            current.X--;
            if (walls.Contains(current))
            {
                current.X = walls.Max(w => w.X) - 1;
            }
            return current;
        }
        else
        {
            current.X++;
            if (walls.Contains(current))
            {
                current.X = 1;
            }
            return current;
        }
    }

    record Grid(HashSet<Point> Walls, Point Start, Point End)
    {
        public static List<(Point, string)> Blizzards { get; set; } = new();

        public static Grid Parse(string[] input)
        {
            var walls = new HashSet<Point>();
            for (int r = 0; r < input.Length; r++)
            {
                for (int c = 0; c < input[r].Length; c++)
                {
                    if (input[r][c] == '.') continue;
                    if (input[r][c] == '#')
                    {
                        walls.Add(new Point(r, c));
                    }
                    else
                    {
                        var direction = "R";
                        switch (input[r][c])
                        {
                            case '>':
                                direction = "R";
                                break;
                            case '<':
                                direction = "L";
                                break;
                            case '^':
                                direction = "U";
                                break;
                            case 'v':
                                direction = "D";
                                break;
                            default:
                                break;
                        }
                        Blizzards.Add((new Point(r, c), direction));
                    }
                }
            }
            return new(walls, new Point(0, input[0].IndexOf('.')), new Point(input.Length - 1, input[^1].IndexOf('.')));
        }

        public HashSet<Point> Neighbors(Point point)
        {
            var neighbors = new HashSet<Point>
            {
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X, point.Y + 1),
                point
            };

            return neighbors;
        }

        public bool OutOfBounds(Point point) => point.X < 0 || point.Y < 0 || point.X > End.X || point.Y > End.Y;
    }
}
