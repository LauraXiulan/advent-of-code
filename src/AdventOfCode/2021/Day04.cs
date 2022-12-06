namespace AdventOfCode._2021;

public class Day04 : Day<int, int>
{
    public override string Example => @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";

    [Test(ExpectedResult = 4512)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 10374)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 1924)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 24742)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var blocks = input.GroupedLines().ToArray();
        var drawn = blocks[0][0].Int32s().ToArray();

        var cards = blocks.Skip(1)
            .Select(entries => new BingoCard(entries
                .SelectMany(entries => entries.Int32s().ToArray())
                .ToArray()))
            .ToList();

        BingoCard? bingo = null;
        var index = 0;
        while (bingo is null)
        {
            foreach (var card in cards)
            {
                card.Adjust(drawn[index]);
            }
            index++;

            bingo = cards.FirstOrDefault(c => c.IsBingo);
        }
        return bingo.Score * drawn[index - 1];
    }

    private static int Two(string input)
    {
        var blocks = input.GroupedLines().ToArray();
        var drawn = blocks[0][0].Int32s().ToArray();

        var cards = blocks.Skip(1)
            .Select(entries => new BingoCard(entries
                .SelectMany(entries => entries.Int32s().ToArray())
                .ToArray()))
            .ToList();

        BingoCard? bingo = null;
        var index = 0;
        while (bingo is null)
        {
            foreach (var card in cards)
            {
                card.Adjust(drawn[index]);
            }
            index++;

            if (cards.Count > 1)
            {
                cards = cards.Where(c => !c.IsBingo).ToList();
            }

            bingo = cards.FirstOrDefault(c => c.IsBingo);
        }
        return bingo.Score * drawn[index - 1];
    }
}

public sealed record BingoCard
{
    private readonly int[] Numbers;
    public BingoCard(int[] numbers) => Numbers = numbers;

    public bool IsBingo => Enumerable.Range(0, 5).Any(index => Row(index).All(c => c == 0) || Col(index).All(c => c == 0));

    public int Score => Numbers.Sum();

    public void Adjust(int card)
    {
        var index = Array.IndexOf(Numbers, card);
        if (index != -1)
        {
            Numbers[index] = 0;
        }
    }

    public IEnumerable<int> Row(int index) => Numbers.Skip(5 * index).Take(5);
    public IEnumerable<int> Col(int index) => Numbers.Skip(index).WithStepSize(5);
}
