<Query Kind="Program" />

Dictionary<string, ProgramInfo> _programs;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("7.txt");
    
    _programs = new Dictionary<string, UserQuery.ProgramInfo>();
    foreach (var programLine in inputLines)
    {
        var programParts = programLine.Split(new[] { " (", ")", " -> ", ", "}, StringSplitOptions.RemoveEmptyEntries);
        _programs.Add(programParts[0], new ProgramInfo(programParts[0], int.Parse(programParts[1]), programParts.Where((pp, idx) => idx >= 2).ToList()));
    }
    
    string rootName = null;
    var childKeys = _programs.Values.SelectMany(v => v.ChildStrings).ToList();
    foreach (var programId in _programs.Keys)
    {
        if (childKeys.All(ck => ck != programId))
        {
            rootName = programId;
            break;
        }
    }
    $"P1: {rootName}".Dump();
    
    Node<ProgramInfo> root = new Node<ProgramInfo>(_programs[rootName]);
    PopulateChildren(root);
    
    var currentNode = root;
    int previousWeight = 0;
    while (true)
    {
        var groupedNodes = currentNode.Children.GroupBy(c => c.BFS().Sum(n => n.Weight));
        if (groupedNodes.Count() > 1)
        {
            previousWeight = groupedNodes.Single(gn => gn.Count() > 1).Key;
            currentNode = groupedNodes.Single(gn => gn.Count() == 1).Single();
        }
        else
        {
            int correctWeight = previousWeight - currentNode.BFS().Sum(n => n.Weight) + currentNode.Value.Weight;
            $"P2: {correctWeight}".Dump();
            break;
        }
    }
}

public void PopulateChildren(Node<ProgramInfo> current)
{
    foreach (var child in current.Value.ChildStrings)
    {
        var childNode = new Node<ProgramInfo>(_programs[child]);
        PopulateChildren(childNode);
        current.AddChild(childNode);
    }
}

public class ProgramInfo
{
    public ProgramInfo(string name, int weight, List<string> children)
    {
        Name = name;
        Weight = weight;
        ChildStrings = children;
    }
    
    public string Name { get; private set; }
    public int Weight { get; private set;}
    public List<string> ChildStrings { get; private set; }
}

public class Node<T>
{
    public List<Node<T>> Children { get; }
    public T Value { get; }
    
    public Node(T value)
    {
        Children = new List<Node<T>>();
        Value = value;
    }
    
    public void AddChild(Node<T> child)
    {
        Children.Add(child);
    }
    
    public IEnumerable<T> BFS()
    {
        yield return Value;
        foreach (Node<T> childNode in Children)
        {
            foreach (var gcNode in childNode.BFS())
            {
                yield return gcNode;
            }
        }
    }
}