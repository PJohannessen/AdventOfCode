<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("2.txt");
    
    int sum = 0;
    foreach (var inputLine in inputLines)
    {
        sum += FindSum(inputLine);
    }
    sum.Dump();
}

int FindSum(string inputLine)
{
    int a = 0;
    int b = 0;
    var numbers = Regex.Matches(inputLine, @"[+-]?\d+(\.\d+)?");
    for (int i = 0; i < numbers.Count - 1; i++)
    {
        a = int.Parse(numbers[i].Value);
        for (int j = i + 1; j < numbers.Count; j++)
        {
            b = int.Parse(numbers[j].Value);
            if (Math.Max(a, b) % Math.Min(a, b) == 0)
            {
                return Math.Max(a, b) / Math.Min(a, b);
            }
        }
    }
    return 0;
}