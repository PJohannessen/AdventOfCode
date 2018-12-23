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
    
    // X, Y and Z coordinates were determined by stepping through large ranges, and adjusting down over time until the area with the highestCount can be found
    int highestCount = 0;
    int startingX = 50989028;
    int startingY = 13298770;
    int startingZ = 44601502;
    double lowestDistance = double.MaxValue;
    int buffer = 10;
    int step = 1;
    for (int z = startingZ + buffer; z >= startingZ - buffer; z -= step)
    {
        for (int y = startingY + buffer; y >= startingY - buffer; y -= step)
        {
            for (int x = startingX + buffer; x >= startingX - buffer; x -= step)
            {
                int count = 0;
                foreach (var n in nanobots)
                {
                    if (n.InRange(x, y, z)) count++;
                }
                if (count > highestCount)
                {
                    highestCount = count;
                    lowestDistance = GetDistanceFromZero(x, y, z);
                }
                else if (count == highestCount)
                {
                    double distanceFromZero = GetDistanceFromZero(x, y, z);
                    if (distanceFromZero < lowestDistance)
                    {
                        lowestDistance = distanceFromZero;
                    }
                }
                highestCount = Math.Max(count, highestCount);
            }
        }
    }

    lowestDistance.Dump();
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
    public double GetDistance(int x, int y, int z)
    {
        double distance = Math.Abs(this.Z - z) + Math.Abs(this.Y - y) + Math.Abs(this.X - x);
        return distance;
    }
    public bool InRange(int x, int y, int z)
    {
        double distance = GetDistance(x, y, z);
        return distance <= this.Signal;
    }
}

public double GetDistanceFromZero(int x, int y, int z)
{
    double distance = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
    return distance;
}