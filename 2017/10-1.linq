<Query Kind="Program" />

void Main()
{
    int elements = 256;
    int[] lengths = "183,0,31,146,254,240,223,150,2,206,161,1,255,232,199,88".Split(',').Select(l => int.Parse(l)).ToArray();
    
    List<int> list = new List<int>();
    for (int i = 0; i < elements; i++) list.Add(i);
    
    int position = 0;
    int skipSize = 0;
    foreach (int l in lengths)
    {
        List<int> tempList = new List<int>(list);
        
        for (int i = 0; i < l; i++)
        {
            int sourcePos =  (position+l-1-i) %list.Count;
            int replacePos = (position + i) % list.Count;
            tempList[replacePos] = list[sourcePos];
        }
        
        list = tempList;
        position += l;
        position += skipSize;
        skipSize++;
    }
    
    $"P1: {list[0] * list[1]}".Dump();
}