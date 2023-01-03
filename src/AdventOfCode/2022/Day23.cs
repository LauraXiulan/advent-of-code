using System.Drawing;

namespace AdventOfCode._2022;

public class Day23 : Day<long, long>
{
    public override string Example => @"....#..
..###.#
#...#.#
.#...##
#.###..
##.#.##
.#..#..";

    [Test(ExpectedResult = 110)]
    public long One_Example() => One(Example);

    [Test(ExpectedResult = 3689)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 20)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 965)]
    public override long Two() => Two(Input);

    private static long One(string input)
    {
        var grid = new Grid(input.Lines().ToArray());
        var preferredDirection = new HashSet<(Point, Point)>();
        var round = 1;

        var ordered = new LinkedList<Func<Grid, HashSet<(Point, Point)>, Point, IEnumerable<string>, (HashSet<(Point, Point)>, bool)>>(
            new List<Func<Grid, HashSet<(Point, Point)>, Point, IEnumerable<string>, (HashSet<(Point, Point)>, bool)>>()
            {
                GoNorth, GoSouth, GoWest, GoEast
            });

        while (round <= 10)
        {
            foreach (var elf in grid.Elves)
            {
                preferredDirection = Propose(grid, preferredDirection, ordered, elf);
            }

            Move(grid, preferredDirection, false);

            ordered.AddLast(ordered.First());
            ordered.RemoveFirst();
            round++;
        }

        var minY = grid.Elves.Min(e => e.Y);
        var minX = grid.Elves.Min(e => e.X);
        var maxY = grid.Elves.Max(e => e.Y);
        var maxX = grid.Elves.Max(e => e.X);

        var answer = (1 + maxX - minX) * (1 + maxY - minY) - grid.Elves.Count;
        return answer;
    }

    private static HashSet<(Point, Point)> Propose(Grid grid, HashSet<(Point, Point)> preferredDirection, LinkedList<Func<Grid, HashSet<(Point, Point)>, Point, IEnumerable<string>, (HashSet<(Point, Point)>, bool)>> ordered, Point elf)
    {
        var directions = grid.Direction(elf);
        if (!directions.Any()) return preferredDirection;

        foreach (var direction in ordered)
        {
            var result = direction(grid, preferredDirection, elf, directions);
            if (result.Item2)
            {
                return result.Item1;
            }
        }

        return preferredDirection;
    }

    private static (HashSet<(Point, Point)>, bool) GoNorth(Grid grid, HashSet<(Point, Point)> preferred, Point elf, IEnumerable<string> directions)
    {
        if (!Grid.Directions["North"].Any(directions.Contains))
        {
            preferred.Add((elf, Grid.North(elf)));
            return (preferred, true);
        }
        return (preferred, false);
    }

    private static (HashSet<(Point, Point)>, bool) GoSouth(Grid grid, HashSet<(Point, Point)> preferred, Point elf, IEnumerable<string> directions)
    {
        if (!Grid.Directions["South"].Any(directions.Contains))
        {
            preferred.Add((elf, Grid.South(elf)));
            return (preferred, true);
        }
        return (preferred, false);
    }

    private static (HashSet<(Point, Point)>, bool) GoWest(Grid grid, HashSet<(Point, Point)> preferred, Point elf, IEnumerable<string> directions)
    {
        if (!Grid.Directions["West"].Any(directions.Contains))
        {
            preferred.Add((elf, Grid.West(elf)));
            return (preferred, true);
        }
        return (preferred, false);
    }

    private static (HashSet<(Point, Point)>, bool) GoEast(Grid grid, HashSet<(Point, Point)> preferred, Point elf, IEnumerable<string> directions)
    {
        if (!Grid.Directions["East"].Any(directions.Contains))
        {
            preferred.Add((elf, Grid.East(elf)));
            return (preferred, true);
        }
        return (preferred, false);
    }

    private static long Two(string input)
    {
        var grid = new Grid(input.Lines().ToArray());
        var preferredDirection = new HashSet<(Point, Point)>();
        var round = 1;

        var ordered = new LinkedList<Func<Grid, HashSet<(Point, Point)>, Point, IEnumerable<string>, (HashSet<(Point, Point)>, bool)>>(
            new List<Func<Grid, HashSet<(Point, Point)>, Point, IEnumerable<string>, (HashSet<(Point, Point)>, bool)>>()
            {
                GoNorth, GoSouth, GoWest, GoEast
            });

        var noElvesMoved = false;

        while (!noElvesMoved)
        {
            foreach (var elf in grid.Elves)
            {
                preferredDirection = Propose(grid, preferredDirection, ordered, elf);
            }

            var moved = false;
            moved = Move(grid, preferredDirection, moved);

            if (!moved)
            {
                noElvesMoved = true;
                break;
            }

            ordered.AddLast(ordered.First());
            ordered.RemoveFirst();
            round++;
        }

        return round;
    }

    private static bool Move(Grid grid, HashSet<(Point, Point)> preferredDirection, bool moved)
    {
        while (preferredDirection.Any())
        {
            var current = preferredDirection.First();
            if (preferredDirection.Skip(1).Any(d => d.Item2 == current.Item2))
            {
                preferredDirection.RemoveWhere(p => p.Item2 == current.Item2);
            }
            else
            {
                moved = true;
                grid.Elves.Remove(current.Item1);
                grid.Elves.Add(current.Item2);
                preferredDirection.Remove(current);
            }
        }

        return moved;
    }

    class Grid
    {
        public HashSet<Point> Elves { get; set; } = new();

        public Grid(string[] input)
        {
            var elves = new HashSet<Point>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        elves.Add(new Point(x, y));
                    }
                }
            }
            Elves = elves;
        }

        public IEnumerable<string> Direction(Point point)
        {
            foreach (var dir in All(point))
            {
                if (Elves.Contains(dir.Value))
                {
                    yield return dir.Key;
                }
            }
        }

        public Dictionary<string, Point> All(Point point) => new()
        {
            { nameof(North), North(point) },
            { nameof(East), East(point) },
            { nameof(West), West(point) },
            { nameof(South), South(point) },
            { nameof(NorthEast), NorthEast(point) },
            { nameof(NorthWest), NorthWest(point) },
            { nameof(SouthEast), SouthEast(point) },
            { nameof(SouthWest), SouthWest(point) }
        };

        public static Dictionary<string, string[]> Directions => new()
        {
            { "North", new[] {"North", "NorthEast", "NorthWest" } },
            { "South", new[] { "South", "SouthEast", "SouthWest" } },
            { "West", new[] { "West", "SouthWest", "NorthWest" } },
            { "East", new[] {"East", "SouthEast", "NorthEast" } },
        };

        public static Point North(Point point) => new(point.X, point.Y - 1);
        public static Point East(Point point) => new(point.X + 1, point.Y);
        public static Point South(Point point) => new(point.X, point.Y + 1);
        public static Point NorthWest(Point point) => new(point.X - 1, point.Y - 1);
        public static Point NorthEast(Point point) => new(point.X + 1, point.Y - 1);
        public static Point SouthWest(Point point) => new(point.X - 1, point.Y + 1);
        public static Point SouthEast(Point point) => new(point.X + 1, point.Y + 1);
        public static Point West(Point point) => new(point.X - 1, point.Y);
    }
}
