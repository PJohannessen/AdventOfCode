<Query Kind="Program" />

void Main()
{
    int elements = 256;
    int rounds = 64;
    byte[] bytes = "183,0,31,146,254,240,223,150,2,206,161,1,255,232,199,88".Select(c => (byte)c).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();

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
    string.Join(string.Empty, denseHash.Select(i => i.ToString("X2"))).ToLower().Dump();
}