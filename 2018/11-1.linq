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

    int topPower = 0;
    int topX = 0;
    int topY = 0;
    for (int x = 1; x <= 298; x++)
    {
        for (int y = 1; y <= 298; y++)
        {
            int currentPower = grid[x - 1, y - 1] + grid[x, y - 1] + grid[x + 1, y - 1] +
                               grid[x - 1, y] + grid[x, y] + grid[x + 1, y] +
                               grid[x - 1, y + 1] + grid[x, y + 1] + grid[x + 1, y + 1];
            if (currentPower > topPower)
            {
                topPower = currentPower;
                topX = x;
                topY = y;
            }
        }
    }
    string.Format("{0},{1}", topX, topY).Dump();
}
