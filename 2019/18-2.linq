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
    List<char> allKeysAndRobots = places.Keys.Where(c => "1234abcdefghijklmnopqrstuvwxyz".Contains(c)).OrderBy(c => c).ToList();
    List<char> allKeys = places.Keys.Where(c => "abcdefghijklmnopqrstuvwxyz".Contains(c)).ToList();
    Dictionary<(char k1, char k2), (int distance, char[] doors)> computed = new Dictionary<(char k1, char k2), (int distance, char[] doors)>();
    var grid = CreateGraph();
    Dictionary<int, List<char>> groups = new Dictionary<int, List<char>>();
    groups.Add(49, new List<char>());
    groups.Add(50, new List<char>());
    groups.Add(51, new List<char>());
    groups.Add(52, new List<char>());
    for (int i = 0; i < allKeysAndRobots.Count; i++)
    {
        for (int j = i + 1; j < allKeysAndRobots.Count; j++)
        {
            if (i == j) continue;
            char c1 = allKeysAndRobots[i];
            char c2 = allKeysAndRobots[j];
            var path = grid.GetPath(new Position(places[c1].X, places[c1].Y), new Position(places[c2].X, places[c2].Y), MovementPatterns.LateralOnly);
            if (path.Length > 0)
            {
                if ("1234".Contains(c1)) {
                    if (!groups[c1].Contains(c2)) groups[c1].Add(c2);
                }
                int distance = path.Length - 1;
                var doorsInPath = doors.Where(d => path.Contains(new Position(d.Value.X, d.Value.Y))).Select(d => d.Key).ToArray();
                computed.Add((c1, c2), (distance, doorsInPath));
                computed.Add((c2, c1), (distance, doorsInPath));
            }
        }
    }
    
    Dictionary<(char c, string s), int> hashes = new Dictionary<(char c, string s), int>();
    Next(new[] { places['1'], places['2'], places['3'], places['4'] }, new char[0], new char[0], 0);

    void Next((int X, int Y)[] robots, char[] visited, char[] doorsOpen, int distance)
    {
        for (int i = 49; i <= 52; i++)
        {
            var robot = robots[i - 49];
            foreach (var next in groups[i].Where(c => !visited.Contains(c)))
            {
                var path = computed[(coords[robot], coords[places[next]])];
                if (path.doors.Any(d => !doorsOpen.Contains(d))) continue;
                
                string visitedS = new string(visited.OrderBy(c => c).ToArray());
                int nextDistance = distance + path.distance;
                if (nextDistance > shortestPath) return;

                if (hashes.ContainsKey((next, visitedS)))
                {
                    int bestDistance = hashes[(next, visitedS)];
                    if (nextDistance > bestDistance) return;
                    hashes[(next, visitedS)] = nextDistance;
                }
                else
                {
                    hashes[(next, visitedS)] = nextDistance;
                }


                char[] nextVisited = visited.Concat(new char[] { next }).ToArray();
                char[] nextDoorsOpen = doorsOpen.Concat(new char[] { char.ToUpper(next) }).ToArray();
                if (nextVisited.Length == allKeys.Count)
                {
                    if (nextDistance < shortestPath)
                    {
                        shortestPath = nextDistance;
                        shortestPath.Dump();
                    }
                    return;
                }
                var newRobots = ((int X, int Y)[])robots.Clone();
                newRobots[i-49] = places[next];
                Next(newRobots, nextVisited, nextDoorsOpen, nextDistance);
            }
        }
    }
    Grid CreateGraph()
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
    string[] inputLines = System.IO.File.ReadAllLines("18-2.txt").ToArray();
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