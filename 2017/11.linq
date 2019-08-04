<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    Direction[] path = File.ReadAllLines("11.txt").Single().Split(',').Select(d => (Direction)Enum.Parse(typeof(Direction), d)).ToArray();
    
    int x = 0;
    int y = 0;
    int maxDistance = 0;
    
    foreach (var step in path)
    {
        switch (step)
        {
            case Direction.n:
                y -= 2;
                break;
            case Direction.ne:
                y -= 1;
                x += 1;
                break;
            case Direction.se:
                y += 1;
                x += 1;
                break;
            case Direction.s:
                y += 2;
                break;
            case Direction.sw:
                y += 1;
                x -= 1;
                break;
            case Direction.nw:
                y -= 1;
                x -= 1;
                break;
        }
        
        int currentDistance = (Math.Abs(x) + Math.Abs(y))/2;
        if (currentDistance > maxDistance) maxDistance = currentDistance;
    }
    
    int finalDistance = (Math.Abs(x) + Math.Abs(y))/2;

    $"P1: {finalDistance}".Dump();
    $"P2: {maxDistance}".Dump();
}

public enum Direction
{
    n,
    ne,
    se,
    s,
    sw,
    nw
}