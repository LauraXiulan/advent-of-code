using NUnit.Framework.Internal;
using System.Drawing;

namespace AdventOfCode._2022;

public class Day15 : Day<int, long>
{
    public override string Example => @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

    [Test(ExpectedResult = 26)]
    public int One_Example() => One(Example, 10);

    [Test(ExpectedResult = 5125700)]
    public override int One() => One(Input, 2000000);

    [Test(ExpectedResult = 56000011)]
    public long Two_Example() => Two(Example, 20);

    [Test(ExpectedResult = 11379394658764)]
    public override long Two() => Two(Input, 4000000);

    private static int One(string input, int row) => input.Lines(Instruction.Parse).SelectMany(line => Map(row, line)).ToHashSet().Count;

    private static IEnumerable<Point> Map(int row, Instruction line)
    {
        var lineLow = line.Sensor.X - line.Distance + int.Abs(row - line.Sensor.Y);
        var lineHigh = line.Sensor.X + line.Distance - int.Abs(row - line.Sensor.Y);

        for (int i = lineLow; i < lineHigh; i++)
        {
            yield return new Point(i, row);
        }
    }

    private static long Two(string input, int max)
    {
        var instructions = input.Lines(Instruction.Parse);
        return Answer(instructions
            .SelectMany(line => Map(line, new Range(0, max)))
            .ToHashSet()
            .First(point => instructions.All(instr => !NotABeacon(point, instr))));
    }

    private static IEnumerable<Point> Map(Instruction instruction, Range startRange)
    {
        var dist = instruction.Distance + 1;
        var rangeX = new Range(instruction.Sensor.X - dist, instruction.Sensor.X + dist);

        var yUp = instruction.Sensor.Y;
        var yDown = instruction.Sensor.Y;
        for (int x = rangeX.Start; x <= rangeX.End; x++)
        {
            if (x <= instruction.Sensor.X)
            {
                var up = new Point(x, yUp++);
                var down = new Point(x, yDown--);
                if (Valid(up, startRange)) yield return up;
                if (Valid(down, startRange)) yield return down;
            }
            else
            {
                var up = new Point(x, yUp--);
                var down = new Point(x, yDown++);
                if (Valid(up, startRange)) yield return up;
                if (Valid(down, startRange)) yield return down;
            }
        }
    }

    private static bool NotABeacon(Point point, Instruction instruction)
        => Math.Abs(point.X - instruction.Sensor.X) + Math.Abs(point.Y - instruction.Sensor.Y) <= instruction.Distance;

    private static bool Valid(Point point, Range range)
        => point.X >= range.Start && point.X <= range.End
            && point.Y >= range.Start && point.Y <= range.End;

    private static long Answer(Point point) => point.X * 4_000_000L + point.Y;

    record Instruction(Point Sensor, Point Beacon, int Distance)
    {
        public static Instruction Parse(string input)
        {
            var splitted = input.Split(": ");
            var sensor = splitted[0][10..].Int32s();
            var beacon = splitted[1][21..].Int32s();

            var s = new Point(sensor.First(), sensor.Last());
            var b = new Point(beacon.First(), beacon.Last());
            var manhattan = int.Abs(sensor.First() - beacon.First()) + int.Abs(sensor.Last() - beacon.Last());

            return new(s, b, manhattan);
        }
    }

    record Range(int Start, int End);
}
