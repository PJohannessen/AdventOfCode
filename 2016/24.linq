<Query Kind="Program">
  <NuGetReference>Dijkstra.NET</NuGetReference>
  <Namespace>Dijkstra.NET.Graph</Namespace>
  <Namespace>Dijkstra.NET.Graph.Exceptions</Namespace>
  <Namespace>Dijkstra.NET.Graph.Simple</Namespace>
  <Namespace>Dijkstra.NET.PageRank</Namespace>
  <Namespace>Dijkstra.NET.ShortestPath</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] rows = File.ReadAllLines("24.txt");
    
    Dictionary<int, uint> places = new Dictionary<int, uint>();
    Graph<int, string> graph = new Graph<int, string>();
    
    int height = rows.Length;
    int width = rows[0].Length;
    
    // Populate the graph and connect the nodes
    for (int i = 0; i < height * width; i++) graph.AddNode(i);
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            char c = rows[y][x];
            
            if (c == '#') continue;
            else
            {
                uint current = (uint)(y*width + x);
                uint up = (uint)((y-1)*width + x);
                uint right = (uint)(y*width + x + 1);
                uint down = (uint)((y+1)*width + x);
                uint left = (uint)(y*width + x - 1);
                
                if (y > 0 && rows[y-1][x] != '#') graph.Connect(current, up, 1, string.Empty);
                if (y < height && rows[y+1][x] != '#') graph.Connect(current, down, 1, string.Empty);
                if (x > 0 && rows[y][x-1] != '#') graph.Connect(current, left, 1, string.Empty);
                if (x < width && rows[y][x+1] != '#') graph.Connect(current, right, 1, string.Empty);
                
                if (c != '.')
                {
                    places.Add(int.Parse(new string(new[] { c } )), current);
                }
            }
        }
    }
    
    // Calculate the distance between every pair of points
    Dictionary<Tuple<int, int>, int> pointCosts = new Dictionary<Tuple<int, int>, int>();
    foreach (var a in places.Keys)
    {
        foreach (var b in places.Keys.Where(b2 => b2 != a))
        {
            var cost = graph.Dijkstra(places[a], places[b]).Distance;
            pointCosts.Add(new Tuple<int, int>(a, b), cost);
        }
    }
    
    // Calculate min cost by checking all paths (always starting at 0)
    int minCost = int.MaxValue;
    foreach (var p in Permutations(places.Keys.Where(k => k != 0)))
    {
        var route = p.ToList();
        route.Insert(0, 0);
        int currentCost = 0;
        for (int i = 1; i < route.Count; i++)
        {
            currentCost += pointCosts[new Tuple<int, int>(route[i-1], route[i])];
        }
        if (currentCost < minCost) minCost = currentCost;
    }

    // Calculate min cost by checking all paths (always starting AND finishing at 0)
    int minCostToZero = int.MaxValue;
    foreach (var p in Permutations(places.Keys.Where(k => k != 0)))
    {
        var route = p.ToList();
        route.Insert(0, 0);
        route.Add(0);
        int currentCost = 0;
        for (int i = 1; i < route.Count; i++)
        {
            currentCost += pointCosts[new Tuple<int, int>(route[i - 1], route[i])];
        }
        if (currentCost < minCostToZero) minCostToZero = currentCost;
    }

    $"P1: {minCost}".Dump();
    $"P2: {minCostToZero}".Dump();
}


public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> source)
{
    if (source == null) throw new ArgumentNullException("source");
    return permutations(source.ToArray());
}

private static IEnumerable<IEnumerable<T>> permutations<T>(IEnumerable<T> source)
{
    var c = source.Count();
    if (c == 1) yield return source;
    else
    {
        for (int i = 0; i < c; i++)
        {
            foreach (var p in permutations(source.Take(i).Concat(source.Skip(i + 1))))
            {
                yield return source.Skip(i).Take(1).Concat(p);
            }
        }
    }

}