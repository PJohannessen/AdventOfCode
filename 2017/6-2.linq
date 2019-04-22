<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputLine = File.ReadAllLines("6.txt").First();
    LinkedList<int> nodes = new LinkedList<int>();
    foreach (var n in inputLine.Split('\t'))
    {
        nodes.AddLast(int.Parse(n));
    }
    
    HashSet<string> seen = new HashSet<string>();
    
    int count = 0;
    while (true)
    {
        count++;
        int max = nodes.Max();
        var currentNode = nodes.Find(max);
        currentNode.Value = 0;
        for (int i = max; i > 0; i--)
        {
            currentNode = currentNode.Next();
            currentNode.Value = currentNode.Value + 1;
        }
        string result = string.Join(" ", nodes.Select(n => n.ToString()));
        if (seen.Contains(result))
        {   
            count.Dump();
            return;
        }
        else
        {
            seen.Add(result);
        }
    }
}