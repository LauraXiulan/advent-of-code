using System;
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

    [Test(ExpectedResult = 1602881844347)]
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
        var rocks = Rocks.GroupedLines().Select(Rock.Parse).ToArray();
        var total = 1000000000000L;
        var typeOfRock = 0;
        var rockCount = 0L;
        var height = 0;
        var jet = 0;
        var states = new Dictionary<State, (long, long)>();
        var cycleFound = false;
        var occupied = new HashSet<Point>();
        var columns = new Dictionary<int, HashSet<int>>
        {
            { 0, new() },
            { 1, new() },
            { 2, new() },
            { 3, new() },
            { 4, new() },
            { 5, new() },
            { 6, new() }
        };
        long heightIncrease = 0;

        Rock Next(int rock, int y) => new(rocks[rock].Position(start.X, y));
        bool OnFloor(Rock current) => current.Points.Any(p => p.Y == 0);
        int HighestRock() => columns.SelectMany(p => p.Value).Max() + 1;

        while (rockCount < total)
        {
            typeOfRock = (int)(rockCount % 5);
            var rock = Next(typeOfRock, height + start.Y);
            var rested = false;

            while (!rested)
            {
                jet %= input.Length;
                rock = Rock.Move(input[jet], rock, occupied);
                var dropped = new Rock(rock.Position(0, -1));
                jet++;
                if (OnFloor(rock) || dropped.Points.Any(occupied.Contains))
                {
                    rested = true;
                    break;
                }
                rock = dropped;
            }

            occupied = SetOccupied(occupied, rock);
            columns = UpdateCols(columns, rock);
            height = HighestRock();

            if (!cycleFound)
            {
                var maxCols = new List<long>();
                foreach (var col in columns)
                {
                    if (columns[col.Key].Any())
                    {
                        maxCols.Add(columns[col.Key].Max());
                    }
                    else
                    {
                        maxCols.Add(-1);
                    }
                }

                var minCol = maxCols.Min();
                var relative = new List<long>();
                foreach (var item in maxCols)
                {
                    relative.Add(item - minCol);
                }

                var state = new State(relative, typeOfRock, jet);
                if (states.Keys.Any(state.Equals))
                {
                    cycleFound = true;
                    var rocksPerCycle = rockCount - states[states.Keys.First(state.Equals)].Item1;
                    var heightsPerCycle = height - states[states.Keys.First(state.Equals)].Item2;
                    var remainingRocks = total - rockCount;
                    var cyclesRemaining = Math.Floor((double)(remainingRocks / rocksPerCycle));
                    var rockRemainder = remainingRocks % rocksPerCycle;

                    heightIncrease = (long)(heightsPerCycle * cyclesRemaining);
                    rockCount = total - rockRemainder;
                }
                else
                {
                    states[state] = (rockCount, height);
                }
            }

            rockCount++;
        }

        return height + heightIncrease;
    }

    private static Dictionary<int, HashSet<int>> UpdateCols(Dictionary<int, HashSet<int>> cols, Rock rock)
    {
        foreach (var point in rock.Points)
        {
            cols[point.X].Add(point.Y);
        }
        return cols;
    }

    class State
    {
        public State(List<long> relative, int rock, int jet)
        {
            Relative = relative;
            Rock = rock;
            Jet = jet;
        }

        public List<long> Relative { get; }
        public int Rock { get; }
        public int Jet { get; }

        public override bool Equals(object? obj)
        {
            if (obj is State { } state)
            {
                return state.Rock == Rock
                    && state.Jet == Jet
                    && Enumerable.SequenceEqual(state.Relative, Relative);
            }
            return false;
        }

        public override int GetHashCode() => Relative.GetHashCode();
    }

    private static HashSet<Point> SetOccupied(HashSet<Point> occupied, Rock current)
    {
        foreach (var p in current.Points)
        {
            occupied.Add(p);
        }
        return occupied;
    }

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
