// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
var yearOptions = new List<string>
{
    "2022"
};

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
            "enter to accept." +
            "Choosing a group will select all options.")
        .AddChoiceGroup("Year", yearOptions)
        .AddChoiceGroup("Day", dayOptions)
        .AddChoiceGroup("Part", partOptions));

foreach (string test in tests)
{
    AnsiConsole.WriteLine(test);
}

//// Store key info in here
//ConsoleKeyInfo keyinfo;
//do
//{
//    keyinfo = Console.ReadKey();

//    // Handle each key input (down arrow will write the menu again with a different selected item)
//    if (keyinfo.Key == ConsoleKey.DownArrow)
//    {
//        if (index + 1 < options.Count)
//        {
//            index++;
//            WriteMenu(options, options[index]);
//        }
//    }
//    if (keyinfo.Key == ConsoleKey.UpArrow)
//    {
//        if (index - 1 >= 0)
//        {
//            index--;
//            WriteMenu(options, options[index]);
//        }
//    }
//    // Handle different action for the option
//    if (keyinfo.Key == ConsoleKey.Enter)
//    {
//        options[index].Selected.Invoke();
//        index = 0;
//    }
//}
//while (keyinfo.Key != ConsoleKey.X);

//Console.ReadKey();