using System.Drawing;

namespace AdventOfCode._2022;

public class Day17 : Day<int, long>
{
    public static string Rocks => @"####

.#.
###
.#.

..#
..#
###

#
#
#
#

##
##";

    public override string Example => ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

    [Test(ExpectedResult = 3068)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 3206)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 1514285714288)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 1)]
    public override long Two() => Two(Input);

    private static int One(string input)
    {
        var start = new Point(2, 3);
        var rock = 1;

        var rocks = Rocks.GroupedLines().Select(Rock.Parse).ToArray();
        var occupied = new HashSet<Point>();
        var queue = new Queue<Rock>();

        var index = 0;
        var i = 0;

        Rock Next(int index, int y) => new(rocks[index].Position(start.X, y));
        int HighestRock() => occupied!.Max(p => p.Y) + 1;
        bool OnFloor(Rock current) => current.Points.Any(p => p.Y == 0);
        int OutOfBounds(int index, int length) => index == length ? 0 : index;

        queue.Enqueue(Next(index, start.Y));
        var next = Next(index, start.Y);

        while (rock <= 2022)
        {
            var current = Rock.Move(input[i], queue.Dequeue(), occupied);

            var dropped = new Rock(current.Position(0, -1));
            if (OnFloor(current) || dropped.Points.Any(occupied.Contains))
            {
                occupied = SetOccupied(occupied, current);
                index = OutOfBounds(++index, rocks.Length);
                queue.Enqueue(Next(index, HighestRock() + start.Y));
                rock++;
            }
            else
            {
                queue.Enqueue(dropped);
            }

            i = OutOfBounds(++i, input.Length);
        }
        var answer = occupied.Max(p => p.Y) + 1;
        return answer;
    }

    private static long Two(string input)
    {
        var start = new Point(2, 3);
        var rock = 1L;

        var rocks = Rocks.GroupedLines().Select(Rock.Parse).ToArray();
        var occupied = new HashSet<Point>();
        var queue = new Queue<Rock>();

        var index = 0;
        var i = 0;

        Rock Next(int index, int y) => new(rocks[index].Position(start.X, y));
        int HighestRock() => occupied!.Max(p => p.Y) + 1;
        bool OnFloor(Rock current) => current.Points.Any(p => p.Y == 0);
        int OutOfBounds(int index, int length) => index == length ? 0 : index;

        queue.Enqueue(Next(index, start.Y));
        var next = Next(index, start.Y);

        while (rock <= 1000000000000)
        {
            var current = Rock.Move(input[i], queue.Dequeue(), occupied);

            var dropped = new Rock(current.Position(0, -1));
            if (OnFloor(current) || dropped.Points.Any(occupied.Contains))
            {
                occupied = SetOccupied(occupied, current);
                index = OutOfBounds(++index, rocks.Length);
                queue.Enqueue(Next(index, HighestRock() + start.Y));
                rock++;
            }
            else
            {
                queue.Enqueue(dropped);
            }

            i = OutOfBounds(++i, input.Length);
        }
        var answer = occupied.Max(p => p.Y) + 1;
        return answer;
    }

    private static HashSet<Point> SetOccupied(HashSet<Point> occupied, Rock current)
    {
        foreach (var p in current.Points)
        {
            occupied.Add(p);
        }
        return occupied;
    }
    private static IEnumerable<Point> Row(int y, HashSet<Point> occupied) => occupied.Where(p => p.Y == y);

    public record Rock(HashSet<Point> Points)
    {
        public static Rock Parse(string[] rock)
        {
            var annoyingRock = @"..#
..#
###";
            var points = new HashSet<Point>();
            for (int y = 0; y < rock.Length; y++)
            {
                string part = rock[y];
                for (int x = 0; x < part.Length; x++)
                {
                    if (part[x] == '#')
                    {
                        if (rock[0] == annoyingRock.Lines().ToArray()[0])
                        {
                            if (y == 0)
                            {
                                points.Add(new Point(x, 2));
                            }
                            else if (y == 2)
                            {
                                points.Add(new Point(x, 0));
                            }
                            else
                            {
                                points.Add(new Point(x, y));
                            }
                        }
                        else
                        {
                            points.Add(new Point(x, y));
                        }
                    }
                }
            }
            return new Rock(points);
        }

        public HashSet<Point> Position(int xAddition, int yAddition)
        {
            var set = new HashSet<Point>();
            foreach (var point in Points)
            {
                set.Add(new Point(point.X + xAddition, point.Y + yAddition));
            }
            return set;
        }

        public static Rock Move(char input, Rock current, HashSet<Point> occupied)
        {
            Func<Point, bool> condition;
            HashSet<Point> positions;

            if (input == '<')
            {
                // Go left
                condition = p => p.X >= 0 && !occupied.Contains(p);
                positions = current.Position(-1, 0);
            }
            else
            {
                // Go right
                condition = p => p.X < 7 && !occupied.Contains(p);
                positions = current.Position(1, 0);
            }

            return positions.All(condition) ? new(positions) : current;
        }
    }
}
