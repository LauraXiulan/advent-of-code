using System.Drawing;

namespace AdventOfCode._2022;

public class Day24 : Day<int, int>
{
    public override string Example => @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#";

    [Test(ExpectedResult = 18)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 255)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 54)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 809)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var (grid, blizzards) = Grid.Parse(input.Lines().ToArray());
        return Move(grid, blizzards, grid.Start, grid.End).Item1;
    }

    private static int Two(string input)
    {
        var (grid, blizzards) = Grid.Parse(input.Lines().ToArray());
        var moveToEnd = Move(grid, blizzards, grid.Start, grid.End);
        var moveToStart = Move(grid, moveToEnd.Item2, grid.End, grid.Start);
        var moveToEndAgain = Move(grid, moveToStart.Item2, grid.Start, grid.End);
        return moveToEnd.Item1 + moveToStart.Item1 + moveToEndAgain.Item1;
    }

    private static (int, HashSet<(Point, char)>) Move(Grid grid, HashSet<(Point, char)> blizzards, Point start, Point end)
    {
        var positions = new HashSet<Point> { start };
        var minute = 0;

        while (!positions.Contains(end))
        {
            minute++;
            blizzards = blizzards.Select(b => (MoveInDirection(b.Item1, b.Item2, grid.Walls), b.Item2)).ToHashSet();
            positions = new HashSet<Point>(positions.SelectMany(p => grid.Neighbors(p, blizzards.Select(b => b.Item1))));
        }

        return (minute, blizzards);
    }

    private static Point MoveInDirection(Point current, char direction, HashSet<Point> walls)
    {
        switch (direction)
        {
            case '>':
                current.X++;
                if (walls.Contains(current))
                {
                    current.X = 1;
                }
                return current;
            case '<':
                current.X--;
                if (walls.Contains(current))
                {
                    current.X = walls.Max(w => w.X) - 1;
                }
                return current;

            case '^':
                current.Y--;
                if (walls.Contains(current))
                {
                    current.Y = walls.Max(w => w.Y) - 1;
                }
                return current;
            default:
                current.Y++;
                if (walls.Contains(current))
                {
                    current.Y = 1;
                }
                return current;
        }
    }

    record Grid(HashSet<Point> Walls, Point Start, Point End)
    {
        public static (Grid, HashSet<(Point, char)>) Parse(string[] input)
        {
            var walls = new HashSet<Point>();
            var blizzards = new HashSet<(Point, char)>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    switch (input[y][x])
                    {
                        case '.':
                            continue;
                        case '#':
                            walls.Add(new Point(x, y));
                            break;
                        default:
                            blizzards.Add((new Point(x, y), input[y][x]));
                            break;
                    }
                }
            }
            return (new(walls, new Point(input[0].IndexOf('.'), 0), new Point(input[^1].IndexOf('.'), input.Length - 1)), blizzards);
        }

        public IEnumerable<Point> Neighbors(Point point, IEnumerable<Point> blizzards)
        {
            var xCoordinates = new[] { 0, 0, 1, -1 };
            var yCoordinates = xCoordinates.Reverse().ToArray();

            if (ValidPoint(point, blizzards)) yield return point;

            for (int i = 0; i < xCoordinates.Length; i++)
            {
                var nPoint = new Point(point.X + xCoordinates[i], point.Y + yCoordinates[i]);
                if (ValidPoint(nPoint, blizzards)) yield return nPoint;
            }
        }

        private bool ValidPoint(Point point, IEnumerable<Point> blizzards) => !OutOfBounds(point) && !Walls.Contains(point) && !blizzards.Contains(point);

        public bool OutOfBounds(Point point) => point.X < Start.X || point.Y < Start.Y || point.X > End.X || point.Y > End.Y;
    }
}
