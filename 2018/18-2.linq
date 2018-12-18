<Query Kind="Program" />

int width;
int height;
char[,] grid;
void Main()
{
    // Results in an eventual pattern that repeats every 84 minutes, so find which one will match 1_000_000_000
    ShowPattern();
    
    // Matches up perfectly with 1,000 - so take that total
    int value = 1000000000;
    while (value > 1000)
    {
        value -= 84;
    }
    value.Dump();
}

void ShowPattern()
{
    int minutes = 1000;
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("18.txt");
    width = inputLines[0].Length;
    height = inputLines.Length;
    grid = new char[width, height];
    for (int j = 0; j < height; j++)
    {
        for (int i = 0; i < width; i++)
        {
            grid[i, j] = inputLines[j][i];
        }
    }

    Calculate(0);
    for (int m = 1; m <= minutes; m++)
    {
        char[,] newGrid = new char[width, height];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (grid[i, j] == '.')
                {
                    if (GetSurrounding(grid, i, j, '|') >= 3) newGrid[i, j] = '|';
                    else newGrid[i, j] = '.';
                }
                else if (grid[i, j] == '|')
                {
                    if (GetSurrounding(grid, i, j, '#') >= 3) newGrid[i, j] = '#';
                    else newGrid[i, j] = '|';
                }
                else if (grid[i, j] == '#')
                {
                    if (GetSurrounding(grid, i, j, '#') >= 1 && GetSurrounding(grid, i, j, '|') >= 1) newGrid[i, j] = '#';
                    else newGrid[i, j] = '.';
                }
            }
        }
        grid = newGrid;
        Calculate(m);
    }
}

void Calculate(int minute)
{
    int trees = 0;
    int lumberyards = 0;
    for (int j = 0; j < height; j++)
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, j] == '|') trees++;
            else if (grid[i, j] == '#') lumberyards++;
        }
    }
    string.Format("{0}, {1}", minute, trees * lumberyards).Dump();
}

int GetSurrounding(char[,] grid, int x, int y, char c)
{
    List<char> surrounding = new List<char>();
    if (x-1 >= 0 && y-1 >= 0) surrounding.Add(grid[x-1, y-1]);
    if (y-1 >= 0) surrounding.Add(grid[x, y-1]);
    if (x+1 < width && y-1 >= 0) surrounding.Add(grid[x+1, y-1]);
    if (x-1 >= 0) surrounding.Add(grid[x-1, y]);
    if (x+1 < width) surrounding.Add(grid[x+1, y]);
    if (x-1 >= 0 && y+1 < height) surrounding.Add(grid[x-1, y+1]);
    if (y+1 < height) surrounding.Add(grid[x, y+1]);
    if (x+1 < width && y+1 < height) surrounding.Add(grid[x+1, y+1]);
    return surrounding.Where(c2 => c2 == c).Count();
}

void Print()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            Console.Write(grid[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}