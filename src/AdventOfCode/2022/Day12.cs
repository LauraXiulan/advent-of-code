using System.Drawing;
using System.Formats.Asn1;
using System.Numerics;

namespace AdventOfCode._2022;

public class Day12 : Day<int, int>
{
    public override string Example => @"
Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

    [Test(ExpectedResult = 31)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 472)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 29)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 465)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var area = Area.Parse(input, 'S');
        return area.Distance(area.Start.First(), area.End);
    }

    private static int Two(string input)
    {
        var area = Area.Parse(input, 'a');
        return area.Start.Select(s => area.Distance(s, area.End)).Min();
    }

    record Area(char[][] Map, HashSet<Point> Start, Point End)
    {
        public int Distance(Point start, Point end)
        {
            var done = new HashSet<Point> { start };
            var queue = new Queue<Point>();
            queue.Enqueue(start);

            var distance = 0;

            while (queue.Any())
            {
                var total = queue.Count;
                for (var i = 0; i < total; i++)
                {
                    var curr = queue.Dequeue();
                    if (curr == end) return distance;

                    var height = Height(curr);
                    foreach (var n in Neighbors(curr, done))
                    {
                        if (Height(n) - height <= 1 && done.Add(n))
                        {
                            queue.Enqueue(n);
                        }
                    }
                }
                distance++;
            }
            return int.MaxValue;
        }

        public char Height(Point point) => Map[point.X][point.Y];

        public IEnumerable<Point> Neighbors(Point point, HashSet<Point> done)
        {
            var xCoordinates = new[] { 0, 0, 1, -1 };
            var yCoordinates = xCoordinates.Reverse().ToArray();

            for (int i = 0; i < xCoordinates.Length; i++)
            {
                var nPoint = new Point(point.X + xCoordinates[i], point.Y + yCoordinates[i]);
                if (WithinBounds(nPoint) && !Visited(nPoint, done))
                {
                    yield return nPoint;
                }
            }
        }
        public static Area Parse(string input, char startChar)
        {
            Point end = new(0, 0);
            HashSet<Point> start = new();

            var lines = input.Lines(l => l.ToCharArray()).ToArray();

            for (var r = 0; r < lines.Length; r++)
            {
                for (var c = 0; c < lines[0].Length; c++)
                {
                    if (lines[r][c] == startChar)
                    {
                        start.Add(new Point(r, c));
                        lines[r][c] = 'a';
                    }
                    else if (lines[r][c] == 'E')
                    {
                        end = new Point(r, c);
                        lines[r][c] = 'z';
                    }
                }
            }
            return new(lines, start, end);
        }

        private bool WithinBounds(Point point)
            => point.X >= 0
            && point.Y >= 0
            && point.X < Map.Length
            && point.Y < Map[0].Length;

        private static bool Visited(Point point, HashSet<Point> done) => done.Contains(point);
    }
}
