<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("14.txt");
    Dictionary<Tuple<int, int, int>, Tuple<int, int>> reindeer = new Dictionary<Tuple<int, int, int>, Tuple<int, int>>();
    int timeToRun = 2503;
    foreach (var inputLine in inputLines)
    {
        var matches = Regex.Matches(inputLine, @"[0-9]+");
        reindeer.Add(new Tuple<int, int, int>(int.Parse(matches[0].Value), int.Parse(matches[1].Value), int.Parse(matches[2].Value)), new Tuple<int, int>(0, 0));
    }

    for (int i = 1; i <= timeToRun; i++)
    {
        foreach (var r in reindeer.Keys.ToList())
        {
            int n = (i % (r.Item2 + r.Item3));
            if (n >= 1 && n <= r.Item2) reindeer[r] = new Tuple<int, int>(reindeer[r].Item1 + r.Item1, reindeer[r].Item2);
        }
        int leading = reindeer.Values.Max(t => t.Item1);
        foreach (var r in reindeer.Where(kvp => kvp.Value.Item1 == leading).Select(kvp => kvp.Key).ToList())
        {
            reindeer[r] = new Tuple<int, int>(reindeer[r].Item1, reindeer[r].Item2 + 1);
        }
        
    }
    
    reindeer.Values.Max(r => r.Item2).Dump();
}