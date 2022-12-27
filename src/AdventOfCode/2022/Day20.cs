namespace AdventOfCode._2022;

public class Day20 : Day<long, long>
{
    public override string Example => "1;2;-3;3;-2;0;4";

    [Test(ExpectedResult = 3)]
    public long One_Example() => Decrypt(Example, 1, 1);

    [Test(ExpectedResult = 5962)]
    public override long One() => Decrypt(Input, 1, 1);

    [Test(ExpectedResult = 1623178306)]
    public long Two_Example() => Decrypt(Example, 811589153, 10);

    [Test(ExpectedResult = 9862431387256)]
    public override long Two() => Decrypt(Input, 811589153, 10);

    private static long Decrypt(string input, long key, int times)
    {
        var linkedSeq = new LinkedList<long>(input.Lines(long.Parse).Select(i => i * key));
        var zero = Move(linkedSeq, times).Find(0)!;
        return zero.Move(1000).Value + zero.Move(2000).Value + zero.Move(3000).Value;
    }

    private static LinkedList<long> Move(LinkedList<long> input, int times)
    {
        var mixed = new LinkedList<long>(input);
        var ordered = mixed.Enumerate().Where(i => i.Value != 0).ToArray();

        while (times-- > 0)
        {
            foreach (var item in ordered)
            {
                var next = item.Move(1);
                mixed.Remove(item);
                mixed.AddBefore(next.Move(item.Value), item);
            }
        }
        return mixed;
    }
}

static class Extensions
{
    public static LinkedListNode<long> Move(this LinkedListNode<long> node, long steps)
    {
        var first = node.List!.First!;
        steps %= node.List.Count;
        steps = steps < 0 ? node.List.Count + steps : steps;
        var next = node;
        while (steps-- > 0)
        {
            next = next.Next ?? first;
        }
        return next;
    }

    public static IEnumerable<LinkedListNode<long>> Enumerate(this LinkedList<long> list)
    {
        var curr = list.First;
        while (curr is { })
        {
            yield return curr;
            curr = curr.Next;
        }
    }
}