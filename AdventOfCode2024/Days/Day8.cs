using Sprache;

namespace AdventOfCode2024.Days;

public class Day8(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    public async Task Part1()
    {
        List<string> input = await agent.GetInputLines(8);

        List<Antenna> antennae = [];

        for (int i = 0; i < input.Count; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != '.')
                {
                    antennae.Add(new Antenna()
                    {
                        X = j,
                        Y = i,
                        Frequency = input[i][j]
                    });
                }
            }
        }

        int maxX = input[0].Length;
        int maxY = input.Count;

        List<Tuple<int, int>> positions = [];
        foreach (var frequency in antennae.GroupBy(a => a.Frequency).Select(g => g.Select(c => c).ToList()))
        {
            for (int i = 0; i < frequency.Count; i++)
            {
                var fi = frequency[i];
                for (int j = 0; j < frequency.Count; j++)
                {
                    if (j == i) continue;
                    var fj = frequency[j];

                    int diffX = Math.Abs(fi.X - fj.X);
                    int diffY = Math.Abs(fi.Y - fj.Y);

                    var poss = new List<Tuple<int, int>>
                    {
                        new(-1, -1),
                        new(-1, 1),
                        new(1, -1),
                        new(1, 1),
                    };

                    foreach (Tuple<int, int> pos in poss)
                    {
                        var newX = fi.X + diffX * pos.Item1;
                        var newY = fi.Y + diffY * pos.Item2;

                        var dist1 = Math.Abs(fi.X - newX) + Math.Abs(fi.Y - newY);
                        var dist2 = Math.Abs(fj.X - newX) + Math.Abs(fj.Y - newY);
                        if (dist1 * 2 != dist2 && dist2 * 2 != dist1) continue;
                        if (newX + diffX * pos.Item1 * -2 != fj.X || newY + diffY * pos.Item2 * -2 != fj.Y) continue;
                        else if (newX < 0 || newX > maxX || newY < 0 || newY > maxY ||
                                 antennae.Any(antenna => antenna.X == newX && antenna.Y == newY)) continue;

                        positions.Add(new Tuple<int, int>(newX, newY));
                    }
                }
            }
        }

        resultPrinter.Print(8, 1, positions.Count);
    }

    private class Antenna
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Frequency { get; set; }
    }

    public async Task Part2()
    {
    }
}