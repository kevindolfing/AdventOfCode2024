namespace AdventOfCode2024;

public class ResultPrinter : IResultPrinter
{
    public void Print(int day, int part, object result)
    {
        Console.WriteLine($"Day {day} Part {part}: {result}");
    }
}