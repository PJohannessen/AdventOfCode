<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("10.txt").ToArray();
    int targetAsteroid = 200;

    int totalHeight = inputLines.Length;
    int totalWidth = inputLines[0].Length;

    HashSet<(int X, int Y)> asteroids = new HashSet<(int, int)>();
    for (int y = 0; y < totalHeight; y++)
    {
        for (int x = 0; x < totalWidth; x++)
        {
            if (inputLines[y][x] == '#') asteroids.Add((x, y));
        }
    }

    (int X, int Y) bestAsteroid = (0, 0);
    int mostAsteroidsVisible = int.MinValue;

    foreach (var asteroid in asteroids)
    {
        int asteroidsVisible = asteroids.Where(otherAsteroid => otherAsteroid != asteroid)
                                        .GroupBy(otherAsteroid => CalculateLineOfSight(asteroid, otherAsteroid))
                                        .Count();
        if (asteroidsVisible > mostAsteroidsVisible)
        {
            mostAsteroidsVisible = asteroidsVisible;
            bestAsteroid = asteroid;
        }
    }

    $"P1: {mostAsteroidsVisible}".Dump();
  
    asteroids.Remove(bestAsteroid);
    
    int count = 0;
    
    while (true)
    {
        var grouped = asteroids.GroupBy(asteroid => CalculateLineOfSight(bestAsteroid, asteroid));
        
        // 90 degrees is straight up, so order by 90 => 180 then -180 => 90
        var orderedGrouped = grouped.OrderBy(g => g.Key)
                                    .Where(g => g.Key >= 90)
                                    .Concat(
                                        grouped.OrderBy(g => g.Key)
                                               .Where(g => g.Key < 90)
                                    );
        foreach (var g in orderedGrouped)
        {
            count++;
            var closest = g.OrderBy(g2 => ManhattanDistance(bestAsteroid, g2)).First();
            if (count == targetAsteroid)
            {
                $"P2: {closest.X * 100 + closest.Y}".Dump();
                return;
            }
            else
            {
                asteroids.Remove(closest);
            }
        }
    }
}
private int ManhattanDistance((int X, int Y) a1, (int X, int Y) a2)
{
    return Math.Abs(a2.Y - a1.Y) + Math.Abs(a2.X - a1.X);
}

private double CalculateLineOfSight((int X, int Y) a1, (int X, int Y) a2)
{
    return Math.Atan2(a1.Y - a2.Y, a1.X - a2.X) * 180.0 / Math.PI;
}
