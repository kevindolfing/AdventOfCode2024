namespace AdventOfCode2024;

public class InputAgent : IInputAgent
{
    private readonly HttpClient _httpClient;
    private readonly DirectoryInfo _inputDirectory;

    public InputAgent(HttpClient httpClient)
    {
        _httpClient = httpClient;

        var session = Environment.GetEnvironmentVariable("AOC_SESSION_TOKEN");
        
        if(string.IsNullOrWhiteSpace(session))
        {
            throw new Exception("AOC_SESSION_TOKEN is not set");
        }
        
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session}");
        
        _inputDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Input"));
        if (!_inputDirectory.Exists)
        {
            _inputDirectory.Create();
        }
    }

    private const int Year = 2024;
    public async Task<string> GetInput(int day)
    {
        var dayFile = new FileInfo(Path.Combine(_inputDirectory.FullName, $"day{day}.txt"));
        
        if (dayFile.Exists)
        {
            // Console.WriteLine($"Reading input for day {day} from file");
            return await File.ReadAllTextAsync(dayFile.FullName);
        }
        
        var response = await _httpClient.GetAsync($"https://adventofcode.com/{Year}/day/{day}/input");

        response.EnsureSuccessStatusCode();

        var input = await response.Content.ReadAsStringAsync();
        
        
        // Console.WriteLine($"Writing input for day {day} to file");
        
        await File.WriteAllTextAsync(dayFile.FullName, input);
        
        return input;
    }

    public async Task<List<string>> GetInputLines(int day)
    {
        var input = await GetInput(day);
        return input[..^1].Split("\n").ToList();
    }
}