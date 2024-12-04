using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public partial class Day3(InputAgent inputAgent) : IDay
{
    public async Task Part1()
    {
        Regex regex = MulRegex();

        string file = await inputAgent.GetInput(3);

        var matches = regex.Matches(file);

        int result = matches.Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));

        ResultPrinter.Print(3, 1, result);
    }

    public async Task Part2()
    {
        Regex regex = MulRegex_Part2();

        string file = await inputAgent.GetInput(3);

        var matches = regex.Matches(file);

        bool enabled = true;
        long result = 0;
        foreach (Match match in matches)
        {
            switch (match.Value)
            {
                case "do()":
                    enabled = true;
                    break;
                case "don't()":
                    enabled = false;
                    break;
                case var _ when enabled:
                    result += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
                    break;
            }
        }

        ResultPrinter.Print(3, 2, result);
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"(?:(mul)\((\d+),(\d+)\))|(?:(don't\(\)))|(?:(do\(\)))")]
    private static partial Regex MulRegex_Part2();
}