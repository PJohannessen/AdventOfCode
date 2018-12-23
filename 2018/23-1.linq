<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("23.txt");
    List<Nanobot> nanobots = new List<UserQuery.Nanobot>();
    foreach (var inputLine in inputLines)
    {
        var numbers = Regex.Matches(inputLine, @"[+-]?\d+(\.\d+)?");
        nanobots.Add(new Nanobot(int.Parse(numbers[0].Value), int.Parse(numbers[1].Value), int.Parse(numbers[2].Value), int.Parse(numbers[3].Value)));
    }    
    nanobots.Count(n2 => n2.InRange(nanobots.OrderByDescending(n => n.Signal).FirstOrDefault()) == true).Dump();
}

public class Nanobot
{
    public Nanobot(int x, int y, int z, int signal) { X = x; Y = y; Z = z; Signal = signal; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int Signal { get; }
    public double GetDistance(Nanobot nanobot)
    {
        double distance = Math.Abs(this.Z - nanobot.Z) + Math.Abs(this.Y - nanobot.Y) + Math.Abs(this.X - nanobot.X);
        return distance;
    }
    public bool InRange(Nanobot nanobot)
    {
        double distance = GetDistance(nanobot);
        return distance <= nanobot.Signal;
    }
}