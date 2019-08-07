<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("22.txt");
    List<Node> nodes = new List<Node>();
    
    for (int i = 2; i < inputLines.Length; i++)
    {
        string[] parts = inputLines[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        int[] positions = parts[0].Split(new[] { "/dev/grid/node-x", "-y" }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray();
        int size = int.Parse(parts[1].Substring(0, parts[1].Length - 1));
        int used = int.Parse(parts[2].Substring(0, parts[2].Length - 1));
        int available = int.Parse(parts[3].Substring(0, parts[3].Length - 1));
        int usePercent = int.Parse(parts[4].Substring(0, parts[4].Length - 1));
        
        nodes.Add(new Node(positions[0], positions[1], size, used, available, usePercent));
    }
    
    int count = 0;
    foreach (var nodeA in nodes.Where(n => n.Used > 0))
    {
        foreach (var nodeB in nodes.Where(n => n != nodeA))
        {
            if (nodeB.Available >= nodeA.Used)
            {
                count++;
            }
        }
    }
    $"P1: {count}".Dump();
    Console.WriteLine();
    
    var target = nodes.Single(n => n.X == 0 && n.Y == 0);
    var source = nodes.Single(n => n.X == nodes.Max(n2=> n2.X) && n.Y == 0);
    
    for (int y = 0; y <= nodes.Max(n => n.Y); y++)
    {
        for (int x = 0; x <= nodes.Max(n => n.X); x++)
        {
            var node = nodes.Single(n => n.X == x && n.Y == y);
            if (node == source) Console.Write('S');
            else if (node == target) Console.Write('T');
            else if (node.UsePercent == 0) Console.Write('$');
            else if (85 >= node.Used) Console.Write('.');
            else Console.Write('#');
        }
        Console.WriteLine();
    }

    // This draws a map with S (data we want), T (where data needs to go), nodes that are too large to move (#), nodes we can move (.) and the free node ($)
    // First need to move $ to S around the wall, which takes 67 steps
    // Then we need to move S left one at a time; this takes five moves (e.g. the node left of it goes down, right, right, up and then S takes its place)
    // It needs to move from x35 to x0, so repeat the above 35 times
    // Gives us (67 + (35*5))
    $"P2: {67+35*5}".Dump(); // 242
}

public class Node
{
    public Node (int x, int y, int size, int used, int available, int usePercent)
    {
        X = x;
        Y = y;
        Size = size;
        Used = used;
        Available = available;
        UsePercent = usePercent;
    }
    
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Size { get; private set; }
    public int Used { get; private set; }
    public int Available { get; private set; }
    public int UsePercent { get; private set; }
}