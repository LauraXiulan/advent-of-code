using Newtonsoft.Json.Bson;

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

    [Test(ExpectedResult = 2508)]
    public override long Two() => Two(Input);

    private static long One(string input)
    {
        var monkeys = input.Lines(Monkey.Parse);
        var outcome = Outcome(monkeys);

        return outcome;
    }

    private static long Two(string input)
    {
        var monkeys = input.Lines(Monkey.Parse);
        var outcome = OutcomeTwo(monkeys);

        return outcome;
    }

    record Monkey(string Name, long Number, string? Expression)
    {
        public long Outcome { get; set; }
        public static Monkey Parse(string input)
        {
            var colon = input.IndexOf(':');
            var name = input[0..colon];
            if (long.TryParse(input[(colon + 1)..], out long number))
            {
                return new(name, number, null);
            }
            return new(name, 0, input[(colon + 1)..]);
        }

        public string[] MonkeysNeeded() => Expression?.Alphabetic().ToArray() ?? Array.Empty<string>();
    }

    private static long Outcome(IEnumerable<Monkey> monkeys)
    {
        var input = new Dictionary<string, long>();
        var queue = new Queue<Monkey>();

        foreach (var monkey in monkeys)
        {
            queue.Enqueue(monkey);
        }

        while (queue.Any())
        {
            var current = queue.Dequeue();
            if (current.Outcome > 0)
            {
                continue;
            }
            if (current.Number > 0)
            {
                current.Outcome = current.Number;
                input.Add(current.Name, current.Outcome);
                continue;
            }

            var monkeysNeeded = current.MonkeysNeeded();
            if (monkeysNeeded.All(m => input.TryGetValue(m, out long _)))
            {
                var outcome = Solve(current, input[monkeysNeeded.First()], input[monkeysNeeded.Last()]);
                input.Add(current.Name, outcome);
            }
            else
            {
                queue.Enqueue(current);
            }
        }

        return input["root"];
    }

    private static long GetHumnValue(Monkey monkey, IEnumerable<Monkey> monkeys)
    {
        var monkeysNeeded = monkey.MonkeysNeeded();
        var firstMonkey = monkeys.First(m => m.Name == monkeysNeeded.First());
        var secondMonkey = monkeys.First(m => m.Name == monkeysNeeded.Last());
        var first = GetValue(firstMonkey, monkeys);
        var last = GetValue(secondMonkey, monkeys);

        if (first is null)
        {
            var val = first;
        }
        else if (last is null)
        {
            var val = last;
        } else
        {
            var val = 0;
        }
        return 0;
    }

    private static Dictionary<string, long> InputValues { get; set; } = new Dictionary<string, long>();
    private static long OutcomeTwo(IEnumerable<Monkey> monkeys)
    {
        var root = monkeys.First(m => m.Name == "root");
        var monkeysNeeded = root.MonkeysNeeded();
        var firstMonkey = monkeys.First(m => m.Name == monkeysNeeded.First());
        var secondMonkey = monkeys.First(m => m.Name == monkeysNeeded.Last());
        var first = GetValue(firstMonkey, monkeys);
        var last = GetValue(secondMonkey, monkeys);
        var values = new HashSet<long> { };

        if (first is null)
        {
            var result = GetHumnValue(firstMonkey, monkeys);
            return 0;
        }
        else
        {
            var result = GetHumnValue(secondMonkey, monkeys);
            return result;
        }
        //var queue = new Queue<Monkey>();

        //foreach (var monkey in monkeys)
        //{
        //    queue.Enqueue(monkey);
        //}

        //while (queue.Any())
        //{
        //    var current = queue.Dequeue();
        //    if (current.Name == "root" && current.MonkeysNeeded().Any(m => InputValues.TryGetValue(m, out long _)))
        //    {
        //        break;
        //    }
        //    if (current.Name == "humn")
        //    {
        //        continue;
        //    }
        //    if (current.Outcome > 0)
        //    {
        //        continue;
        //    }
        //    if (current.Number > 0)
        //    {
        //        current.Outcome = current.Number;
        //        InputValues.Add(current.Name, current.Outcome);
        //        continue;
        //    }

        //    var monkeysNeeded = current.MonkeysNeeded();
        //    if (monkeysNeeded.All(m => InputValues.TryGetValue(m, out long _)))
        //    {
        //        var outcome = Solve(current, InputValues[monkeysNeeded.First()], InputValues[monkeysNeeded.Last()]);
        //        if (outcome > 0)
        //        {
        //            InputValues.Add(current.Name, outcome);
        //        }
        //        else
        //        {
        //            queue.Enqueue(current);
        //        }
        //    }
        //    else
        //    {
        //        queue.Enqueue(current);
        //    }
        //}

        //var rootMonkeys = monkeys.First(m => m.Name == "root").MonkeysNeeded();
        //var first = InputValues.TryGetValue(rootMonkeys.First(), out long val1);
        //var second = InputValues.TryGetValue(rootMonkeys.Last(), out long val2);

        //if (first)
        //{
        //    InputValues.Add(rootMonkeys.Last(), val1);
        //}
        //else
        //{
        //    InputValues.Add(rootMonkeys.First(), val2);
        //}

        //var firstMonkey = monkeys.First(m => m.MonkeysNeeded().Contains("humn"));
        //var result = GetValue(firstMonkey, monkeys);

        //TestContext.Progress.WriteLine($"Queue length is {queue.Count}");
        //while (queue.Any())
        //{
        //    var current = queue.Dequeue();
        //    TestContext.Progress.WriteLine($"Queue length is {queue.Count}");
        //    TestContext.Progress.WriteLine($"Processing {current.Name}. {input.Count} items in list of {monkeys.Count()} monkeys.");
        //    var monkeysNeeded = current.MonkeysNeeded();
        //    if (monkeysNeeded.Any(m => input.TryGetValue(m, out long _) && input.TryGetValue(current.Name, out long _)))
        //    {
        //        current.Outcome = input[current.Name];
        //        var left = input.TryGetValue(monkeysNeeded.First(), out long valLeft);
        //        var right = input.TryGetValue(monkeysNeeded.Last(), out long valRight);
        //        if (left)
        //        {
        //            var (monkey, outcome) = SolveTwo(current, monkeysNeeded, valLeft, null);
        //            input.Add(monkey, outcome);
        //        }
        //        else
        //        {
        //            var (monkey, outcome) = SolveTwo(current, monkeysNeeded, null, valRight);
        //            input.Add(monkey, outcome);
        //        }
        //    }
        //    else
        //    {
        //        queue.Enqueue(current);
        //    }
        //}
        // Do Something
        return InputValues["humn"];
    }

    private static long? GetValue(Monkey monkey, IEnumerable<Monkey> monkeys)
    {
        if (monkey.Name == "humn") return null;
        if (monkey.Number > 0) return monkey.Number;

        var neededMonkeys = monkey.MonkeysNeeded();
        var left = neededMonkeys.First();
        var right = neededMonkeys.Last();

        var leftVal = InputValues.TryGetValue(left, out long l) ? l : GetValue(monkeys.First(m => m.Name == left), monkeys);
        var rightVal = InputValues.TryGetValue(right, out long r) ? r : GetValue(monkeys.First(m => m.Name == right), monkeys);

        if (leftVal is null || rightVal is null) return null;
        var answer = Solve(monkey, leftVal.Value, rightVal.Value);
        InputValues.Add(monkey.Name, answer);
        return answer;
        //var outcome = InputValues.TryGetValue(monkey.Name, out long o) ? o : 0;

        //var solve = SolveThree(monkey, outcome, leftVal, rightVal, neededMonkeys);
        //return InputValues.TryGetValue(monkey.Name, out long ou) ? ou : null;
    }

    private static long Solve(Monkey monkey, long one, long two)
    {
        var op = monkey.Expression!.Operator().First();
        switch (op)
        {
            case "*":
                monkey.Outcome = one * two;
                break;
            case "/":
                monkey.Outcome = one / two;
                break;
            case "+":
                monkey.Outcome = one + two;
                break;
            case "-":
                monkey.Outcome = one - two;
                break;
            default:
                break;
        }
        return monkey.Outcome;
    }

    private static (string, long) SolveTwo(Monkey monkey, string[] monkeysNeeded, long? one, long? two)
    {
        var op = monkey.Expression!.Operator().First();
        switch (op)
        {
            case "*":
                if (one.HasValue)
                {
                    return (monkeysNeeded.Last(), monkey.Outcome / one.Value);
                }
                else
                {
                    return (monkeysNeeded.First(), monkey.Outcome / two.Value);
                }
            case "/":
                if (one.HasValue)
                {
                    return (monkeysNeeded.Last(), monkey.Outcome * one.Value);
                }
                else
                {
                    return (monkeysNeeded.First(), monkey.Outcome * two.Value);
                }
            case "+":
                if (one.HasValue)
                {
                    return (monkeysNeeded.Last(), monkey.Outcome - one.Value);
                }
                else
                {
                    return (monkeysNeeded.First(), monkey.Outcome - two.Value);
                }
            case "-":
                if (one.HasValue)
                {
                    return (monkeysNeeded.Last(), monkey.Outcome + one.Value);
                }
                else
                {
                    return (monkeysNeeded.First(), monkey.Outcome + two.Value);
                }
            default:
                return ("", 0);
        }
    }

    private static long? SolveThree(Monkey monkey, long outcome, long? left, long? right, string[] monkeys)
    {
        if (outcome == 0)
        {
            if (!left.HasValue && !right.HasValue)
            {
                return null;
            }
            else
            {
                return Solve(monkey, left.Value, right.Value);
            }
        }
        else
        {
            var op = monkey.Expression!.Operator().First();
            if (left.HasValue)
            {
                switch (op)
                {
                    case "*":
                        InputValues.Add(monkeys.Last(), outcome / left.Value);
                        return outcome / left.Value;
                    case "/":
                        InputValues.Add(monkeys.Last(), outcome * left.Value);
                        return outcome * left.Value;
                    case "+":
                        InputValues.Add(monkeys.Last(), outcome - left.Value);
                        return outcome - left.Value;
                    case "-":
                        InputValues.Add(monkeys.Last(), outcome + left.Value);
                        return outcome + left.Value;
                    default:
                        return null;
                }
            }
            else
            {
                switch (op)
                {
                    case "*":
                        InputValues.Add(monkeys.First(), outcome / right.Value);
                        return outcome / right.Value;
                    case "/":
                        InputValues.Add(monkeys.First(), outcome * right.Value); // iets anders
                        return outcome * right.Value;
                    case "+":
                        InputValues.Add(monkeys.First(), outcome - right.Value);
                        return outcome - right.Value;
                    case "-":
                        InputValues.Add(monkeys.First(), outcome + right.Value); // Iets anders
                        return outcome + right.Value;
                    default:
                        return null;
                }
            }
        }
    }
}
