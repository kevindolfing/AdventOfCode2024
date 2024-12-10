using System.Text;

namespace AdventOfCode2024.Days;

public class Day6(InputAgent agent) : IDay
{
    public async Task Part1()
    {
        var input = await agent.GetInputLines(6);

        Map map = BuildMapFromInput(input);

        SimulateGuard(map);

        var visitedCount = map.Tiles.Sum(r => r.Count(t => t.Visited));

        ResultPrinter.Print(6, 1, visitedCount);
    }

    private static void SimulateGuard(Map map)
    {
        var position = map.Position;
        var mapList = map.Tiles;

        bool finished = false;
        var currentDirection = Direction.Up;

        //mark position as visited
        mapList[position.Y][position.X].Visited = true;

        while (!finished)
        {
            Tuple<int, int> nextSquare = currentDirection switch
            {
                Direction.Up => new Tuple<int, int>(position.X, position.Y - 1),
                Direction.Right => new Tuple<int, int>(position.X + 1, position.Y),
                Direction.Down => new Tuple<int, int>(position.X, position.Y + 1),
                Direction.Left => new Tuple<int, int>(position.X - 1, position.Y),
                _ => throw new Exception("Invalid direction")
            };

            if (nextSquare.Item1 < 0 || nextSquare.Item1 >= mapList[0].Count || nextSquare.Item2 < 0 ||
                nextSquare.Item2 >= mapList.Count)
            {
                finished = true;
                continue;
            }

            var square = mapList[nextSquare.Item2][nextSquare.Item1];

            if (square.IsObstacle)
            {
                //turn right
                currentDirection = currentDirection switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    _ => throw new Exception("Invalid direction")
                };
            }
            else
            {
                square.Visited = true;
                position.X = nextSquare.Item1;
                position.Y = nextSquare.Item2;
            }
        }
    }

    private static Map BuildMapFromInput(List<string> input)
    {
        List<List<Tile>> mapList = new List<List<Tile>>();
        Tile? position = null;
        for (var i = 0; i < input.Count; i++)
        {
            string line = input[i];

            List<Tile> row = new List<Tile>();
            for (var j = 0; j < line.Length; j++)
            {
                char c = line[j];

                TileType type;
                switch (c)
                {
                    // ^ is the current position of the guard. It will go up
                    // # Is an obstacle
                    // . Is an empty spot
                    case '#':
                        type = TileType.Obstacle;
                        break;
                    case '^':
                        position = new Tile { X = j, Y = i };
                        type = TileType.Free;
                        break;
                    case '.':
                        type = TileType.Free;
                        break;
                    default:
                        throw new Exception("Invalid tile type");
                }

                row.Add(new Tile { TileType = type, X = j, Y = i });
            }

            mapList.Add(row);
        }

        if (position is null)
        {
            throw new Exception("StartingPosition is null");
        }

        var Map = new Map(position, mapList);
        return Map;
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    class Tile
    {
        public TileType TileType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Visited { get; set; }

        public List<Direction> HitDirections { get; init; } = [];

        public bool IsObstacle => TileType == TileType.Obstacle;
    }

    enum TileType
    {
        Free,
        Obstacle
    }

    record Map(Tile Position, List<List<Tile>> Tiles);

    public async Task Part2()
    {
        var input = await agent.GetInputLines(6);

        Map map = BuildMapFromInput(input);
        var originalStartingPosition = new Tile { X = map.Position.X, Y = map.Position.Y };
        SimulateGuard(map);

        //Only consider visited tiles that are not the starting position for putting an obstacle, since putting an obstable in an unvisited position will result in the guard not hitting it.
        //This reduces the permutations from 16900 down to 5100
        List<Tile> considerPositions = map.Tiles.SelectMany(row => row.Where(tile =>
            tile.Visited && (tile.X != originalStartingPosition.X || tile.Y != originalStartingPosition.Y))).ToList();

        Console.WriteLine($"We've got {considerPositions.Count} positions to check");
        
        int sum = 0;
        
        foreach (Tile considerPosition in considerPositions)
        {
            Map testMap = BuildMapFromInput(input);
            testMap.Tiles[considerPosition.Y][considerPosition.X].TileType = TileType.Obstacle;

            if (GuardIsLooping(testMap))
            {
                sum++;
            }
        }

        ResultPrinter.Print(6, 2, sum);
    }

    private static bool GuardIsLooping(Map map)
    {
        // OK so we know that if an obstacle is hit twice from the same direction, it's a loop
        var position = map.Position;
        var mapList = map.Tiles;

        bool finished = false;
        var currentDirection = Direction.Up;

        //mark position as visited
        mapList[position.Y][position.X].Visited = true;

        while (!finished)
        {
            Tuple<int, int> nextSquare = currentDirection switch
            {
                Direction.Up => new Tuple<int, int>(position.X, position.Y - 1),
                Direction.Right => new Tuple<int, int>(position.X + 1, position.Y),
                Direction.Down => new Tuple<int, int>(position.X, position.Y + 1),
                Direction.Left => new Tuple<int, int>(position.X - 1, position.Y),
                _ => throw new Exception("Invalid direction")
            };

            if (nextSquare.Item1 < 0 || nextSquare.Item1 >= mapList[0].Count || nextSquare.Item2 < 0 ||
                nextSquare.Item2 >= mapList.Count)
            {
                finished = true;
                continue;
            }

            var square = mapList[nextSquare.Item2][nextSquare.Item1];

            if (square.IsObstacle)
            {
                if (square.HitDirections.Contains(currentDirection))
                {
                    return true;
                }

                square.HitDirections.Add(currentDirection);

                //turn right
                currentDirection = currentDirection switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    _ => throw new Exception("Invalid direction")
                };
            }
            else
            {
                square.Visited = true;
                position.X = nextSquare.Item1;
                position.Y = nextSquare.Item2;
            }
        }

        return false;
    }
}