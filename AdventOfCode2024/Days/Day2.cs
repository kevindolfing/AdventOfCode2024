﻿namespace AdventOfCode2024.Days;

public class Day2(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    /// <summary>
    /// A report is safe if:
    /// The levels are either all increasing or all decreasing.
    /// Any two adjacent levels differ by at least one and at most three.
    /// </summary>
    public async Task Part1()
    {
        var fileContents = await agent.GetInputLines(2);

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

        resultPrinter.Print(2, 1, validReportCount);
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

    public async Task Part2()
    {
        var fileContents = await agent.GetInputLines(2);

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

        resultPrinter.Print(2, 2, validReportCount);
    }

    public bool ReportIsInOrder_2(List<int> report)
    {
        if (ReportDiffsWithin1And3(report) && ReportIsInOrder(report)) return true;
        for (int i = 0; i < report.Count; i++)
        {
            var subReport = report.Slice(0, i);
            if (i != report.Count - 1) subReport.AddRange(report.Slice(i + 1, report.Count - i - 1));
            if (ReportDiffsWithin1And3(subReport) && ReportIsInOrder(subReport)) return true;
        }

        return false;
    }
}