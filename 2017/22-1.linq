<Query Kind="Program" />

int _bursts = 10000;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] lines = File.ReadAllLines("22.txt");
    HashSet<Tuple<int, int>> infections = new HashSet<Tuple<int, int>>();
    
    int minX;
    int maxX;
    int minY;
    int maxY;
    maxY = lines.Length / 2;
    minY = maxY * -1;
    maxX = lines.First().Length / 2;
    minX = maxX * -1;
    
    for (int y = minY ; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            if (lines[y+maxY][x+maxX] == '#') infections.Add(new Tuple<int, int>(y, x));
        }
    }
    
    Direction d = Direction.N;
    var pos = new Tuple<int, int>(0, 0);
    int newInfections = 0;
    
    for (int i = 1; i <= _bursts; i++)
    {
        bool infected = infections.Contains(pos);
        if (infected) d = Infected[d];
        else d = Clean[d];

        if (infected) infections.Remove(pos);
        else
        {
            infections.Add(pos);
            newInfections++;
        }
        
        if (d == Direction.N) pos = new Tuple<int, int>(pos.Item1 - 1, pos.Item2);
        else if (d == Direction.S) pos = new Tuple<int, int>(pos.Item1 + 1, pos.Item2);
        else if (d == Direction.W) pos = new Tuple<int, int>(pos.Item1, pos.Item2 - 1);
        else if (d == Direction.E) pos = new Tuple<int, int>(pos.Item1, pos.Item2 + 1);
    }
    
    newInfections.Dump();
}

public enum Direction
{
    N,
    S,
    E,
    W
}

public static Dictionary<Direction, Direction> Infected = new Dictionary<Direction, Direction>
{
    { Direction.N, Direction.E },
    { Direction.E, Direction.S },
    { Direction.S, Direction.W },
    { Direction.W, Direction.N }
};

public static Dictionary<Direction, Direction> Clean = new Dictionary<Direction, Direction>
{
    { Direction.N, Direction.W },
    { Direction.W, Direction.S },
    { Direction.S, Direction.E },
    { Direction.E, Direction.N }
};