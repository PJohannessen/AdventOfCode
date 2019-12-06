<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    List<(string Parent, string Child)> orbitPairs =
        File.ReadAllLines("6.txt")
        .Select(op => (op.Substring(0,3), op.Substring(4,3)))
        .ToList();
    Dictionary<string, SpaceThing> spaceThings = new Dictionary<string, UserQuery.SpaceThing>();

    foreach (var st in orbitPairs.Select(l => l.Parent)
                                 .Concat(orbitPairs.Select(l => l.Child))
                                 .Distinct()
                                 .Select(t => new SpaceThing {Name = t}))
    {
        spaceThings.Add(st.Name, st);
    }
    
    foreach (var op in orbitPairs)
    {
        spaceThings[op.Parent].Children.Add(spaceThings[op.Child]);
        spaceThings[op.Child].Parent = spaceThings[op.Parent];
    }
    
    List<string> youParents = spaceThings["YOU"].Parents().ToList();
    List<string> sanParents = spaceThings["SAN"].Parents().ToList();
    var commonParent = youParents.Intersect(sanParents).First();
    
    int p1 = spaceThings.Values.Sum(t => t.CountParents());
    int p2 = youParents.IndexOf(commonParent) + sanParents.IndexOf(commonParent);

    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}

public class SpaceThing
{
    public string Name { get; set; }
    public SpaceThing Parent { get; set; }
    public List<SpaceThing> Children { get; set; } = new List<SpaceThing>();
    
    public int CountParents()
    {
        if (Parent == null) return 0;
        else return (1 + Parent.CountParents());
    }
    
    public IEnumerable<string> Parents()
    {
        if (Parent != null)
        {
            yield return Parent.Name;
            foreach (var p in Parent.Parents())
            {
                yield return p;
            }
        }
    }
}