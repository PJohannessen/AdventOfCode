<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("4.txt");
    
    int valid = 0;
    foreach (var line in inputLines)
    {
        var words = line.Split(' ').Select(w => new string(w.OrderBy(l => l).ToArray())).ToList();
        if (words.Count == words.Distinct().Count()) valid++;
    }
    valid.Dump();
}