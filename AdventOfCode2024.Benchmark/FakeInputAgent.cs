using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2024.Benchmark;

public class FakeInputAgent(string input): IInputAgent
{
    public Task<string> GetInput(int day)
    {
        return Task.FromResult(input);
    }

    public Task<List<string>> GetInputLines(int day)
    {
        return Task.FromResult(input[..^1].Split("\n").ToList());
    }
}