namespace AdventOfCode2024.Days;

public class Day10(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    public async Task Part1()
    {
        var input = (await agent.GetInputLines(10)).Select(line =>
            line.ToCharArray().Select(c => Convert.ToInt32(char.GetNumericValue(c))).ToArray()).ToArray();
        var sum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != 0) continue;

                sum += CheckTile(input, j, i).Distinct().Count();
            }
        }

        resultPrinter.Print(10, 1, sum);
    }

    public static List<Tuple<int,int>> CheckTile(int[][] field, int x, int y)
    {
        int currentValue = field[y][x];

        if (currentValue == 9)
        {
            Console.WriteLine($"found a 9 at y={y} x={x}");
            return [new Tuple<int, int>(x,y)];
        }

        List<Tuple<int,int>> pathCount = [];

        // check up down left right
        if (y > 0)
        {
            int upValue = field[y - 1][x];
            if (upValue == currentValue + 1)
            {
                pathCount.AddRange(CheckTile(field, x, y - 1));
            }
        }

        if (y < field.Length - 1)
        {
            int downValue = field[y + 1][x];
            if (downValue == currentValue + 1)
            {
                pathCount.AddRange(CheckTile(field, x, y + 1));
            }
        }

        if (x > 0)
        {
            int leftValue = field[y][x - 1];
            if (leftValue == currentValue + 1)
            {
                pathCount.AddRange(CheckTile(field, x - 1, y));
            }
        }

        if (x < field[y].Length - 1)
        {
            int rightValue = field[y][x + 1];
            if (rightValue == currentValue + 1)
            {
                pathCount.AddRange(CheckTile(field, x + 1, y));
            }
        }

        return pathCount;
    }

    public async Task Part2()
    {
        var input = (await agent.GetInputLines(10)).Select(line =>
            line.ToCharArray().Select(c => Convert.ToInt32(char.GetNumericValue(c))).ToArray()).ToArray();
        var sum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != 0) continue;

                sum += CheckTile(input, j, i).Count;
            }
        }

        resultPrinter.Print(10, 2, sum);

    }
}