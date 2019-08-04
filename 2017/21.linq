<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] instructions = File.ReadAllLines("21.txt");
    Dictionary<string, string> patterns = new Dictionary<string, string>();

    foreach (var i in instructions)
    {
        string[] parts = i.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        string p = parts[0];
        string r = parts[1].Replace("/", "");
        
        string p1, p2, p3, p4, p5, p6, p7, p8;
        p1 = p2 = p3 = p4 = p5 = p6 = p7 = p8 = null;
        
        if (p.Length == 5)
        {
            p1 = p.Replace("/", "");
            p2 = new string(new[] { p1[2], p1[0], p1[3], p1[1] });
            p3 = new string(new[] { p1[3], p1[2], p1[1], p1[0] });
            p4 = new string(new[] { p1[1], p1[3], p1[0], p1[2] });
            p5 = new string(new[] { p1[2], p1[3], p1[0], p1[1] });
            p6 = new string(new[] { p1[1], p1[0], p1[3], p1[2] });
            p7 = new string(new[] { p1[0], p1[2], p1[1], p1[3] });
            p8 = new string(new[] { p1[3], p1[1], p1[2], p1[0] });
        }
        else if (p.Length == 11)
        {
            p1 = p.Replace("/", "");
            p2 = new string(new[] { p1[6], p1[3], p1[0], p1[7], p1[4], p1[1], p1[8], p1[5], p1[2] });
            p3 = new string(new[] { p1[8], p1[7], p1[6], p1[5], p1[4], p1[3], p1[2], p1[1], p1[0] });
            p4 = new string(new[] { p1[2], p1[5], p1[8], p1[1], p1[4], p1[7], p1[0], p1[3], p1[6] });
            p5 = new string(new[] { p1[2], p1[1], p1[0], p1[5], p1[4], p1[3], p1[8], p1[7], p1[6] });
            p6 = new string(new[] { p1[6], p1[7], p1[8], p1[3], p1[4], p1[5], p1[0], p1[1], p1[2] });
            p7 = new string(new[] { p1[8], p1[5], p1[2], p1[7], p1[4], p1[1], p1[6], p1[3], p1[0] });
            p8 = new string(new[] { p1[0], p1[3], p1[6], p1[1], p1[4], p1[7], p1[2], p1[5], p1[8] });
        }
        
        patterns.Add(p1, r);
        if (!patterns.ContainsKey(p2)) patterns.Add(p2, r);
        if (!patterns.ContainsKey(p3)) patterns.Add(p3, r);
        if (!patterns.ContainsKey(p4)) patterns.Add(p4, r);
        if (!patterns.ContainsKey(p5)) patterns.Add(p5, r);
        if (!patterns.ContainsKey(p6)) patterns.Add(p6, r);
        if (!patterns.ContainsKey(p7)) patterns.Add(p7, r);
        if (!patterns.ContainsKey(p8)) patterns.Add(p8, r);
    }
    
    int currentSize = 3;
    int iterations = 18; // P1: 5
    char[,] grid = new char[3, 3] { { '.', '#', '.' }, { '.', '.', '#' }, { '#', '#', '#' } };

    for (int i = 1; i <= iterations; i++)
    {
        char[,] newGrid = new char[0,0];
        
        if (currentSize % 2 == 0)
        {
            int newSize = (currentSize + currentSize / 2);
            newGrid = new char[newSize, newSize];
            
            int y2 = 0;
            for (int y1 = 0; y1 < currentSize; y1 += 2)
            {
                int x2 = 0;
                for (int x1 = 0; x1 < currentSize; x1 += 2)
                {
                    string p = new string(new[] { grid[y1, x1], grid[y1, x1 + 1], grid[y1 + 1, x1], grid[y1 + 1, x1 + 1] });
                    string r = patterns[p];

                    newGrid[y2, x2] = r[0];
                    newGrid[y2, x2 + 1] = r[1];
                    newGrid[y2, x2 + 2] = r[2];
                    newGrid[y2 + 1, x2] = r[3];
                    newGrid[y2 + 1, x2 + 1] = r[4];
                    newGrid[y2 + 1, x2 + 2] = r[5];
                    newGrid[y2 + 2, x2] = r[6];
                    newGrid[y2 + 2, x2 + 1] = r[7];
                    newGrid[y2 + 2, x2 + 2] = r[8];

                    x2 += 3;
                }
                
                y2 += 3;
            }
            currentSize = newSize;
        }
        else
        {
            int newSize = (currentSize + currentSize / 3);
            newGrid = new char[newSize, newSize];
            
            int y2 = 0;
            for (int y1 = 0; y1 < currentSize; y1 += 3)
            {
                int x2 = 0;
                for (int x1 = 0; x1 < currentSize; x1 += 3)
                {
                    string p = new string(new[] { grid[y1, x1], grid[y1, x1+1], grid[y1, x1+2], grid[y1+1, x1], grid[y1+1, x1+1], grid[y1+1, x1+2], grid[y1+2, x1], grid[y1+2, x1+1], grid[y1+2, x1+2] });
                    string r = patterns[p];

                    newGrid[y2, x2] = r[0];
                    newGrid[y2, x2 + 1] = r[1];
                    newGrid[y2, x2 + 2] = r[2];
                    newGrid[y2, x2 + 3] = r[3];
                    newGrid[y2 + 1, x2] = r[4];
                    newGrid[y2 + 1, x2 + 1] = r[5];
                    newGrid[y2 + 1, x2 + 2] = r[6];
                    newGrid[y2 + 1, x2 + 3] = r[7];
                    newGrid[y2 + 2, x2] = r[8];
                    newGrid[y2 + 2, x2 + 1] = r[9];
                    newGrid[y2 + 2, x2 + 2] = r[10];
                    newGrid[y2 + 2, x2 + 3] = r[11];
                    newGrid[y2 + 3, x2] = r[12];
                    newGrid[y2 + 3, x2 + 1] = r[13];
                    newGrid[y2 + 3, x2 + 2] = r[14];
                    newGrid[y2 + 3, x2 + 3] = r[15];
                    
                    x2 += 4;
                }
                
                y2 += 4;
            }
            currentSize = newSize;
        }

        grid = newGrid;
    }

    int count = 0;
    for (int y = 0; y < currentSize; y++)
    {
        for (int x = 0; x < currentSize; x++)
        {
            if (grid[y,x] == '#') count++;
        }
    }
    
    count.Dump();
}