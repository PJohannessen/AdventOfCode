using System.Text;

namespace aoc;

public class Day3
{
    public static void Process()
    {
        string[] lines = Utils.ParseStrings("3.txt");

        int maxX = lines[0].Length;
        int maxY = lines.Length;

        Dictionary<(int X, int Y), char> parts = new Dictionary<(int X, int Y), char>();
        Dictionary<List<(int X, int Y)>, int> numbers = new Dictionary<List<(int X, int Y)>, int>();
        
        for (int y = 0; y < maxY; y++)
        {
            StringBuilder sb = new StringBuilder();
            List<(int X, int Y)> coords = new List<(int X, int Y)>();
            for (int x = 0; x < maxX; x++)
            {
                if (char.IsDigit(lines[y][x]))
                {
                    sb.Append(lines[y][x]);
                    coords.Add((x, y));
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        int n = int.Parse(sb.ToString());
                        numbers.Add(coords, n);
                        sb = new StringBuilder();
                    }
                    if (lines[y][x] != '.')
                    {
                        parts.Add((x, y), lines[y][x]);
                    }
                    coords = new List<(int X, int Y)>();
                }
            }
            if (sb.Length > 0)
            {
                int n = int.Parse(sb.ToString());
                numbers.Add(coords, n);
                sb = new StringBuilder();
            }
        }

        var partNumbers = numbers
            .Where(n => n.Key.Any(coord => parts.Any(p => Touching(p.Key, coord))))
            .Sum(n => n.Value);

        var gearNumbers = parts
            .Where(p => p.Value == '*')
            .Where(p => numbers.Count(n => n.Key.Any(coord => Touching(coord, p.Key))) == 2)
            .Select(p => numbers.Where(n => n.Key.Any(coord => Touching(coord, p.Key))).Aggregate(1, (x, y) => x * y.Value))
            .Sum();

        Console.WriteLine($"P1: {partNumbers}");
        Console.WriteLine($"P2: {gearNumbers}");
    }

    private static bool Touching((int X, int Y) a, (int X, int Y) b)
    {
        int xDiff = Math.Abs(a.X - b.X);
        int yDiff = Math.Abs(a.Y - b.Y);
        return xDiff <= 1 && yDiff <= 1;
    }
}