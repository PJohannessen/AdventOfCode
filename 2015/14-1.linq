<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("14.txt");
    List<Tuple<int, int, int>> reindeer = new List<Tuple<int, int, int>>();
    int timeToRun = 2503;
    foreach (var inputLine in inputLines)
    {
        var matches = Regex.Matches(inputLine, @"[0-9]+");
        reindeer.Add(new Tuple<int, int, int>(int.Parse(matches[0].Value), int.Parse(matches[1].Value), int.Parse(matches[2].Value)));
    }
    int longestRun = 0;
    foreach (var r in reindeer)
    {
        int run = 0;
        run += (int)Math.Floor((double)(timeToRun / (r.Item2 + r.Item3))) * r.Item1 * r.Item2;
        run += Math.Min(timeToRun % (r.Item2 + r.Item3), r.Item2) * r.Item1;
        if (run > longestRun) longestRun = run;
    }
    longestRun.Dump();
}