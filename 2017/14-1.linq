<Query Kind="Program" />

void Main()
{
    int size = 128;
    int[,] grid = new int[size, size];
    string key = "wenycdww";

    int squareCount = 0;
    for (int y = 0; y < size; y++)
    {
        string knot = $"{key}-{y}";
        int[] knotHash = KnotHash(knot);
        string row = string.Join(string.Empty, knotHash.Select(i => Convert.ToString(i, 2).PadLeft(8, '0')));
        squareCount += row.Count(c => c == '1');
    }
    
    squareCount.Dump();
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