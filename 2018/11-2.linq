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
            int powerLevel = rackId * y;
            powerLevel += serialNumber;
            powerLevel *= rackId;
            string powerLevelString = powerLevel.ToString();
            powerLevel = powerLevel >= 100 ? int.Parse(powerLevelString[powerLevelString.Length - 3].ToString()) : 0;
            powerLevel -= 5;
            grid[(x - 1), (y - 1)] = powerLevel;
        }
    }

    int topSize = 1;
    int topPower = 0;
    int topX = 0;
    int topY = 0;
    
    for (int size = 1; size <= 300; size++)
    {
        for (int x = 1; x <= 301 - size; x++)
        {
           for (int y = 1; y <= 301 - size; y++)
            {
                int currentPower = 0;
                for (int x2 = 1; x2 <= size; x2++)
                {
                    for (int y2 = 1; y2 <= size; y2++)
                    {
                        currentPower += grid[x - 2 + x2, y - 2 + y2];
                    }
                }
                if (currentPower > topPower)
                {
                    topPower = currentPower;
                    topX = x;
                    topY = y;
                    topSize = size;
                }
            }
        }
    }
    
    string.Format("{0},{1},{2}", topX, topY, topSize).Dump();
}