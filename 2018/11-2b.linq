<Query Kind="Program" />

void Main()
{
    int serialNumber = 6878;
    int[,] grid = new int[300, 300];
    for (int x = 1; x <= 300; x++)
    {
        for (int y = 1; y <= 300; y++)
        {
            int rackId = x + 10;
            int powerLevel = (((rackId * y + serialNumber) * rackId) / 100 % 10) - 5;
            grid[(x - 1), (y - 1)] = powerLevel;
        }
    }
    int[,] sums = new int[300, 300];

    int topSize = 1;
    int topPower = 0;
    int topX = 0;
    int topY = 0;
    
    for (int size = 1; size <= 300; size++)
    {
        for (int x = 0; x <= 300 - size; x++)
        {
           for (int y = 0; y <= 300 - size; y++)
            {
                int currentPower = sums[x, y];
                for (int i = 1; i <= size; i++)
                {
                    currentPower += grid[x + i-1, y+size-1];
                    if (i != size) currentPower += grid[x+size-1, y + i-1];
                }
                if (currentPower > topPower)
                {
                    topPower = currentPower;
                    topX = x + 1;
                    topY = y + 1;
                    topSize = size;
                }
                sums[x,y] = currentPower;
            }
        }
    }

    string.Format("{0},{1},{2}", topX, topY, topSize).Dump();
}