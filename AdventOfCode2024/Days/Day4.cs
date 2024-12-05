namespace AdventOfCode2024.Days;

public class Day4(InputAgent agent) : IDay
{
    private static char[] Xmas = ['X', 'M', 'A', 'S'];

    private static readonly Direction[] directions =
    [
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, -1),
        new(0, 1),
        new(1, -1),
        new(1, 0),
        new(1, 1)
    ];

    public async Task Part1()
    {
        var content = (await agent.GetInputLines(4)).Select(l => l.ToCharArray()).ToArray();

        int xmasCount = 0;

        for (int i = 0; i < content.Length; i++)
        {
            for (int j = 0; j < content[0].Length; j++)
            {
                var c = content[i][j];

                if (c != Xmas[0])
                {
                    continue;
                }

                foreach (var direction in directions)
                {
                    for (int k = 1; k < Xmas.Length; k++)
                    {
                        int di = i + direction.r * k;
                        int dj = j + direction.c * k;

                        if (di < 0 || di >= content.Length || dj < 0 || dj >= content[0].Length)
                        {
                            break;
                        }

                        if (content[di][dj] != Xmas[k])
                        {
                            break;
                        }

                        if (k == 3)
                        {
                            xmasCount++;
                        }
                    }
                }
            }
        }

        ResultPrinter.Print(4, 1, xmasCount);
    }

    public async Task Part2()
    {
        var content = (await agent.GetInputLines(4)).Select(l => l.ToCharArray()).ToArray();

        int xmasCount = 0;

        for (int i = 0; i < content.Length; i++)
        {
            for (int j = 0; j < content[0].Length; j++)
            {
                var c = content[i][j];

                if (c != 'A')
                {
                    continue;
                }

                if (Part2_TestChar(content, i, j))
                {
                    xmasCount++;
                }
            }
        }

        ResultPrinter.Print(4, 2, xmasCount);
    }

    private static readonly Direction[][] directions_Part2 =
    {
        [new Direction(-1, -1), new Direction(1, 1)],
        [new Direction(-1, 1), new Direction(1, -1)],
    };

    private static readonly char[] EdgeChars = ['M', 'S'];

    private static bool Part2_TestChar(char[][] content, int i, int j)
    {
        foreach (Direction[] direction in directions_Part2)
        {
            int di = i + direction[0].r;
            int dj = j + direction[0].c;

            if (di < 0 || di >= content.Length || dj < 0 || dj >= content[0].Length)
            {
                return false;
            }

            char first = content[di][dj];

            if (!EdgeChars.Contains(first))
            {
                return false;
            }

            di = i + direction[1].r;
            dj = j + direction[1].c;

            if (di < 0 || di >= content.Length || dj < 0 || dj >= content[0].Length)
            {
                return false;
            }

            char second = content[di][dj];

            if (!EdgeChars.Contains(second) || second == first)
            {
                return false;
            }
        }

        return true;
    }
}

record Direction(int r, int c);