namespace AdventOfCode2024.Days;

public class Day1: IDay
{
    public void Part1()
    {
        string[] fileContent = File.ReadAllLines("Inputs/1-1.txt");
        List<int> diffs = [];
        List<int> left = [];
        List<int> right = [];
        foreach (string line in fileContent)
        {
            string[] parts = line.Split("   ");
            left.Add(int.Parse(parts[0]));
            right.Add(int.Parse(parts[1]));
        }
        
        left.Sort();
        right.Sort();

        if (left.Count != right.Count)
        {
            throw new Exception("Left and Right aren't the same size!");
        }

        for (var i = 0; i < left.Count; i++)
        {
            int leftItem = left[i];
            int rightItem = right[i];
            
            diffs.Add(Math.Abs(leftItem - rightItem));
        }
        
        Console.WriteLine($"Day 1 Part 1: {diffs.Sum()}");
    }

    public void Part2()
    {
        string fileContent = File.ReadAllText("Inputs/1-1.txt");
        long sum = 0;
        List<int> left = [];
        List<int> right = [];
        foreach (string line in fileContent.Split(Environment.NewLine))
        {
            string[] parts = line.Split("   ");
            left.Add(int.Parse(parts[0]));
            right.Add(int.Parse(parts[1]));
        }

        foreach (int leftItem in left)
        {
            IEnumerable<int> rightItems = right.Where(r => r == leftItem);
            sum += leftItem * rightItems.Count();
        }
        
        Console.WriteLine($"Day 1 Part 2: {sum}");
    }
    
    
}