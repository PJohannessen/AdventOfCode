<Query Kind="Program">
  <NuGetReference>RoyT.AStar</NuGetReference>
  <Namespace>RoyT.AStar</Namespace>
</Query>

static void Main()
{
    int shortestPath = int.MaxValue;
    Dictionary<(int X, int Y), char> coords = new Dictionary<(int X, int Y), char>();
    Dictionary<char, (int X, int Y)> places = new Dictionary<char, (int X, int Y)>();
    Dictionary<char, (int X, int Y)> doors = new Dictionary<char, (int X, int Y)>();
    Load(coords, places, doors);
    string initialMoveable = "@.abcdefghijklmnopqrstuvwxyz";
    List<char> allKeys = places.Keys.Where(c => "@abcdefghijklmnopqrstuvwxyz".Contains(c)).ToList();
    Dictionary<(char k1, char k2), (int distance, char[] doors)> computed = new Dictionary<(char k1, char k2), (int distance, char[] doors)>();
    var grid = CreateGraph(initialMoveable);
    for (int i = 0; i < allKeys.Count; i++)
    {
        for (int j = i + 1; j < allKeys.Count; j++)
        {
            if (i == j) continue;
            char c1 = allKeys[i];
            char c2 = allKeys[j];
            var path = grid.GetPath(new Position(places[c1].X, places[c1].Y), new Position(places[c2].X, places[c2].Y), MovementPatterns.LateralOnly);
            if (path.Length > 0)
            {
                int distance = path.Length - 1;
                var doorsInPath = doors.Where(d => path.Contains(new Position(d.Value.X, d.Value.Y))).Select(d => d.Key).ToArray();
                computed.Add((c1, c2), (distance, doorsInPath));
                computed.Add((c2, c1), (distance, doorsInPath));
            }
        }
    }
    
    Dictionary<(char c, string s), int> hashes = new Dictionary<(char c, string s), int>();
    Next('@', new char[1] { '@' }, new char[0], 0);

    void Next(char place, char[] visited, char[] doorsOpen, int distance)
    {
        foreach (var next in computed.Where(
            c => c.Key.k1 == place &&
            c.Value.doors.All(d => doorsOpen.Contains(d)) &&
            !visited.Contains(c.Key.k2))
        )
        {
            string visitedS = new string(visited.OrderBy(c => c).ToArray());
            int nextDistance = distance + next.Value.distance;
            if (nextDistance > shortestPath) return;
            
            if (hashes.ContainsKey((next.Key.k2, visitedS)))
            {
                int bestDistance = hashes[(next.Key.k2, visitedS)];
                if (nextDistance > bestDistance) return;
                hashes[(next.Key.k2, visitedS)] = nextDistance;
            }
            else
            {
                hashes[(next.Key.k2, visitedS)] = nextDistance;
            }
            
            
            char[] nextVisited = visited.Concat(new char[] { next.Key.k2 }).ToArray();
            char[] nextDoorsOpen = doorsOpen.Concat(new char[] { char.ToUpper(next.Key.k2) }).ToArray();
            if (nextVisited.Length == allKeys.Count)
            {
                if (nextDistance < shortestPath)
                {
                    shortestPath = nextDistance;
                    shortestPath.Dump();
                }
                return;
            }
            Next(next.Key.k2, nextVisited, nextDoorsOpen, nextDistance);
        }
    }
    Grid CreateGraph(string moveable)
    {
        var grid = new Grid(81, 81, 1.0f);
        foreach (var kvp in coords.Where(kvp => kvp.Value == '#'))
        {
            grid.BlockCell(new Position(kvp.Key.X, kvp.Key.Y));
        }
        return grid;
    }
}
private static void Load(Dictionary<(int X, int Y), char> coords, Dictionary<char, (int X, int Y)> places, Dictionary<char, (int X, int Y)> doors)
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = System.IO.File.ReadAllLines("18-1.txt").ToArray();
    for (int y = 0; y < inputLines.Length; y++)
    {
        for (int x = 0; x < inputLines[0].Length; x++)
        {
            char c = inputLines[y][x];
            switch (c)
            {
                case '.':
                    coords[(x, y)] = c;
                    break;
                case '#':
                    coords[(x, y)] = c;
                    break;
                default:
                    coords[(x, y)] = c;
                    places[c] = (x, y);
                    if (char.IsUpper(c))
                    {
                        doors[c] = (x, y);
                    }
                    break;
            }
        }
    }
}