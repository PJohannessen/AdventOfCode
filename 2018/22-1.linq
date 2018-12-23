<Query Kind="Program" />

const int Depth = 11394;

void Main()
{
    int width = 10;
    Tuple<int, int> target = new Tuple<int, int>(7, 701);
    Region[,] cave = new Region[width, Depth];
    for (int y = 0; y < Depth; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (x == 0 && y == 0) cave[x, y] = new Region(x, y, 0);
            else if (x == target.Item1 && y == target.Item2) cave[x,y] = new Region(x, y, 0);
            else if (y == 0) cave[x, y] = new Region(x, y, x * 16807);
            else if (x == 0) cave[x, y] = new Region(x, y, y * 48271);
            else cave[x, y] = new Region(x, y, cave[x-1, y].ErosionLevel * cave[x, y-1].ErosionLevel);
        }
    }
    
    int risk = 0;
    for (int y = 0; y <= target.Item2; y++)
    {
        for (int x = 0; x <= target.Item1; x++)
        {
            risk += (int)cave[x, y].Type;
        }
    }
    
    risk.Dump();
}

class Region
{
    public Region(int x, int y, int geologicalIndex) { X = x; Y = y; GeologicalIndex = geologicalIndex; }
    public int X { get; }
    public int Y { get; }
    public int GeologicalIndex { get; }
    public int ErosionLevel
    {
        get
        {
            return (GeologicalIndex + Depth) % 20183;
        }
    }
    public RegionType Type { get { return (RegionType)(ErosionLevel % 3); } } 
}

enum RegionType
{
    Rocky = 0,
    Wet = 1,
    Narrow = 2,
}
