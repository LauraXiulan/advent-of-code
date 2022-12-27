using System.Data;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using static AdventOfCode._2022.Day22;

namespace AdventOfCode._2022;

public class Day22 : Day<long, long>
{
    public override string Example => @"        ...#    ,
        .#..    ,
        #...    ,
        ....    ,
...#.......#    ,
........#...    ,
..#....#....    ,
..........#.    ,
        ...#....,
        .....#..,
        .#......,
        ......#.,

10R5L5R10L4R5L5";

    [Test(ExpectedResult = 6032)]
    public long One_Example() => One(Example);

    [Test(ExpectedResult = 55244)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 5031)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 2508)]
    public override long Two() => Two(Input);

    private static long One(string input)
    {
        var lines = input.GroupedLines(StringSplitOptions.None).ToArray();
        var trimmed = lines[0].Select(l => l.Replace(",", "")).ToArray();
        return Move(new Grid(lines[1][0], trimmed));
    }

    private static long Move(Grid grid)
    {
        var dict = new Dictionary<string, int> { { "R", 0 }, { "D", 1 }, { "L", 2 }, { "U", 3 } };
        var index = 0;
        var start = new Point(0, grid.Row(0).IndexOf(grid.Row(0).First(c => c == '.')));
        var facing = "R";
        var queue = new Queue<Point>();
        queue.Enqueue(start);

        while (index < grid.Movements.Length)
        {
            var movement = grid.Movements[index];

            while (movement > 0)
            {
                var current = queue.Dequeue();
                Point point;
                if (facing == "R") (point, facing) = grid.MoveRight(current, current);
                else if (facing == "L") (point, facing) = grid.MoveLeft(current, current);
                else if (facing == "U") (point, facing) = grid.MoveUp(current, current);
                else (point, facing) = grid.MoveDown(current, current);

                queue.Enqueue(point);
                if (point == current) break;
                movement--;
            }

            if (index < grid.Turns.Length) facing = DetermineFacing(grid.Turns[index], facing, dict);
            index++;
        }

        var position = queue.Dequeue();
        return 1000 * (position.X + 1) + 4 * (position.Y + 1) + dict[facing];
    }

    private static string DetermineFacing(string facing, string current, Dictionary<string, int> dict)
    {
        if (facing == "R")
        {
            var newVal = dict[current] + 1;
            var entry = dict.FirstOrDefault(x => x.Value == newVal);
            if (entry.Key is null) return "R";
            return entry.Key;
        }
        else
        {
            var newVal = dict[current] - 1;
            var entry = dict.FirstOrDefault(x => x.Value == newVal);
            if (entry.Key is null) return "U";
            return entry.Key;
        }
    }

    // 107051 < ?? 144329 ?? 199192
    private static long Two(string input)
    {
        var lines = input.GroupedLines(StringSplitOptions.None).ToArray();
        var trimmed = lines[0].Select(l => l.Replace(",", "")).ToArray();
        return Move(new Grid(lines[1][0], trimmed));
    }

    // 0 - 49, 50 - 99, 100 - 149, 150 - 199


    private static (Point, string) Movement(Point point, string facing)
    {
        // 0-49, 50-99 
        if (point.X == 0 && point.Y >= 50 && point.Y <= 99 && facing == "U") // Klopt
        {
            return (new Point(point.Y + 100, 0), "R");
        }
        else if (point.X == 50 && point.Y >= 50 && point.Y <= 99 && facing == "U") // Klopt
        {
            return (new Point(point.X - 1, point.Y), "U");
        }
        else if (point.X == 100 && point.Y >= 50 && point.Y <= 99 && facing == "U") // Klopt
        {
            return (new Point(point.X - 1, point.Y), "U");
        }
        else if (point.X == 100 && point.Y >= 0 && point.Y <= 49 && facing == "U") // Klopt
        {
            return (new Point(point.Y + 50, point.X / 2), "R");
        }
        else if (point.X == 0 && point.Y >= 100 && point.Y <= 149 && facing == "U") // Klopt
        {
            return (new Point(199, point.Y - 100), "U");
        }

        else if (point.X >= 0 && point.X <= 49 && point.Y == 50 && facing == "L") // Klopt
        {
            return (new Point(point.Y + 100 - (point.X + 1), 0), "R");
        }

        else if (point.X == 49 && point.Y >= 50 && point.Y <= 99 && facing == "D") // Klopt
        {
            return (new Point(point.X + 1, point.Y), "D");
        }

        else if (point.X >= 0 && point.X <= 49 && point.Y == 99 && facing == "R") // Klopt
        {
            return (new Point(point.X, point.Y + 1), "R");
        }

        // 50 - 99, 50 - 99

        else if (point.Y == 50 && point.X >= 50 && point.X <= 99 && facing == "L") // Klopt
        {
            return (new Point(point.Y * 2, point.X - 50), "D");
        }

        else if (point.X == 99 && point.Y >= 50 && point.Y <= 99 && facing == "D") // Klopt
        {
            return (new Point(point.X + 1, point.Y), "D");
        }

        else if (point.X >= 50 && point.X <= 99 && point.Y == 99 && facing == "R") // Klopt
        {
            return (new Point(49, point.X + 50), "U");
        }

        // 100 - 149, 50 - 99

        else if (point.Y == 50 && point.X >= 100 && point.X <= 149 && facing == "L") // Klopt
        {
            return (new Point(point.X, point.Y - 1), "L");
        }

        else if (point.X == 149 && point.Y >= 50 && point.Y <= 99 && facing == "D") // Klopt
        {
            return (new Point(point.Y + 100, 49), "R");
        }

        else if (point.X >= 100 && point.X <= 149 && point.Y == 99 && facing == "R") // Klopt
        {
            var diff = point.X - 100;
            return (new Point(100 - 51 - diff, 149), "L");
        }

        // 100 - 149, 0 - 49

        else if (point.Y == 0 && point.X >= 100 && point.X <= 149 && facing == "L") // Klopt
        {
            var diff = point.X - 100;
            return (new Point(0, 100 - 1 - diff), "R");
        }

        else if (point.X == 149 && point.Y >= 0 && point.Y <= 50 && facing == "D") // Klopt
        {
            return (new Point(point.X + 1, point.Y), "D");
        }

        else if (point.X >= 100 && point.X <= 149 && point.Y == 49 && facing == "R") // Klopt
        {
            return (new Point(point.X, point.Y + 1), "R");
        }

        // 0 - 49, 100 - 149

        else if (point.Y == 100 && point.X >= 0 && point.X <= 49 && facing == "L") // Klopt
        {
            return (new Point(point.X, point.Y - 1), "L");
        }

        else if (point.X == 49 && point.Y >= 100 && point.Y <= 149 && facing == "D") // Klopt
        {
            return (new Point(point.Y - 50, 99), "L");
        }

        else if (point.X >= 0 && point.X <= 49 && point.Y == 149 && facing == "R") // Klopt
        {
            return (new Point(149 - point.X, 99), "L");
        }

        // 150 - 199, 0 - 49
        else if (point.X == 150 && point.Y >= 0 && point.Y <= 49 && facing == "U") // Klopt
        {
            return (new Point(point.X - 1, point.Y), "U");
        }

        else if (point.Y == 0 && point.X >= 150 && point.X <= 199 && facing == "L") // Klopt
        {
            return (new Point(0, point.X - 100), "D");
        }

        else if (point.X == 199 && point.Y >= 0 && point.Y <= 49 && facing == "D") // Klopt
        {
            return (new Point(0, point.Y + 100), "D");
        }

        else if (point.X >= 150 && point.X <= 199 && point.Y == 49 && facing == "R") // Klopt
        {
            return (new Point(149, point.X - 100), "U");
        }

        else return (point, facing);
    }

    public sealed record Grid
    {
        public readonly string[] Input;
        public readonly string[] Turns;
        public readonly int[] Movements;

        public Grid(string instruction, string[] input)
        {
            Input = input;
            Movements = instruction.Int32s().ToArray();
            Turns = instruction.Alphabetic().ToArray();
        }

        public string Row(int index) => Input[index];
        public string Col(int index) => string.Concat(Input.Select(s => s[index]).ToArray());

        //public Point MoveRight(Point point, Point original)
        //{
        //    var newPoint = point;
        //    if (newPoint.Y == Input[0].Length - 1) newPoint.Y = 0;
        //    else newPoint.Y += 1;

        //    if (Col(newPoint.Y)[newPoint.X] == ' ') return MoveRight(newPoint, original);
        //    else if (Col(newPoint.Y)[newPoint.X] == '#') return original;
        //    else return newPoint;
        //}

        //public Point MoveLeft(Point point, Point original)
        //{
        //    var newPoint = point;
        //    if (newPoint.Y == 0) newPoint.Y = Input[0].Length - 1;
        //    else newPoint.Y -= 1;

        //    if (Col(newPoint.Y)[newPoint.X] == ' ') return MoveLeft(newPoint, original);
        //    else if (Col(newPoint.Y)[newPoint.X] == '#') return original;
        //    else return newPoint;
        //}

        //public Point MoveUp(Point point, Point original)
        //{
        //    var newPoint = point;
        //    if (newPoint.X == 0) newPoint.X = Input.Length - 1;
        //    else newPoint.X -= 1;

        //    if (Row(newPoint.X)[newPoint.Y] == ' ') return MoveUp(newPoint, original);
        //    else if (Row(newPoint.X)[newPoint.Y] == '#') return original;
        //    else return newPoint;
        //}

        //public Point MoveDown(Point point, Point original)
        //{
        //    var newPoint = point;
        //    if (newPoint.X == Input.Length - 1) newPoint.X = 0;
        //    else newPoint.X += 1;

        //    if (Row(newPoint.X)[newPoint.Y] == ' ') return MoveDown(newPoint, original);
        //    else if (Row(newPoint.X)[newPoint.Y] == '#') return original;
        //    else return newPoint;
        //}

        public (Point, string) MoveRight(Point point, Point original)
        {
            var (newPoint, facing) = Movement(point, "R");
            if (facing != "R")
            {
                if (facing == "U") return MoveUp(newPoint, original);
                else if (facing == "D") return MoveDown(newPoint, original);
                else return MoveLeft(newPoint, original);
            }
            else newPoint.Y += 1;

            if (Col(newPoint.Y)[newPoint.X] == ' ') return MoveRight(newPoint, original);
            else if (Col(newPoint.Y)[newPoint.X] == '#') return (original, "R");
            else return (newPoint, "R");
        }

        public (Point, string) MoveLeft(Point point, Point original)
        {
            var (newPoint, facing) = Movement(point, "L");
            if (facing != "L")
            {
                if (facing == "U") return MoveUp(newPoint, original);
                else if (facing == "D") return MoveDown(newPoint, original);
                else return MoveRight(newPoint, original);
            }
            else newPoint.Y -= 1;

            if (Col(newPoint.Y)[newPoint.X] == ' ') return MoveLeft(newPoint, original);
            else if (Col(newPoint.Y)[newPoint.X] == '#') return (original, "L");
            else return (newPoint, "L");
        }

        public (Point, string) MoveUp(Point point, Point original)
        {
            var (newPoint, facing) = Movement(point, "U");
            if (facing != "U")
            {
                if (facing == "L") return MoveLeft(newPoint, original);
                else if (facing == "D") return MoveDown(newPoint, original);
                else return MoveRight(newPoint, original);
            }
            else newPoint.X -= 1;

            if (Row(newPoint.X)[newPoint.Y] == ' ') return MoveUp(newPoint, original);
            else if (Row(newPoint.X)[newPoint.Y] == '#') return (original, "U");
            else return (newPoint, "U");
        }

        public (Point, string) MoveDown(Point point, Point original)
        {
            var (newPoint, facing) = Movement(point, "D");
            if (facing != "D")
            {
                if (facing == "L") return MoveLeft(newPoint, original);
                else if (facing == "U") return MoveUp(newPoint, original);
                else return MoveRight(newPoint, original);
            }
            else newPoint.X += 1;

            if (Row(newPoint.X)[newPoint.Y] == ' ') return MoveDown(newPoint, original);
            else if (Row(newPoint.X)[newPoint.Y] == '#') return (original, "D");
            else return (newPoint, "D");
        }
    }
}
