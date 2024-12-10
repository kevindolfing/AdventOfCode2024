namespace AdventOfCode2024;

public interface IInputAgent
{
    Task<string> GetInput(int day);
    Task<List<string>> GetInputLines(int day);
}