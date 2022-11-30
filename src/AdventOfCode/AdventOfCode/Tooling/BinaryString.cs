namespace AdventOfCode.Tooling;

public static class BinaryString
{
    public static int SumCharArray(this string[] array, int index) => array.Sum(i => int.Parse(i[index].ToString()));

    public static int ToInt(this string binary) => Convert.ToInt32(binary, 2);
}
