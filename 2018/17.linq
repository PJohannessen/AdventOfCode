<Query Kind="Program" />

List<Measurement> measurements = new List<UserQuery.Measurement>();
int width = 0;
int height = 0;
int offsetX = 0;
Space[,] grid;
Dictionary<int, Settling> settlings = new Dictionary<int, UserQuery.Settling>();

void Main()
{
    Setup();

    Stack<Position> stack = new Stack<Position>();
    stack.Push(new Position(500 - offsetX, 1));
    while (stack.Count > 0)
    {
        var current = stack.Peek();
        Space? below = null;
        if (height > current.Y + 1) below = grid[current.X, current.Y + 1];
        if (below == Space.Sand)
        {
            grid[current.X, current.Y + 1] = Space.Water;
            stack.Push(new Position(current.X, current.Y + 1));
            continue;
        }

        if (below != Space.Water && below != null)
        {
            Space? left = null;
            if (0 <= current.X - 1) left = grid[current.X - 1, current.Y];
            if (left == Space.Sand)
            {
                grid[current.X - 1, current.Y] = Space.Water;
                stack.Push(new Position(current.X - 1, current.Y));
                continue;
            }
            else if (left == Space.Clay || left == Space.Settled)
            {
                if (settlings.ContainsKey(current.Y)) settlings[current.Y].Left = current.X;
                else settlings.Add(current.Y, new Settling { Left = current.X });
            }

            Space? right = null;
            if (width > current.X + 1) right = grid[current.X + 1, current.Y];
            if (right == Space.Sand)
            {
                grid[current.X + 1, current.Y] = Space.Water;
                stack.Push(new Position(current.X + 1, current.Y));
                continue;
            }
            else if (right == Space.Clay || right == Space.Settled)
            {
                if (settlings.ContainsKey(current.Y)) settlings[current.Y].Right = current.X;
                else settlings.Add(current.Y, new Settling { Right = current.X });
            }
        }
        stack.Pop();
        Position prev = null;
        if (stack.Count > 0) prev = stack.Peek();
        if (prev != null && prev.Y != current.Y && settlings.ContainsKey(current.Y))
        {
            var settling = settlings[current.Y];
            if (settling.Left.HasValue && settling.Right.HasValue)
            {
                for (int x = settling.Left.Value; x <= settling.Right.Value; x++)
                {
                    grid[x, current.Y] = Space.Settled;
                }
            }
            settlings.Remove(current.Y);
        }
    }

    int flowing = 0;
    int settled = 0;
    for (int y = measurements.Min(m => m.FromY); y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == Space.Water) flowing++;
            else if (grid[x, y] == Space.Settled) settled++;
        }
    }
    string.Format("1: {0}", flowing + settled).Dump();
    string.Format("2: {0}", settled).Dump();
}

public class Settling
{
    public int? Left { get; set; }
    public int? Right { get; set; }
}

enum Space
{
    Sand,
    Clay,
    Water,
    Settled
}

public class Position
{
    public Position (int x, int y) { X = x; Y = y; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class Measurement
{
    public int FromX { get; set; }
    public int ToX { get; set; }
    public int FromY { get; set; }
    public int ToY { get; set; }
}

void Print()
{
    for (int y = measurements.Min(m => m.FromY); y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == Space.Water) Console.Write('|');
            else if (grid[x, y] == Space.Settled) Console.Write('~');
            else if (grid[x, y] == Space.Clay) Console.Write('#');
            else if (grid[x, y] == Space.Sand) Console.Write('.');
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

void Setup()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("17.txt");
    foreach (var inputLine in inputLines)
    {
        var measurement = new Measurement();
        var pairs = inputLine.Split(new[] { ", " }, StringSplitOptions.None);
        if (pairs[0][0] == 'x')
        {
            measurement.FromX = int.Parse(pairs[0].Substring(2, pairs[0].Length - 2));
            measurement.ToX = measurement.FromX;
        }
        else
        {
            measurement.FromY = int.Parse(pairs[0].Substring(2, pairs[0].Length - 2));
            measurement.ToY = measurement.FromY;
        }

        if (pairs[1][0] == 'x')
        {
            var secondPairs = pairs[1].Substring(2, pairs[1].Length - 2).Split(new[] { ".." }, StringSplitOptions.None);
            int from = int.Parse(secondPairs[0]);
            int to = int.Parse(secondPairs[1]);
            measurement.FromX = from;
            measurement.ToX = to;
        }
        else
        {
            var secondPairs = pairs[1].Substring(2, pairs[1].Length - 2).Split(new[] { ".." }, StringSplitOptions.None);
            int from = int.Parse(secondPairs[0]);
            int to = int.Parse(secondPairs[1]);
            measurement.FromY = from;
            measurement.ToY = to;
        }

        measurements.Add(measurement);
    }
    
    offsetX = measurements.Min(m => m.FromX) - 10;
    width = measurements.Max(m => m.ToX) - measurements.Min(m => m.FromX) + 1 + 20;
    height = measurements.Max(m => m.ToY) + 1;
    grid = new Space[width, height];
    foreach (var measurement in measurements)
    {
        for (int y = measurement.FromY; y <= measurement.ToY; y++)
        {
            for (int x = measurement.FromX - offsetX; x <= measurement.ToX - offsetX; x++)
            {
                grid[x, y] = Space.Clay;
            }
        }
    }
    grid[500 - offsetX, 0] = Space.Water;
    grid[500 - offsetX, 1] = Space.Water;
}