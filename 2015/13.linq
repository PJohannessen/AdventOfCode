<Query Kind="Program" />

Dictionary<string, Dictionary<string, int>> lookup = new Dictionary<string, System.Collections.Generic.Dictionary<string, int>>();

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("13.txt");
    foreach (var inputLine in inputLines)
    {
        string[] parts = inputLine.Split(' ');
        if (!lookup.ContainsKey(parts[0])) lookup.Add(parts[0], new Dictionary<string, int>());
        int diff = 0;
        if (parts[2] == "gain") diff += int.Parse(parts[3]);
        else diff -= int.Parse(parts[3]);
        lookup[parts[0]].Add(parts[10].Substring(0, parts[10].Length - 1), diff);        
    }
    
    $"P1: {FindBest()}".Dump();
    
    lookup.Add("Me", new Dictionary<string, int>());
    foreach (var person in lookup.Keys.Where(k => k != "Me"))
    {
        lookup[person].Add("Me", 0);
        lookup["Me"].Add(person, 0);
    }
    $"P2: {FindBest()}".Dump();
}

int FindBest()
{
    int highestTotal = 0;
    foreach (var perm in Generate<string>(lookup.Count, lookup.Keys.ToArray()))
    {
        int currentTotal = 0;
        for (int i = 0; i < perm.Length; i++)
        {
            currentTotal += lookup[perm[i]][perm[i + 1 < perm.Length ? i + 1 : 0]];
            currentTotal += lookup[perm[i]][perm[i - 1 >= 0 ? i - 1 : perm.Length - 1]];
        }
        if (currentTotal > highestTotal) highestTotal = currentTotal;
    }
    return highestTotal;
}

// https://en.wikipedia.org/wiki/Heap%27s_algorithm
IEnumerable<T[]> Generate<T>(int n, T[] array)
{
    if (n == 1) yield return array;
    else
    {
        for (int i = 0; i < n - 1; i ++)
        {
           foreach (var perm in Generate<T>(n - 1, array)) yield return perm;
           if (n % 2 == 0)
           {
               T a = array[i];
               T b = array[n-1];
               array[i] = b;
               array[n-1] = a;
           }
           else
            {
                T a = array[0];
                T b = array[n-1];
                array[0] = b;
                array[n-1] = a;
            }
        }
        foreach (var perm in Generate<T>(n - 1, array)) yield return perm;
    }
}