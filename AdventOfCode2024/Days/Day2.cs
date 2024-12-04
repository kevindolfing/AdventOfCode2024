namespace AdventOfCode2024.Days;

public class Day2 : IDay
{
    /// <summary>
    /// A report is safe if:
    /// The levels are either all increasing or all decreasing.
    /// Any two adjacent levels differ by at least one and at most three.
    /// </summary>
    public void Part1()
    {
        var fileContents = File.ReadAllLines("Inputs/2-1.txt");

        List<List<int>> reports = [];
        foreach (string line in fileContents)
        {
            reports.Add(line.Split(' ').Select(int.Parse).ToList());
        }

        int validReportCount = 0;
        foreach (List<int> report in reports)
        {
            var isValid = ReportIsInOrder(report) && ReportDiffsWithin1And3(report);
            validReportCount += isValid ? 1 : 0;
        }

        Console.WriteLine($"Day 2 Part 1: {validReportCount}");
    }

    public bool ReportIsInOrder(List<int> report)
    {
        if (report.Count == 1) return true;
        if (report[0] > report[1])
        {
            for (int i = 0; i < report.Count - 1; i++)
            {
                if (report[i] <= report[i + 1])
                {
                    return false;
                }
            }
        }
        else if (report[0] < report[1])
        {
            for (int i = 0; i < report.Count - 1; i++)
            {
                if (report[i] >= report[i + 1])
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public bool ReportDiffsWithin1And3(List<int> report)
    {
        for (int i = 0; i < report.Count - 1; i++)
        {
            var diff = Math.Abs(report[i] - report[i + 1]);
            if (diff is < 1 or > 3)
            {
                return false;
            }
        }

        return true;
    }

    public void Part2()
    {
        var fileContents = File.ReadAllLines("Inputs/2-1.txt");

        List<List<int>> reports = [];
        foreach (string line in fileContents)
        {
            reports.Add(line.Split(' ').Select(int.Parse).ToList());
        }

        int validReportCount = 0;
        foreach (List<int> report in reports)
        {
            var isValid = ReportIsInOrder_2(report);
            validReportCount += isValid ? 1 : 0;
        }

        Console.WriteLine($"Day 2 Part 2: {validReportCount}");
    }

    public bool ReportIsInOrder_2(List<int> report)
    {
        if (ReportDiffsWithin1And3(report) && ReportIsInOrder(report)) return true;
        for (int i = 0; i < report.Count; i++)
        {
            var subReport = report.Slice(0, i);
            if(i != report.Count - 1) subReport.AddRange(report.Slice(i + 1, report.Count - i - 1));
            if (ReportDiffsWithin1And3(subReport) && ReportIsInOrder(subReport)) return true;
            
        }

        return false;
        // int errors = 0;
        // List<int> diffs = [];
        // for (int i = 0; i < report.Count - 1; i++)
        // {
        //     diffs.Add(report[i + 1] - report[i]);
        // }
        //
        // errors += diffs.Select(d => Math.Abs(d)).Count(d => d is < 1 or > 3);
        // if (errors == 1) errors = 0;
        // var ctz = diffs.Select(d => d > 0 ? 1 : d < 0 ? -1 : 0);
        // var mostOccurringValue = ctz.GroupBy(d => d).OrderByDescending(g => g.Key).First().First();
        // errors += ctz.Count(v => v != mostOccurringValue);
        //
        // return errors;
    }
}