namespace AdventOfCode._2022;

public class Day21 : Day<long, long>
{
    public override string Example => @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";

    [Test(ExpectedResult = 152)]
    public long One_Example() => One(Example);

    [Test(ExpectedResult = 256997859093114)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 301)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 3952288690726)]
    public override long Two() => Two(Input);

    private static long One(string input) => Outcome(input.Lines(Monkey.Parse));

    private static long Two(string input)
    {
        var monkeys = input.Lines(Monkey.Parse);
        return GetHumnValue(monkeys.First(m => m.Name == "root"), monkeys, 0);
    }

    record Monkey(string Name, long Number, string? Expression)
    {
        public long Outcome { get; set; }
        public static Monkey Parse(string input)
        {
            var colon = input.IndexOf(':');
            var name = input[0..colon];
            return long.TryParse(input[(colon + 1)..], out long number)
                ? (new(name, number, null))
                : (new(name, 0, input[(colon + 1)..]));
        }

        public NeededMonkeys Needed(IEnumerable<Monkey> monkeys)
            => Expression is null ? NeededMonkeys.Empty : new(Expression.Alphabetic(), monkeys);

        public static Monkey Empty => new("", 0, null);
    }

    record NeededMonkeys(Monkey A, Monkey B)
    {
        public NeededMonkeys(string a, string b, IEnumerable<Monkey> monkeys)
            : this(monkeys.First(m => m.Name == a), monkeys.First(m => m.Name == b)) { }

        public NeededMonkeys(IEnumerable<string> needed, IEnumerable<Monkey> monkeys)
            : this(needed.First(), needed.Last(), monkeys) { }

        public static NeededMonkeys Empty => new(Monkey.Empty, Monkey.Empty);

        public bool IsEmpty => this == Empty;

        public string[] Names => new[] { A.Name, B.Name };

        public void Resolve(Queue<Monkey> queue, Monkey current, Dictionary<string, long> input)
        {
            if (IsEmpty) return;
            if (Names.All(m => input.TryGetValue(m, out long _)))
                input.Add(current.Name, Solve(current, input[A.Name], input[B.Name]));
            else queue.Enqueue(current);
        }
    }

    private static long Outcome(IEnumerable<Monkey> monkeys)
    {
        var input = new Dictionary<string, long>();
        var queue = new Queue<Monkey>(monkeys);

        while (queue.Any())
        {
            var current = queue.Dequeue();
            if (current.Outcome > 0) continue;
            if (current.Number > 0)
            {
                current.Outcome = current.Number;
                input.Add(current.Name, current.Outcome);
                continue;
            }

            current.Needed(monkeys).Resolve(queue, current, input);
        }

        return input["root"];
    }

    private static long GetHumnValue(Monkey monkey, IEnumerable<Monkey> monkeys, long val)
    {
        var needed = monkey.Needed(monkeys);
        var a = GetValue(needed.A, monkeys);
        var known = a
            ?? GetValue(needed.B, monkeys)
            ?? throw new ArgumentNullException(null, message: "Value from both monkeys is null.");

        var newVal = SolveUnknown(monkey, known != a, known, val);
        if (needed.Names.Contains("humn")) return newVal;
        return GetHumnValue(known == a ? needed.B : needed.A, monkeys, newVal);
    }

    private static Dictionary<string, long> InputValues { get; set; } = new();
    private static long? GetValue(Monkey monkey, IEnumerable<Monkey> monkeys)
    {
        if (monkey.Name == "humn") return null;
        if (monkey.Number > 0) return monkey.Number;
        if (InputValues.TryGetValue(monkey.Name, out long v)) return v;

        var neededMonkeys = monkey.Needed(monkeys);
        var a = InputValues.TryGetValue(neededMonkeys.A.Name, out long l) ? l : GetValue(neededMonkeys.A, monkeys);
        var b = InputValues.TryGetValue(neededMonkeys.B.Name, out long r) ? r : GetValue(neededMonkeys.B, monkeys);

        if (a is null || b is null) return null;
        var answer = Solve(monkey, a.Value, b.Value);
        InputValues.Add(monkey.Name, answer);
        return answer;
    }

    private static long Solve(Monkey monkey, long one, long two)
        => monkey.Expression!.Operator().First() switch
        {
            "*" => one * two,
            "/" => one / two,
            "+" => one + two,
            "-" => one - two,
            _ => throw new NotSupportedException(),
        };

    private static long SolveUnknown(Monkey monkey, bool aIsNull, long aOrB, long c)
        => (monkey.Name == "root" ? "=" : monkey.Expression!.Operator().First()) switch
        {
            "*" => c / aOrB,
            "/" => aIsNull ? c * aOrB : aOrB / c,
            "+" => c - aOrB,
            "-" => aIsNull ? c + aOrB : aOrB - c,
            "=" => aOrB,
            _ => throw new NotSupportedException(),
        };
}