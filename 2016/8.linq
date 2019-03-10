<Query Kind="Program" />

const int width = 50;
const int height = 6;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("8.txt");
    
    List<Point> points = new List<Point>();
    
    foreach (var line in inputLines)
    {
        var numbers = Regex.Matches(line, @"[+-]?\d+(\.\d+)?");
        int a = int.Parse(numbers[0].Value);
        int b = int.Parse(numbers[1].Value);
        
        if (line.StartsWith("rect"))
        {
            for (int y = 0; y < b; y++)
            {
                for (int x = 0; x < a; x++)
                {
                    if (!points.Any(p => p.X == x && p.Y == y))
                    {
                        points.Add(new Point{X = x, Y = y});
                    }
                }
            }
        }
        else if (line.StartsWith("rotate column"))
        {
            foreach (var p in points.Where(p => p.X == a))
            {
                p.RotateColumn(b);
            }
        }
        else if (line.StartsWith("rotate row"))
        {
            foreach (var p in points.Where(p => p.Y == a))
            {
                p.RotateRow(b);
            }
        }
    }

    $"A: {points.Count}".Dump();
    "B:".Dump();
    Print(points);
}

class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public void RotateColumn(int n)
    {
        Y = (Y + n) % height;
    }

    public void RotateRow(int n)
    {
        X = (X + n) % width;
    }
}

void Print(List<Point> points)
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (points.Any(p => p.X == x && p.Y == y)) Console.Write('#');
            else Console.Write('.');
        }
        Console.WriteLine();
    }
}