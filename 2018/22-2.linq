<Query Kind="Program">
  <NuGetReference>Dijkstra.NET</NuGetReference>
  <Namespace>Dijkstra.NET.Contract</Namespace>
  <Namespace>Dijkstra.NET.Delegates</Namespace>
  <Namespace>Dijkstra.NET.Extensions</Namespace>
  <Namespace>Dijkstra.NET.Model</Namespace>
</Query>

const int Height = 800;
const int Depth = 11394;
static Tuple<int, int> Target = new Tuple<int, int>(7, 701);
const int Width = 50;


Graph<uint, string> graph = new Graph<uint, string>();
uint currentNode = 0;
Region[,] cave = new Region[Width, Height];

void Main()
{
    for (int y = 0; y < Height; y++)
    {
        for (int x = 0; x < Width; x++)
        {
            if (x == 0 && y == 0) cave[x, y] = new Region(x, y, 0, currentNode);
            else if (x == Target.Item1 && y == Target.Item2) cave[x, y] = new Region(x, y, 0, currentNode);
            else if (y == 0) cave[x, y] = new Region(x, y, x * 16807, currentNode);
            else if (x == 0) cave[x, y] = new Region(x, y, y * 48271, currentNode);
            else cave[x, y] = new Region(x, y, cave[x - 1, y].ErosionLevel * cave[x, y - 1].ErosionLevel, currentNode);
            graph.AddNode(cave[x, y].NodeTorch);
            graph.AddNode(cave[x, y].NodeClimbingGear);
            graph.AddNode(cave[x, y].NodeNeither);
            if (cave[x, y].Type == RegionType.Narrow)
            {
                graph.Connect(cave[x,y].NodeTorch, cave[x,y].NodeNeither, 7, string.Empty);
                graph.Connect(cave[x,y].NodeNeither, cave[x,y].NodeTorch, 7, string.Empty);
            }
            else if (cave[x, y].Type == RegionType.Rocky)
            {
                graph.Connect(cave[x, y].NodeClimbingGear, cave[x, y].NodeTorch, 7, string.Empty);
                graph.Connect(cave[x, y].NodeTorch, cave[x, y].NodeClimbingGear, 7, string.Empty);
            }
            else
            {
                graph.Connect(cave[x, y].NodeClimbingGear, cave[x, y].NodeNeither, 7, string.Empty);
                graph.Connect(cave[x, y].NodeNeither, cave[x, y].NodeClimbingGear, 7, string.Empty);
            }

            currentNode += 3;
        }
    }

    for (int y = 0; y < Height; y++)
    {
        for (int x = 0; x < Width; x++)
        {
            Region currentRegion = cave[x, y];
            if (x + 1 < Width - 1) Pair(currentRegion, cave[x + 1, y]);
            if (x - 1 >= 0) Pair(currentRegion, cave[x - 1, y]);
            if (y + 1 < Height - 1) Pair(currentRegion, cave[x, y + 1]);
            if (y - 1 >= 0) Pair(currentRegion, cave[x, y - 1]);
        }
    }

    var shortestPath = graph.Dijkstra(cave[0, 0].NodeTorch, cave[Target.Item1, Target.Item2].NodeTorch);
    shortestPath.Distance.Dump();
}

void Pair(Region currentRegion, Region newRegion)
{
    if (currentRegion.Type == RegionType.Narrow && newRegion.Type == RegionType.Narrow)
    {
        graph.Connect(currentRegion.NodeTorch, newRegion.NodeTorch, 1, string.Empty);
        graph.Connect(currentRegion.NodeNeither, newRegion.NodeNeither, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Rocky && newRegion.Type == RegionType.Rocky)
    {
        graph.Connect(currentRegion.NodeTorch, newRegion.NodeTorch, 1, string.Empty);
        graph.Connect(currentRegion.NodeClimbingGear, newRegion.NodeClimbingGear, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Wet && newRegion.Type == RegionType.Wet)
    {
        graph.Connect(currentRegion.NodeClimbingGear, newRegion.NodeClimbingGear, 1, string.Empty);
        graph.Connect(currentRegion.NodeNeither, newRegion.NodeNeither, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Narrow && newRegion.Type == RegionType.Rocky)
    {
        graph.Connect(currentRegion.NodeTorch, newRegion.NodeTorch, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Narrow && newRegion.Type == RegionType.Wet)
    {
        graph.Connect(currentRegion.NodeNeither, newRegion.NodeNeither, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Rocky && newRegion.Type == RegionType.Narrow)
    {
        graph.Connect(currentRegion.NodeTorch, newRegion.NodeTorch, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Rocky && newRegion.Type == RegionType.Wet)
    {
        graph.Connect(currentRegion.NodeClimbingGear, newRegion.NodeClimbingGear, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Wet && newRegion.Type == RegionType.Narrow)
    {
        graph.Connect(currentRegion.NodeNeither, newRegion.NodeNeither, 1, string.Empty);
    }
    else if (currentRegion.Type == RegionType.Wet && newRegion.Type == RegionType.Rocky)
    {
        graph.Connect(currentRegion.NodeClimbingGear, newRegion.NodeClimbingGear, 1, string.Empty);
    }
}

enum Equipment
{
    Torch,
    ClimbingGear,
    Neither
}

class Region
{
    public Region(int x, int y, int geologicalIndex, uint node) { X = x; Y = y; GeologicalIndex = geologicalIndex; NodeTorch = node; NodeClimbingGear = node + 1; NodeNeither = node + 2;}
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
    public uint NodeTorch { get; }
    public uint NodeClimbingGear { get; }
    public uint NodeNeither { get; }
}

enum RegionType
{
    Rocky = 0,
    Wet = 1,
    Narrow = 2,
}
