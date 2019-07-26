<Query Kind="Program">
  <NuGetReference>Dijkstra.NET</NuGetReference>
  <Namespace>Dijkstra.NET.Contract</Namespace>
  <Namespace>Dijkstra.NET.Delegates</Namespace>
  <Namespace>Dijkstra.NET.Extensions</Namespace>
  <Namespace>Dijkstra.NET.Model</Namespace>
</Query>

const int favouriteNumber = 1350;
const int startX = 1;
const int startY = 1;
const int targetX = 31;
const int targetY = 39;
const int maxX = 50;
const int maxY = 50;

void Main()
{
    var graph = new Graph<int, string>();
    foreach (int i in Enumerable.Range(0, maxX * maxY))
    {
        graph.AddNode(i);
    }
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            bool isOpen = IsOpen(x, y);
            if (isOpen)
            {
                var adjacents = Adjacents(x, y);
                foreach (var adj in adjacents)
                {
                    bool isAdjacentOpen = IsOpen(adj.Item1, adj.Item2);
                    if (isAdjacentOpen)
                    {
                        graph.Connect(Value(x, y), Value(adj.Item1, adj.Item2), 1, null);
                    }
                }
            }
        }
    }
    var shortestPath = graph.Dijkstra(Value(startX, startY), Value(targetX, targetY));
    int inReach = 0;
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            var path = graph.Dijkstra(Value(startX, startY), Value(x, y));
            if (path.IsFounded && path.Distance <= 50) inReach++;
        }
    }

    $"P1: {shortestPath.Distance}".Dump();
    $"P2: {inReach}".Dump();
}

private static IEnumerable<Tuple<int, int>> Adjacents(int x, int y)
{
    // 0,0
    if (x > 0)
        yield return new Tuple<int, int>(x - 1, y);
    if (x < maxX-1)
        yield return new Tuple<int, int>(x + 1, y);
    if (y > 0)
        yield return new Tuple<int, int>(x, y - 1);
    if (y < maxY-1)
        yield return new Tuple<int, int>(x, y + 1);

}

private static bool IsOpen(int x, int y)
{
    int sum = x*x + 3*x + 2*x*y + y + y*y + favouriteNumber;
    int bits = CountBits((uint)sum);
    return bits % 2 == 0;
}

public static int CountBits(uint value)
{
    int count = 0;
    while (value != 0)
    {
        count++;
        value &= value - 1;
    }
    return count;
}

public static uint Value(int x, int y)
{
    return (uint)(x + y * maxX);
}