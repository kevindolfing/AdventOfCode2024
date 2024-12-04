namespace AdventOfCode2024.Days;

public class Day1(InputAgent agent) : IDay
{
    public async Task Part1()
    {
        List<string> fileContent =  await agent.GetInputLines(1);
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

        ResultPrinter.Print(1, 1, diffs.Sum());
    }

    public async Task Part2()
    {
        List<string> fileContent = await agent.GetInputLines(1);
        long sum = 0;
        List<int> left = [];
        List<int> right = [];
        foreach (string line in fileContent)
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


        ResultPrinter.Print(1, 2, sum);
    }
}