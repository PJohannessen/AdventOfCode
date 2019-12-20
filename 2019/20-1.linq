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
    
    foreach (var portal in portals.Where(p => p.Value.Count == 2))
    {
        uint a = points[(portal.Value[0].X, portal.Value[0].Y)];
        uint b = points[(portal.Value[1].X, portal.Value[1].Y)];
        graph.Connect(a, b, 1, string.Empty);
        graph.Connect(b, a, 1, string.Empty);
    }
    
    uint start = points[(portals["AA"][0].X, portals["AA"][0].Y)];
    uint end = points[(portals["ZZ"][0].X, portals["ZZ"][0].Y)];
    
    var path = graph.Dijkstra(start, end);
    path.Distance.Dump();

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