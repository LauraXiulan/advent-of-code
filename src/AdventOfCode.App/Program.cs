// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var dir = Path.Combine(Directory.GetCurrentDirectory() + "../../../../../AdventOfCode");
var directories = Directory.EnumerateDirectories(dir);
var years = directories
    .Select(d => d[(dir.Length + 1)..])
    .Where(d => Regex.IsMatch(d, Year().ToString()));

var dayOptions = new List<string>
{
    "Day01",
    "Day02"
};

var partOptions = new List<string>
{
    "Part01",
    "Part02",
};

var tests = AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
        .Title("Which exercises do you want to run?")
        .Required()
        .InstructionsText(
            "Press space to toggle the options, " +
            "enter to accept. " +
            "Choosing a group will select all options.")
        .AddChoiceGroup("Year", years)
        .AddChoiceGroup("Day", dayOptions)
        .AddChoiceGroup("Part", partOptions));

foreach (string test in tests)
{
    AnsiConsole.WriteLine(test);
}

partial class Program
{
    [GeneratedRegex("^([0-9]{4})$", RegexOptions.Compiled)]
    private static partial Regex Year();
}