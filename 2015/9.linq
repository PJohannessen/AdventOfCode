<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("9.txt");
    Dictionary<Tuple<string, string>, int> routes = new Dictionary<System.Tuple<string, string>, int>();
    
    foreach (var il in inputLines)
    {
        var inputs = il.Split(new[] { " to ", " = " }, StringSplitOptions.RemoveEmptyEntries);
        routes.Add(new Tuple<string, string>(inputs[0], inputs[1]), int.Parse(inputs[2]));
        routes.Add(new Tuple<string, string>(inputs[1], inputs[0]), int.Parse(inputs[2]));
    }
    var cities = routes.Keys.Select(k => k.Item1).Union(routes.Keys.Select(k => k.Item2)).Distinct().ToList();
    var permutations = Permutations(cities);
    int shortestPath = int.MaxValue;
    int longestPath = int.MinValue;
    foreach (var p in permutations)
    {
        var path = p.ToList();
        int currentPath = 0;
        for (int i = 0; i < p.Count() - 1; i++)
        {
            currentPath += routes[new Tuple<string, string>(path[i], path[i+1])];
        }
        if (currentPath < shortestPath) shortestPath = currentPath;
        if (currentPath > longestPath) longestPath = currentPath;
    }
    $"P1: {shortestPath}".Dump();
    $"P2: {longestPath}".Dump();
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