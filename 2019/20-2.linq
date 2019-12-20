<Query Kind="Program">
  <NuGetReference>Dijkstra.NET</NuGetReference>
  <Namespace>Dijkstra.NET.Graph</Namespace>
  <Namespace>Dijkstra.NET.Graph.Exceptions</Namespace>
  <Namespace>Dijkstra.NET.Graph.Simple</Namespace>
  <Namespace>Dijkstra.NET.PageRank</Namespace>
  <Namespace>Dijkstra.NET.ShortestPath</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = System.IO.File.ReadAllLines("20.txt");
    
    Dictionary<string, List<(int X, int Y)>> portals = new Dictionary<string, List<(int X, int Y)>>();
    Dictionary<(int X, int Y), uint> points = new Dictionary<(int X, int Y), uint>();
    Graph<(int X, int Y), string> graph = new Graph<(int X, int Y), string>();
    
    for (int y = 0; y < inputLines.Length; y++)
    {
        for (int x = 0; x < inputLines[0].Length; x++)
        {
            char c = inputLines[y][x];
            if (c == '.')
            {
                uint n = graph.AddNode((x, y));
                points.Add((x, y), n);
            }
        }
    }
    
    string CAPS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    for (int y = 0; y < inputLines.Length; y++)
    {
        for (int x = 0; x < inputLines[0].Length; x++)
        {
            char c = inputLines[y][x];
            if (c == '.')
            {
                uint point = points[(x, y)];
                var adjacent = GetAdjacent(x, y);
                foreach (var a in adjacent.Where(adj => adj.C == '.'))
                {
                    graph.Connect(point, points[(a.X, a.Y)], 1, string.Empty);
                }
            }
            if (CAPS.Contains(c))
            {
                var adjacent = GetAdjacent(x, y);
                if (adjacent.Any(a => a.C == '.') && adjacent.Any(a => CAPS.Contains(a.C)))
                {
                    var portal = adjacent.Single(a => a.C == '.');
                    var ch = adjacent.Single(a => CAPS.Contains(a.C));
                    string name = new string(new char[] { c, ch.C }.OrderBy(c => c).ToArray());
                    if (!portals.ContainsKey(name)) portals.Add(name, new List<(int X, int Y)>());
                    portals[name].Add((portal.X, portal.Y));
                }
            }
        }
    }
    
    (int X, int Y) start = portals["AA"][0];
    (int X, int Y) end = portals["ZZ"][0];
    
    Dictionary<(int X, int Y), Dictionary<(int X, int Y), int>> distances =
        new Dictionary<(int X, int Y), Dictionary<(int X, int Y), int>>();
        
    List<(int x, int y)> allPortals = portals.SelectMany(p => p.Value).ToList();
    foreach (var portal in allPortals)
    {
        distances.Add(portal, new Dictionary<(int X, int Y), int>());
    }
    
    for (int p1 = 0; p1 < allPortals.Count-1; p1++)
    {
        for (int p2 = p1+1; p2 < allPortals.Count; p2++)
        {
            var path = graph.Dijkstra(points[allPortals[p1]], points[allPortals[p2]]);
            if (path.IsFounded)
            {
                distances[allPortals[p1]].Add(allPortals[p2], path.Distance);
                distances[allPortals[p2]].Add(allPortals[p1], path.Distance);
            }
        }
    }
    
    int bestDistance = 7500; // Keep increasing til the answer is within range
    Search(start, 0, 0);
    bestDistance.Dump();
    
    void Search((int X, int Y) start, int currentDistance, int depth)
    {
        if (currentDistance > bestDistance || depth < 0) return;
        if (depth == 0 && distances[start].ContainsKey(end))
        {
            int finalDistance = currentDistance + distances[start][end];
            if (finalDistance < bestDistance) bestDistance = finalDistance;
            return;
        }
        foreach (var visitable in distances[start].Where(d => d.Key != start && d.Key != end))
        {
            int distanceModifier = 1;
            if (visitable.Key.X == allPortals.Min(p => p.x) ||
                visitable.Key.Y == allPortals.Min(p => p.y) ||
                visitable.Key.X == allPortals.Max(p => p.x) ||
                visitable.Key.Y == allPortals.Max(p => p.y))
                {
                    distanceModifier = -1;
                }
            var nextPortal = portals.Single(p => p.Value.Contains(visitable.Key)).Value.SingleOrDefault(p => p != visitable.Key);
            if (nextPortal == default((int X, int Y))) continue;
            int nextDistance = currentDistance+distances[start][visitable.Key]+1;
            Search(nextPortal, nextDistance, depth+distanceModifier);
        }
    }
    
    List<(int X, int Y, char C)> GetAdjacent(int x, int y)
    {
        List<(int X, int Y, char C)> adjacent = new List<(int X, int Y, char C)>();
        if (x > 0) // Left
        {
            char c = inputLines[y][x-1];
            adjacent.Add((x-1, y, c));
        }
        if (x < inputLines[0].Length-1) // Right
        {
            char c = inputLines[y][x+1];
            adjacent.Add((x+1, y, c));
        }
        if (y > 0) // Up
        {
            char c = inputLines[y-1][x];
            adjacent.Add((x, y-1, c));
        }
        if (y < inputLines.Length-1) // Down
        {
            char c = inputLines[y+1][x];
            adjacent.Add((x, y+1, c));
        }
        
        return adjacent;
    }
}