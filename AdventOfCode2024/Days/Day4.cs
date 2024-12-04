namespace AdventOfCode2024.Days;

public class Day4(InputAgent agent) : IDay
{
    public async Task Part1()
    {
        var content = await agent.GetInput(4);
        Console.WriteLine(content);
    }

    public Task Part2()
    {
        throw new NotImplementedException();
    }
}