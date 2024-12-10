namespace AdventOfCode2024.Days;

public class Day9(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    public async Task Part1()
    {
        var input = (await agent.GetInput(9)).TrimEnd().Select(it => int.Parse(it.ToString())).ToList();

        List<int?> fileSystemList = [];

        var index = 0;
        for (int i = 0; i < input.Count; i++)
        {
            var inputValue = input[i];

            for (int j = 0; j < inputValue; j++)
            {
                fileSystemList.Add(i % 2 == 0 ? index : null);
            }

            if (i % 2 == 0) index++;
        }

        int?[] fileSystem = fileSystemList.ToArray();

        var filledTo = 0;
        for (int i = fileSystem.Length - 1; i >= 0; i--)
        {
            int? value = fileSystem[i];

            for (; filledTo < i; filledTo++)
            {
                int? check = fileSystem[filledTo];

                if (check != null) continue;

                fileSystem[filledTo] = value;
                fileSystem[i] = null;
                break;
            }
        }

        long sum = 0;
        for (int i = 0; i < fileSystem.Length; i++)
        {
            if (fileSystem[i] == null) break;

            sum += i * fileSystem[i].Value;
        }

        resultPrinter.Print(9, 1, sum);
    }

    public async Task Part2()
    {
        var input = (await agent.GetInput(9)).TrimEnd().Select(it => int.Parse(it.ToString())).ToList();

        List<int?> fileSystemList = [];

        var index = 0;
        for (int i = 0; i < input.Count; i++)
        {
            var inputValue = input[i];

            for (int j = 0; j < inputValue; j++)
            {
                fileSystemList.Add(i % 2 == 0 ? index : null);
            }

            if (i % 2 == 0) index++;
        }

        int?[] fileSystem = fileSystemList.ToArray();

        for (int i = fileSystem.Length - 1; i >= 0; i--)
        {
            int? value = fileSystem[i];
            if (value == null) continue;

            int i2 = i;
            for (; i2 > 0; i2--)
            {
                if (fileSystem[i2] != value)
                {
                    i2++;
                    break;
                }
            }

            var blockLength = i - i2 + 1;


            for (int j = 0; j < i; j++)
            {
                if (fileSystem[j] != null) continue;
                int j2 = j;
                for (; j2 < i2; j2++)
                {
                    if (fileSystem[j2] != null)
                    {
                        j2--;
                        break;
                    }
                }

                var bl = j2 - j + 1;

                if (bl >= blockLength)
                {
                    for (int k = 0; k < blockLength; k++)
                    {
                        fileSystem[j + k] = fileSystem[i - blockLength + 1 + k];
                        fileSystem[i - blockLength + 1 + k] = null;
                    }

                    break;
                }
            }

            i -= blockLength - 1;
        }

        long sum = 0;
        for (int i = 0; i < fileSystem.Length; i++)
        {
            if (fileSystem[i] == null) continue;

            sum += i * fileSystem[i].Value;
        }

        resultPrinter.Print(9, 2, sum);
    }
}  