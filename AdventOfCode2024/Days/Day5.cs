namespace AdventOfCode2024.Days;

public class Day5(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    public async Task Part1()
    {
        List<string> input = await agent.GetInputLines(5);

        int indexofWhiteLine = input.IndexOf(string.Empty);

        List<Tuple<int, int>> rules = input.Slice(0, indexofWhiteLine).Select(lines =>
        {
            string[] parts = lines.Split("|");
            return new Tuple<int, int>(int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();

        List<string> updatesString = input.Slice(indexofWhiteLine + 1, input.Count - indexofWhiteLine - 1);

        var sum = 0;
        foreach (var update in updatesString)
        {
            var updateParts = update.Split(',').Select(int.Parse).ToList();

            if (ValidateRulesForUpdate(rules, updateParts))
            {
                var middleItem = updateParts[updateParts.Count / 2];
                sum += middleItem;
            }
        }

        resultPrinter.Print(5, 1, sum);
    }

    private static bool ValidateRulesForUpdate(List<Tuple<int, int>> rules, List<int> updateParts)
    {
        foreach (Tuple<int, int> rule in rules)
        {
            var indexFirst = updateParts.IndexOf(rule.Item1);

            if (indexFirst == -1) continue;

            var indexSecond = updateParts.IndexOf(rule.Item2);
            if (indexSecond == -1) continue;

            if (indexFirst >= indexSecond)
            {
                return false;
            }
        }

        return true;
    }

    public async Task Part2()
    {
        List<string> input = await agent.GetInputLines(5);

        int indexofWhiteLine = input.IndexOf(string.Empty);

        List<Tuple<int, int>> rules = input.Slice(0, indexofWhiteLine).Select(lines =>
        {
            string[] parts = lines.Split("|");
            return new Tuple<int, int>(int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();

        List<string> updatesString = input.Slice(indexofWhiteLine + 1, input.Count - indexofWhiteLine - 1);

        IEnumerable<List<int>> invalidParts = updatesString
            .Select(update => update.Split(',').Select(int.Parse).ToList())
            .Where(updateParts => !ValidateRulesForUpdate(rules, updateParts))
            .ToList();

        List<List<int>> validParts = [];
        int count = 0;
        foreach (List<int> part in invalidParts)
        {
            var relevantRules = rules.Where(r => part.Contains(r.Item1) && part.Contains(r.Item2)).ToList();
            List<int> newList = [];
            
            foreach (int p in part)
            {
                for (int i = 0; i < newList.Count + 1; i++)
                {
                    List<int> tempList = new(newList);
                    tempList.Insert(i, p);
                    if (ValidateRulesForUpdate(relevantRules, tempList))
                    {
                        newList = tempList;
                        break;
                    }
                }
            }

            if (!ValidateRulesForUpdate(rules, newList)) throw new("Part is still invalid!!!");
            
            validParts.Add(newList);
        }
        
        var sum = validParts.Sum(it => it[it.Count / 2]);
        resultPrinter.Print(5, 2, sum);
    }
}