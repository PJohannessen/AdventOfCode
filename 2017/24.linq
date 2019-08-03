<Query Kind="Program" />

int _maxStrength = 0;

int _maxStrengthLongest = 0;
int _longest = 0;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("24.txt");
    
    List<Component> components = new List<Component>();
    foreach (var s in steps)
    {
        int[] parts = s.Split('/').Select(p => int.Parse(p)).ToArray();
        components.Add(new Component(parts[0], parts[1]));
    }
    
    Build(0, new List<Component>(), components);

    $"P1: {_maxStrength}".Dump();
    $"P2: {_maxStrengthLongest}".Dump();
}

public void Build(int connector, List<Component> bridge, List<Component> availableComponents)
{
    var suitableComponents = availableComponents.Where(c => c.A == connector || c.B == connector).ToArray();
    if (suitableComponents.Length == 0)
    {
        int strength = bridge.Sum(sc => sc.A + sc.B);
        
        if (strength > _maxStrength) _maxStrength = strength;
        if (bridge.Count > _longest)
        {
            _longest = bridge.Count;
            _maxStrengthLongest = strength;
        }
        else if (bridge.Count == _longest && strength > _maxStrengthLongest)
        {
            _maxStrengthLongest = strength;
        }
    }
    else
    {
        foreach (var component in suitableComponents)
        {
            var nextBridge = new List<Component>(bridge);
            nextBridge.Add(component);
            var remainingComponents = availableComponents.Where(c => c != component).ToList();
            int nextConnector = component.A;
            if (component.A == connector) nextConnector = component.B;
            Build(nextConnector, nextBridge, remainingComponents);
        }
    }
    
}

public class Component
{
    public Component(int a, int b)
    {
        A = a;
        B = b;
    }
    
    public int A { get; private set; }
    public int B { get; private set; }
}