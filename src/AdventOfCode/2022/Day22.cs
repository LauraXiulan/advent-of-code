using System.Data;
using System.Drawing;

namespace AdventOfCode._2022;

public class Day22 : Day<int, int>
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
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 55244)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 123149)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var lines = input.GroupedLines(StringSplitOptions.None).ToArray();
        var trimmed = lines[0].Select(l => l.Replace(",", "")).ToArray();
        var grid = new Grid(lines[1][0], trimmed);

        return Move(grid, (state) => state.Step(state.Direction).Donut(grid));
    }

    private static int Move(Grid grid, Func<State, State> offGrid)
    {
        var index = 0;
        var state = new State(grid.Start, 'R');

        while (index < grid.Movements.Length)
        {
            var movement = grid.Movements[index];

            while (movement > 0)
            {
                var newState = grid.Move(state, state, offGrid);
                if (newState == state) break;
                state = newState;
                movement--;
            }

            if (index < grid.Turns.Length)
            {
                state = state with { Direction = DetermineDirection(grid.Turns[index], state.Direction) };
            };
            index++;
        }

        return state.Answer;
    }

    private static Dictionary<char, int> Dict => new() { { 'R', 0 }, { 'D', 1 }, { 'L', 2 }, { 'U', 3 } };

    private static char DetermineDirection(char direction, char current)
    {
        if (direction == 'R')
        {
            var newVal = Dict[current] + 1;
            var entry = Dict.FirstOrDefault(x => x.Value == newVal);
            if (entry.Key == default) return 'R';
            return entry.Key;
        }
        else
        {
            var newVal = Dict[current] - 1;
            var entry = Dict.FirstOrDefault(x => x.Value == newVal);
            if (entry.Key == default) return 'U';
            return entry.Key;
        }
    }

    private static int Two(string input)
    {
        var lines = input.GroupedLines(StringSplitOptions.None).ToArray();
        var trimmed = lines[0].Select(l => l.Replace(",", "")).ToArray();
        var grid = new Grid(lines[1][0], trimmed);

        return Move(grid, Movement);
    }
    private static State Movement(State state)
    {
        var position = state.Pos;
        var direction = state.Direction;
        // 0-49, 50-99 
        if (position.Y == 0 && position.X >= 50 && position.X <= 99 && direction == 'U')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(0, position.X + 100)}, R");
            return new(0, position.X + 100, 'R');
        }

        if (position.Y >= 0 && position.Y <= 49 && position.X == 50 && direction == 'L')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(0, position.X + 100 - (position.Y + 1))}, R");
            return new(0, position.X + 100 - (position.Y + 1), 'R');
        }

        if (position.Y == 49 && position.X >= 50 && position.X <= 99 && direction == 'D')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X, position.Y + 1)}, D");
            return new(position.X, position.Y + 1, 'D');
        }

        if (position.Y >= 0 && position.Y <= 49 && position.X == 99 && direction == 'R')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X + 1, position.Y)}, R");
            return new(position.X + 1, position.Y, 'R');
        }

        // 50 - 99, 50 - 99
        if (position.Y == 50 && position.X >= 50 && position.X <= 99 && direction == 'U')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X, position.Y - 1)}, U");
            return new(position.X, position.Y - 1, 'U');
        }

        if (position.X == 50 && position.Y >= 50 && position.Y <= 99 && direction == 'L')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.Y - 50, 100)}, D");
            return new(position.Y - 50, 100, 'D');
        }

        if (position.Y == 99 && position.X >= 50 && position.X <= 99 && direction == 'D')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X, position.Y + 1)}, D");
            return new(position.X, position.Y + 1, 'D');
        }

        if (position.Y >= 50 && position.Y <= 99 && position.X == 99 && direction == 'R')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.Y + 50, 49)}, U");
            return new(position.Y + 50, 49, 'U');
        }

        // 100 - 149, 50 - 99
        if (position.Y == 100 && position.X >= 50 && position.X <= 99 && direction == 'U')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X, position.Y - 1)}, U");
            return new(position.X, position.Y - 1, 'U');
        }

        if (position.X == 50 && position.Y >= 100 && position.Y <= 149 && direction == 'L')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(49, position.Y)}, L");
            return new(49, position.Y, 'L');
        }

        if (position.Y == 149 && position.X >= 50 && position.X <= 99 && direction == 'D')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(49, position.X + 100)}, L");
            return new(49, position.X + 100, 'L');
        }

        if (position.Y >= 100 && position.Y <= 149 && position.X == 99 && direction == 'R')
        {
            var diff = position.Y - 100;
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(149, 100 - 51 - diff)}, L");
            return new(149, 100 - 51 - diff, 'L');
        }

        // 100 - 149, 0 - 49
        if (position.Y == 100 && position.X >= 0 && position.X <= 49 && direction == 'U')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(50, position.X + 50)}, R");
            return new(50, position.X + 50, 'R');
        }

        if (position.X == 0 && position.Y >= 100 && position.Y <= 149 && direction == 'L')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(50, 149 - position.Y)}, R");
            return new(50, 149 - position.Y, 'R');
        }

        if (position.Y == 149 && position.X >= 0 && position.X <= 49 && direction == 'D')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X, position.Y + 1)}, D");
            return new(position.X, position.Y + 1, 'D');
        }

        if (position.Y >= 100 && position.Y <= 149 && position.X == 49 && direction == 'R')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X + 1, position.Y)}, R");
            return new(position.X + 1, position.Y, 'R');
        }

        // 0 - 49, 100 - 149
        if (position.Y == 0 && position.X >= 100 && position.X <= 149 && direction == 'U')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X - 100, 199)}, U");
            return new(position.X - 100, 199, 'U');
        }

        if (position.X == 100 && position.Y >= 0 && position.Y <= 49 && direction == 'L')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(99, position.Y)}, L");
            return new(99, position.Y, 'L');
        }

        if (position.Y == 49 && position.X >= 100 && position.X <= 149 && direction == 'D')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(99, position.X - 50)}, L");
            return new(99, position.X - 50, 'L');
        }

        if (position.Y >= 0 && position.Y <= 49 && position.X == 149 && direction == 'R')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(99, position.X - position.Y)}, L");
            return new(99, position.X - position.Y, 'L');
        }

        // 150 - 199, 0 - 49
        if (position.Y == 150 && position.X >= 0 && position.X <= 49 && direction == 'U')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X, position.Y - 1)}, U");
            return new(position.X, position.Y - 1, 'U');
        }

        if (position.X == 0 && position.Y >= 150 && position.Y <= 199 && direction == 'L')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.Y - 100, 0)}, D");
            return new(position.Y - 100, 0, 'D');
        }

        if (position.Y == 199 && position.X >= 0 && position.X <= 49 && direction == 'D')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.X + 100, 0)}, D");
            return new(position.X + 100, 0, 'D');
        }

        if (position.Y >= 150 && position.Y <= 199 && position.X == 49 && direction == 'R')
        {
            TestContext.Progress.WriteLine($"prev: {position}, {direction}, next: {new Point(position.Y - 100, 149)}, U");
            return new(position.Y - 100, 149, 'U');
        }

        switch (direction)
        {
            case 'R':
                position.X += 1;
                break;
            case 'D':
                position.Y += 1;
                break;
            case 'L':
                position.X -= 1;
                break;
            default:
                position.Y -= 1;
                break;
        }

        return new(position, direction);
    }

    public sealed record Grid
    {
        public readonly string[] Input;
        public readonly char[] Turns;
        public readonly int[] Movements;

        public Grid(string instruction, string[] input)
        {
            Input = input;
            Movements = instruction.Int32s().ToArray();
            Turns = instruction.Alphabetic().Select(s => s[0]).ToArray();
        }

        public string Row(int index) => Input[index];

        public Point Start => new(Row(0).IndexOf(Row(0).First(c => c == '.')), 0);

        public State Move(State newState, State originalState, Func<State, State> offGrid)
        {
            newState = offGrid(newState);
            var character = Row(newState.Pos.Y)[newState.Pos.X];
            if (character == ' ') return Move(newState, originalState, offGrid);
            else if (character == '#') return originalState;
            else return newState;
        }
    }

    public record struct State(Point Pos, char Direction)
    {
        public State(int x, int y, char direction) : this(new(x, y), direction) { }

        public State Donut(Grid grid)
        {
            var x = (Pos.X + grid.Input[0].Length) % grid.Input[0].Length;
            var y = (Pos.Y + grid.Input.Length) % grid.Input.Length;

            return this with { Pos = new Point(x, y) };
        }

        public State Step(char d) => d switch
        {
            'R' => this with { Pos = new Point(Pos.X + 1, Pos.Y + 0) },
            'D' => this with { Pos = new Point(Pos.X + 0, Pos.Y + 1) },
            'L' => this with { Pos = new Point(Pos.X - 1, Pos.Y + 0) },
            'U' => this with { Pos = new Point(Pos.X + 0, Pos.Y - 1) },
            _ => this,
        };

        public int Answer => 1000 * (Pos.Y + 1) + 4 * (Pos.X + 1) + Dict[Direction];
    }
}