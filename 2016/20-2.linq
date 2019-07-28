<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("20.txt");

    Dictionary<ulong, ulong> ranges = new Dictionary<ulong, ulong>();
    foreach (var il in inputLines)
    {
        ulong[] split = il.Split('-').Select(l => ulong.Parse(l)).ToArray();
        if (ranges.ContainsKey(split[0]))
        {
            if (ranges[split[0]] < split[1]) ranges[split[0]] = split[1];
        }
        else ranges.Add(split[0], split[1]);
    }
    
    ulong min = 0;
    ulong max = ranges.Values.Max();
    int valid = 0;
    
    while (min <= max)
    {
        if (ranges.Keys.Min() <= min)
        {
            if (min <= ranges[ranges.Keys.Min()]) min = ranges[ranges.Keys.Min()] + 1;
            ranges.Remove(ranges.Keys.Min());
        }
        else
        {
            valid++;
            min++;
        }
    }
    
    valid.Dump();
}