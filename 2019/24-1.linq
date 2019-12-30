<Query Kind="Program" />

static void Main(string[] args)
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("24.txt");

    int xLength = inputLines[0].Length;
    int yLength = inputLines.Length;
    char[,] grid = new char[yLength, xLength];
    for (int y = 0; y < yLength; y++)
    {
        for (int x = 0; x < xLength; x++)
        {
            char c = inputLines[y][x];
            grid[x,y] = c;
        }
    }

    HashSet<long> ratings = new HashSet<long>();

    while (true)
    {
        char[,] newGrid = new char[yLength, xLength];
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                char c = grid[x,y];
                char up = '.', down = '.', right = '.', left = '.';
                if (y > 0) up = grid[x, y-1];
                if (y < yLength-1) down = grid[x, y+1];
                if (x > 0) left = grid[x-1, y];
                if (x < xLength-1) right = grid[x+1, y];

                int adjacentBugs = (new char[] { up, down, left, right }).Count(n => n == '#');
                if (c == '#')
                {
                    if (adjacentBugs != 1)
                        c = '.';
                }
                else
                {
                    if (adjacentBugs >= 1 && adjacentBugs <= 2)
                        c = '#';
                }
                newGrid[x,y] = c;
            }
        }

        grid = newGrid;
        long diversity = CalculateBiodiversity();
        if (ratings.Contains(diversity))
        {
            Console.WriteLine(diversity);
            return;
        }
        else
        {
            ratings.Add(diversity);
        }
    }

    long CalculateBiodiversity()
    {
        long biodiversity = 0;
        int counter = 1;
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                char c = grid[x,y];
                if (c == '#') biodiversity += counter;
                counter *= 2;
            }
        }
        return biodiversity;
    }
}