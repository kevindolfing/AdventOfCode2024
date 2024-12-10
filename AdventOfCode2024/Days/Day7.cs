using System.Diagnostics;

namespace AdventOfCode2024.Days;

public class Day7(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    public async Task Part1()
    {
        var input = await agent.GetInputLines(7);

        long totalSum = 0;
        foreach (string line in input)
        {
            totalSum += CheckLine(line);
        }

        resultPrinter.Print(7, 1, totalSum);
    }

    /// <example>
    /// 5519591: 5 519 507 83
    /// Should check for:
    /// 5 + 519 + 507 + 83
    /// 5 + 519 + 507 * 83
    /// 5 + 519 * 507 * 83
    /// 5 * 519 + 507 * 83
    /// 5 * 519 * 507 * 83
    /// 5 + 519 * 507 + 83
    /// 5 * 519 * 507 + 83
    /// 5 * 519 + 507 + 83
    /// </example>
    /// <param name="line"></param>
    private static long CheckLine(string line)
    {
        string[] parts = line.Split(": ");
        long sum = long.Parse(parts[0]);
        long[] numbers = parts[1].Split(' ').Select(long.Parse).ToArray();

        return RecursiveCheck(numbers, sum, new List<Operator>() { Operator.Add, Operator.Mult }) ? sum : 0;
    }

    enum Operator
    {
        Mult,
        Add,
        Conc
    }

    private static bool RecursiveCheck(long[] numbers, long checkSum, List<Operator> operators, long currentSum = 0,
        int index = 0)
    {

        long number = numbers[index];
        if (numbers.Length == 1) return number == checkSum;

        if (index == 0)
        {
            return RecursiveCheck(numbers, checkSum, operators, number, 1);
        }

        foreach (Operator op in operators)
        {
            var newSum = op switch
            {
                Operator.Add => currentSum + number,
                Operator.Mult => currentSum * number,
                Operator.Conc => long.Parse($"{currentSum}{number}"),
                _ => throw new Exception("Invalid operator")
            };

            if (index != numbers.Length - 1) // if it's not the last number
            {
                if (RecursiveCheck(numbers, checkSum, operators, newSum, index + 1))
                {
                    return true;
                }
            }
            else
            {
                if (newSum == checkSum)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async Task Part2()
    {
        var input = await agent.GetInputLines(7);

        long totalSum = 0;
        foreach (string line in input)
        {
            var result = CheckLine_Part2(line);
            totalSum += result;
        }

        resultPrinter.Print(7, 2, totalSum);
    }

    private static long CheckLine_Part2(string line)
    {
        string[] parts = line.Split(": ");
        long sum = long.Parse(parts[0]);
        long[] numbers = parts[1].Split(' ').Select(long.Parse).ToArray();

        return RecursiveCheck(numbers, sum, [Operator.Add, Operator.Mult, Operator.Conc]) ? sum : 0;
    }
}