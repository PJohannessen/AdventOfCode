<Query Kind="Program" />

void Main()
{
    int size = 128;
    int[,] grid = new int[size, size];
    string key = "wenycdww";

    for (int y = 0; y < size; y++)
    {
        string knot = $"{key}-{y}";
        int[] knotHash = KnotHash(knot);
        string row = string.Join(string.Empty, knotHash.Select(i => Convert.ToString(i, 2).PadLeft(8, '0')));
        for (int x = 0; x < size; x++)
        {
            if (row[x] == '1') grid[y,x] = -1;
        }
    };
    
    int nextRegion = 1;
    
    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            if (grid[y,x] != -1) continue;
            int currentRegion = nextRegion;
            Populate(currentRegion, y, x);
            nextRegion++;
        }
    }
    
    (nextRegion-1).Dump();
    
    void Populate(int region, int y, int x)
    {
        if (grid[y, x] >= 0) return;
        grid[y, x] = region;
        if (y > 0) Populate(region, y-1, x);
        if (y < size-1) Populate(region, y+1, x);
        if (x > 0) Populate(region, y, x-1);
        if (x < size-1) Populate(region, y, x+1);
    }
}

public int[] KnotHash(string input)
{
    int elements = 256;
    int rounds = 64;
    byte[] bytes = input.Select(c => (byte)c).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();

    int position = 0;
    int skipSize = 0;

    List<int> list = new List<int>();
    for (int i = 0; i < elements; i++) list.Add(i);

    for (int r = 1; r <= rounds; r++)
    {
        foreach (int l in bytes)
        {
            List<int> tempList = new List<int>(list);

            for (int i = 0; i < l; i++)
            {
                int sourcePos = (position + l - 1 - i) % list.Count;
                int replacePos = (position + i) % list.Count;
                tempList[replacePos] = list[sourcePos];
            }

            list = tempList;
            position += l;
            position += skipSize;
            skipSize++;
        }
    }

    List<int> denseHash = new List<int>();
    for (int i = 0; i < list.Count; i += 16)
    {
        denseHash.Add(list.Skip(i + 1).Take(15).Aggregate(list[i], (prev, next) => prev ^ next));
    }
    return denseHash.ToArray();
}