namespace AdventOfCode._2022;

public class Day06 : Day<int, int>
{
    public override string Example => "mjqjpqmgbljsphdztnvjfqwrcgsmlb";

    [Test(ExpectedResult = 7)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1757)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 19)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 2950)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var index = 0;
        var answer = 0;
        var distinct = 4;

        while (index < input.Length)
        {
            var characters = input.Skip(index).Take(distinct);
            var list = new List<char>();

            foreach (var ch in characters)
            {
                if (!list.Contains(ch))
                {
                    list.Add(ch);
                }
            }

            if (list.Count == distinct)
            {
                answer = index + distinct;
                break;
            }
            index++;
        }

        return answer;
    }
    private static int Two(string input)
    {
        var index = 0;
        var answer = 0;
        var distinct = 14;

        while (index < input.Length)
        {
            var characters = input.Skip(index).Take(distinct);
            var list = new List<char>();

            foreach (var ch in characters)
            {
                if (!list.Contains(ch))
                {
                    list.Add(ch);
                }
            }

            if (list.Count == distinct)
            {
                answer = index + distinct;
                index = input.Length;
            }
            else
            {
                index++;
            }
        }

        return answer;
    }
}
