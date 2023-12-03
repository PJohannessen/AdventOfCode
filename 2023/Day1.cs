namespace aoc;

public class Day1
{
    public static void Process()
    {
        string[] lines = Utils.ParseStrings("1.txt");

        var matches = new Dictionary<string, int>()
        {
            { "0", 0 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        List<int> p1Values = new List<int>();
        List<int> p2Values = new List<int>();
        
        foreach (var line in lines)
        {
            var p1FirstMatch =
                matches
                    .Where(kvp => char.IsDigit(kvp.Key[0]))
                    .Select(kvp => (kvp.Value, line.IndexOf(kvp.Key, StringComparison.Ordinal)))
                    .Where(tuple => tuple.Item2 >= 0)
                    .MinBy(tuple => tuple.Item2)
                    .Value;
            var p1LastMatch =
                matches
                    .Where(kvp => char.IsDigit(kvp.Key[0]))
                    .Select(kvp => (kvp.Value, line.LastIndexOf(kvp.Key, StringComparison.Ordinal)))
                    .Where(tuple => tuple.Item2 >= 0)
                    .MaxBy(tuple => tuple.Item2)
                    .Value;
            p1Values.Add(int.Parse($"{p1FirstMatch}{p1LastMatch}"));
            
            var p2FirstMatch =
                matches
                    .Select(kvp => (kvp.Value, line.IndexOf(kvp.Key, StringComparison.Ordinal)))
                    .Where(tuple => tuple.Item2 >= 0)
                    .MinBy(tuple => tuple.Item2)
                    .Value;
            var p2LastMatch =
                matches
                    .Select(kvp => (kvp.Value, line.LastIndexOf(kvp.Key, StringComparison.Ordinal)))
                    .Where(tuple => tuple.Item2 >= 0)
                    .MaxBy(tuple => tuple.Item2)
                    .Value;
            p2Values.Add(int.Parse($"{p2FirstMatch}{p2LastMatch}"));
        }

        Console.WriteLine($"P1: {p1Values.Sum()}");
        Console.WriteLine($"P2: {p2Values.Sum()}");
    }
}