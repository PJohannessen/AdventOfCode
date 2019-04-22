<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("2.txt");
    
    int checksum = 0;
    foreach (var inputLine in inputLines)
    {
        int min = int.MaxValue;
        int max = 0;
        var numbers = Regex.Matches(inputLine, @"[+-]?\d+(\.\d+)?");
        for (int i = 0; i < numbers.Count; i++)
        {
            var n = int.Parse(numbers[i].Value);
            min = Math.Min(n, min);
            max = Math.Max(n, max);
        }
        checksum += (max - min);
    }
    checksum.Dump();
}