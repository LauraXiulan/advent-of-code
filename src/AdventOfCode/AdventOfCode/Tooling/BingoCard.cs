namespace AdventOfCode.Tooling;

public class BingoCard
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
