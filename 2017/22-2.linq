<Query Kind="Program" />

int _bursts = 10000000;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] lines = File.ReadAllLines("22.txt");
    Dictionary<Tuple<int, int>, State> places = new Dictionary<Tuple<int, int>, State>();
    
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
            if (lines[y+maxY][x+maxX] == '#') places.Add(new Tuple<int, int>(y, x), State.I);
        }
    }
    
    Direction d = Direction.N;
    var pos = new Tuple<int, int>(0, 0);
    int newInfections = 0;
    
    for (int i = 1; i <= _bursts; i++)
    {
        State? state = places.ContainsKey(pos) ? places[pos] : (State?)null;
        
        if (state == State.I) d = Infected[d];
        else if (state == State.F) d = Flagged[d];
        else if (state == null) d = Clean[d];
        // leave weakened alone

        if (state == State.I) places[pos] = State.F;
        else if (state == State.F) places.Remove(pos);
        else if (state == null) places.Add(pos, State.W);
        else if (state == State.W)
        {
            places[pos] = State.I;
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

public enum State
{
    W,
    I,
    F
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

public static Dictionary<Direction, Direction> Flagged = new Dictionary<Direction, Direction>
{
    { Direction.N, Direction.S },
    { Direction.W, Direction.E },
    { Direction.S, Direction.N },
    { Direction.E, Direction.W }
};