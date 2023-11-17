namespace AdventOfCode._2022;

public class Day05 : Day<string, string>
{
    public override string Example => @"
    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    [Test(ExpectedResult = "CMZ")]
    public string One_Example() => One(Example);

    [Test(ExpectedResult = "MQSHJMWNH")]
    public override string One() => One(Input);

    [Test(ExpectedResult = "MCD")]
    public string Two_Example() => Two(Example);

    [Test(ExpectedResult = "LLWJRBHVZ")]
    public override string Two() => Two(Input);

    private static string One(string input)
    {
        var stacks = new List<Stack<char>>()
        {
            new Stack<char>(new List<char> { 'B', 'W', 'N' }),
            new Stack<char>(new List<char> { 'L', 'Z', 'S', 'P', 'T', 'D', 'M', 'B' }),
            new Stack<char>(new List<char> { 'Q', 'H', 'Z', 'W', 'R' }),
            new Stack<char>(new List<char> { 'W', 'D', 'V', 'J', 'Z', 'R' }),
            new Stack<char>(new List<char> { 'S', 'H', 'M', 'B' }),
            new Stack<char>(new List<char> { 'L', 'G', 'N', 'J', 'H', 'V', 'P', 'B' }),
            new Stack<char>(new List<char> { 'J', 'Q', 'Z', 'F', 'H', 'D', 'L', 'S' }),
            new Stack<char>(new List<char> { 'W', 'S', 'F', 'J', 'G', 'Q', 'B' }),
            new Stack<char>(new List<char> { 'Z', 'W', 'M', 'S', 'C', 'D', 'J' }),
        }.ToArray();

        //var stacks = new List<Stack<char>>()
        //{
        //    new Stack<char>(new List<char> { 'Z', 'N' }),
        //    new Stack<char>(new List<char> { 'M', 'C', 'D' }),
        //    new Stack<char>(new List<char> { 'P' }),
        //}.ToArray();

        //var groupedLines = input.Replace("    [", "[0] [").Replace("]    ", "] [0]").GroupedLines().ToArray(); // Trims out spaces
        //var stacks = Enumerable.Range(0, groupedLines[0].Last().Int32s().Last()).Select(_ => new Stack<char>()).ToArray();
        //var lines = groupedLines[0].SkipLast(1).Reverse();

        //foreach (var line in lines)
        //{
        //    var charArray = line.ToCharArray().Where(char.IsAsciiLetterOrDigit).ToArray();

        //    for (int i = 0; i < charArray.Length; i++)
        //    {
        //        if (char.IsAsciiLetter(charArray[i]))
        //        {
        //            stacks[i].Push(charArray[i]);
        //        }
        //    }
        //}

        var groupedLines = input.GroupedLines().ToArray();

        var boxes = new Boxes(stacks);
        foreach (var instruction in groupedLines[1])
        {
            boxes.Move(instruction);
        }

        string[] topBoxes = boxes.Stacks.Select(s => s.Peek().ToString()).ToArray();
        return string.Join("", topBoxes);
    }

    private static string Two(string input)
    {
        var stacks = new List<Stack<char>>()
        {
            new Stack<char>(new List<char> { 'B', 'W', 'N' }),
            new Stack<char>(new List<char> { 'L', 'Z', 'S', 'P', 'T', 'D', 'M', 'B' }),
            new Stack<char>(new List<char> { 'Q', 'H', 'Z', 'W', 'R' }),
            new Stack<char>(new List<char> { 'W', 'D', 'V', 'J', 'Z', 'R' }),
            new Stack<char>(new List<char> { 'S', 'H', 'M', 'B' }),
            new Stack<char>(new List<char> { 'L', 'G', 'N', 'J', 'H', 'V', 'P', 'B' }),
            new Stack<char>(new List<char> { 'J', 'Q', 'Z', 'F', 'H', 'D', 'L', 'S' }),
            new Stack<char>(new List<char> { 'W', 'S', 'F', 'J', 'G', 'Q', 'B' }),
            new Stack<char>(new List<char> { 'Z', 'W', 'M', 'S', 'C', 'D', 'J' }),
        }.ToArray();

        //var stacks = new List<Stack<char>>()
        //{
        //    new Stack<char>(new List<char> { 'Z', 'N' }),
        //    new Stack<char>(new List<char> { 'M', 'C', 'D' }),
        //    new Stack<char>(new List<char> { 'P' }),
        //}.ToArray();

        var groupedLines = input.GroupedLines().ToArray();

        var boxes = new Boxes(stacks);
        foreach (var instruction in groupedLines[1])
        {
            boxes.MoveSet(instruction);
        }

        string[] topBoxes = boxes.Stacks.Select(s => s.Peek().ToString()).ToArray();
        return string.Join("", topBoxes);
    }

    public class Boxes
    {
        public Boxes(Stack<char>[] stacks)
        {
            Stacks = stacks;
        }

        public Stack<char>[] Stacks { get; internal set; }
        public void Move(string instruction)
        {
            var instr = instruction.Int32s().ToArray();
            var origin = Box(instr[1]);
            var destination = Box(instr[2]);

            for (int i = 0; i < instr[0]; i++)
            {
                var box = origin.Pop();
                destination.Push(box);
            }
        }

        public void MoveSet(string instruction)
        {
            var instr = instruction.Int32s().ToArray();
            var origin = Box(instr[1]);
            var destination = Box(instr[2]);

            if (instr[0] > 1)
            {
                var stack = new Stack<char>();
                for (int i = 0; i < instr[0]; i++)
                {
                    var box = origin.Pop();
                    stack.Push(box);
                }
                foreach (var item in stack)
                {
                    destination.Push(item);
                }
            }
            else
            {
                var box = origin.Pop();
                destination.Push(box);
            }
        }

        public Stack<char> Box(int index) => Stacks[index - 1];
    }
}
