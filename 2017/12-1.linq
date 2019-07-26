<Query Kind="Program" />

static Dictionary<int, List<int>> programs = new Dictionary<int, List<int>>();
static HashSet<int> connected = new HashSet<int>();

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("12.txt");
    
    foreach (var il in inputLines)
    {
        string[] inputs = il.Split(new[] { " <-> ", ", " }, StringSplitOptions.RemoveEmptyEntries);
        programs.Add(int.Parse(inputs[0]), inputs.Skip(1).Select(i => int.Parse(i)).ToList());
    }
    
    Check(0, new HashSet<int>());
    connected.Count().Dump();
}

static void Check(int p, HashSet<int> from)
{
    if (!programs.ContainsKey(p) || from.Contains(p)) return;
    if (!connected.Contains(p)) connected.Add(p);
    var current = new HashSet<int>(from);
    current.Add(p);
    
    foreach (var p2 in programs[p])
    {
        Check(p2, current);
    }
}