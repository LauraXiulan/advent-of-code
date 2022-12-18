namespace AdventOfCode._2022;

public class Day11 : Day<long, long>
{
    public override string Example => @"
Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

    [Test(ExpectedResult = 10605)]
    public long One_Example() => One(Example);

    [Test(ExpectedResult = 57838)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 2713310158L)]
    public long Two_Example() => Two(Example);

    [Test(ExpectedResult = 15050382231L)]
    public override long Two() => Two(Input);

    private static long One(string input)
    {
        var round = 0;
        var lines = input.GroupedLines().ToArray();
        var monkeys = new List<Monkey>();

        foreach (var line in lines)
        {
            var monkey = new Monkey(line);
            monkeys.Add(monkey);
        }

        while (round < 20)
        {
            foreach (var monkey in monkeys)
            {
                if (monkey.Items.Any())
                {
                    monkey.Items = monkey.Items.Select(i =>
                    {
                        monkey.Inspected++;
                        var newItem = monkey.Operation(i);
                        newItem /= 3;
                        var newMonkey = monkey.Test(newItem);
                        monkeys.ElementAt(newMonkey).Items.Add(newItem);
                        return (long)0;
                    }).Where(item => item != 0).ToList();
                }
            }
            round++;
        }

        var mostActive = monkeys.Select(m => m.Inspected).OrderByDescending(s => s).Take(2).ToArray();
        return mostActive[0] * mostActive[1];
    }

    private static long Two(string input)
    {
        var round = 0;
        var lines = input.GroupedLines().ToArray();
        var monkeys = new List<Monkey>();

        foreach (var line in lines)
        {
            var monkey = new Monkey(line);
            monkeys.Add(monkey);
        }

        var productOfDivisors = monkeys.Select(m => m.Division).Aggregate(1, (acc, cur) => acc * cur);

        while (round < 10000)
        {
            foreach (var monkey in monkeys)
            {
                if (monkey.Items.Any())
                {
                    monkey.Items = monkey.Items.Select(i =>
                    {
                        monkey.Inspected++;
                        var newItem = monkey.Operation(i) % productOfDivisors;
                        monkeys.ElementAt(monkey.Test(newItem)).Items.Add(newItem);
                        return (long)0;
                    }).Where(item => item != 0).ToList();
                }
            }
            round++;
        }

        var mostActive = monkeys.Select(m => m.Inspected).OrderByDescending(s => s).Take(2).ToArray();
        return mostActive[0] * mostActive[1];
    }
}

public class Monkey
{
    public Monkey(string[] input)
    {
        Rank = input[0].Int32s().First();
        Items = input[1].ToLong().ToList();

        var operation = input[2][11..];
        var op = operation.Substring(10, 1);
        var right = operation[12..];

        Division = input[3].Int32s().First();
        Operation = (old) => Operator(old, op, (right == "old" ? old : long.Parse(right)));

        var trueCase = input[4].Int32s().First();
        var falseCase = input[5].Int32s().First();

        Test = (newItem) => newItem % Division == 0 ? trueCase : falseCase;
    }

    public int Rank { get; }
    public long Inspected { get; set; }
    public int Division { get; set; }

    public List<long> Items { get; set; }

    public Func<long, long> Operation { get; }

    public Func<long, int> Test { get; }

    private static long Operator(long old, string op, long rh)
        => op switch
        {
            "*" => old * rh,
            "+" => old + rh,
            _ => 0,
        };
}
