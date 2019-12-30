<Query Kind="Program" />

static int[][,] allGrids;
const int modifier = 100;
const int minutes = 200;
const int range = 250;

static void Main(string[] args)
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("24.txt");
    int xLength = inputLines[0].Length;
    int yLength = inputLines.Length;
    allGrids = Enumerable.Range(0, range).Select(i => new int[yLength, xLength]).ToArray();

    int[,] centerGrid = new int[xLength, yLength];
    for (int y = 0; y < yLength; y++)
    {
        for (int x = 0; x < xLength; x++)
        {
            char c = inputLines[y][x];
            if (c == '?' || c == '.') centerGrid[x, y] = 0;
            else centerGrid[x, y] = 1;
        }
    }
    allGrids[0 + modifier] = centerGrid;

    for (int m = 1; m <= minutes; m++)
    {
        var newGrids = Enumerable.Range(0, range).Select(i => new int[yLength, xLength]).ToArray();
        for (int g = 0; g < allGrids.Length; g++)
        {
            int[,] currentGrid = allGrids[g];
            int[,] newGrid = new int[xLength, yLength];
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    int adjacentBugs = 0;

                    if (x == 2 && y == 2)
                    {
                        newGrid[x, y] = 0;
                        continue;
                    }

                    if (y == 0) adjacentBugs += GetForOuterTop(g);
                    else if (x == 2 && y == 3) adjacentBugs += GetForInnerBottom(g);
                    else adjacentBugs += currentGrid[x, y - 1];

                    if (y == 4) adjacentBugs += GetForOuterBottom(g);
                    else if (x == 2 && y == 1) adjacentBugs += GetForInnerTop(g);
                    else adjacentBugs += currentGrid[x, y + 1];

                    if (x == 0) adjacentBugs += GetForOuterLeft(g);
                    else if (x == 3 && y == 2) adjacentBugs += GetForInnerRight(g);
                    else adjacentBugs += currentGrid[x - 1, y];

                    if (x == 4) adjacentBugs += GetForOuterRight(g);
                    else if (x == 1 && y == 2) adjacentBugs += GetForInnerLeft(g);
                    else adjacentBugs += currentGrid[x + 1, y];

                    int current = currentGrid[x, y];
                    if (current == 1 && adjacentBugs != 1) current = 0;
                    else if (current == 0 && adjacentBugs >= 1 && adjacentBugs <= 2) current = 1;
                    newGrid[x, y] = current;
                }
            }
            newGrids[g] = newGrid;
        }

        allGrids = newGrids;
    }
    
    Calc();

    void Calc()
    {
        int totalBugs = 0;
        for (int g = 0; g < allGrids.Length; g++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    totalBugs += allGrids[g][x, y];
                }
            }
        }
        Console.WriteLine(totalBugs);
    }

    int GetForOuterLeft(int depth)
    {
        if (depth == 0) return 0;
        var grid = allGrids[depth - 1];
        return grid[1, 2];
    }

    int GetForOuterRight(int depth)
    {
        if (depth == 0) return 0;
        var grid = allGrids[depth - 1];
        return grid[3, 2];
    }

    int GetForOuterTop(int depth)
    {
        if (depth == 0) return 0;
        var grid = allGrids[depth - 1];
        return grid[2, 1];
    }

    int GetForOuterBottom(int depth)
    {
        if (depth == 0) return 0;
        var grid = allGrids[depth - 1];
        return grid[2, 3];
    }

    int GetForInnerLeft(int depth)
    {
        if (depth == range - 1) return 0;
        var grid = allGrids[depth + 1];
        return grid[0, 0] + grid[0, 1] + grid[0, 2] + grid[0, 3] + grid[0, 4];
    }

    int GetForInnerRight(int depth)
    {
        if (depth == range - 1) return 0;
        var grid = allGrids[depth + 1];
        return grid[4, 0] + grid[4, 1] + grid[4, 2] + grid[4, 3] + grid[4, 4];
    }

    int GetForInnerTop(int depth)
    {
        if (depth == range - 1) return 0;
        var grid = allGrids[depth + 1];
        return grid[0, 0] + grid[1, 0] + grid[2, 0] + grid[3, 0] + grid[4, 0];
    }

    int GetForInnerBottom(int depth)
    {
        if (depth == range - 1) return 0;
        var grid = allGrids[depth + 1];
        return grid[0, 4] + grid[1, 4] + grid[2, 4] + grid[3, 4] + grid[4, 4];
    }
}