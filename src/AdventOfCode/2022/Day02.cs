using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2022;

public class Day02 : Day
{
    public override string Example => @"A Y
B X
C Z";

    [Test(ExpectedResult = 15)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 14163)]
    public override long One() => One(Input);

    [Test(ExpectedResult = 12)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 12091)]
    public override long Two() => Two(Input);

    private static int One(string input)
        => input.Lines()
            .Select(s => s switch
            {
                // A = Rock, B = Paper, C = Scissors, X = Rock (1), Y = Paper (2), Z = Scissors (3)
                // Lost = 0, Draw = 3, Win = 6
                "A X" => 4,
                "A Y" => 8,
                "A Z" => 3,
                "B X" => 1,
                "B Y" => 5,
                "B Z" => 9,
                "C X" => 7,
                "C Y" => 2,
                "C Z" => 6,
                _ => 0
            }).Sum();

    private static int Two(string input)
        => input.Lines()
            .Select(s => s switch
            {
                // A = Rock (1), B = Paper (2), C = Scissors (3), X = Lose, Y = Draw, Z = Win
                // Lost = 1, Draw = 3, Win = 5
                "A X" => 3,
                "A Y" => 4,
                "A Z" => 8,
                "B X" => 1,
                "B Y" => 5,
                "B Z" => 9,
                "C X" => 2,
                "C Y" => 6,
                "C Z" => 7,
                _ => 0
            }).Sum();
}
