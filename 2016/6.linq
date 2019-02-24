<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("6.txt");
    
    for (int i = 0; i < inputLines[0].Length; i++)
    {
        var chars = inputLines.Select(il => il[i]).ToList();
        Console.Write((chars.GroupBy(c => c).OrderByDescending(c => c.Count()).ToList()).First().Key);
    }
    Console.WriteLine();
    for (int i = 0; i < inputLines[0].Length; i++)
    {
        var chars = inputLines.Select(il => il[i]).ToList();
        Console.Write((chars.GroupBy(c => c).OrderByDescending(c => c.Count()).ToList()).Last().Key);
    }
}