<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

public static int Size = 0;
public static List<char[,]> LargeGrids = new();
public const int MonsterWidth = 20;
public const int MonsterHeight = 3;
public char[,] MonsterGrid = new char[MonsterHeight,MonsterWidth];
public const int MonsterHashes = 15;
public static long P1 = -1;
public static int P2 = -1;

void Main()
{
	string[] inputStrings = Utils.ParseStrings("20.txt", true);
    var grids = ParseGrids(inputStrings);
    Size = (int)Math.Sqrt(grids.Count);
    MonsterGrid = new char[,] {
        { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.' },
        { '#', '.', '.', '.', '.', '#', '#', '.', '.', '.', '.', '#', '#', '.', '.', '.', '.', '#', '#', '#' },
        { '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '.' }
    };
    
    Solve(new (), grids);
    CountMonsters();

    $"P1: {P1}".Dump();
    $"P2: {P2}".Dump();
}

public int CountMonsters()
{
    int totalHashes = 0;
    for (int y = 0; y < (Size*8); y++)
    {
        for (int x = 0; x < (Size*8); x++)
        {
            if (LargeGrids[0][y, x] == '#') totalHashes++;
        }
    }
    
    int gridNumber = 1;
    foreach (var largeGrid in LargeGrids)
    {
        int monsters = 0;
        for (int y1 = 0; y1 < (Size*8)-MonsterHeight; y1++)
        {
            for (int x1 = 0; x1 < (Size*8)-MonsterWidth; x1++)
            {
                bool match = MonsterMatch(largeGrid, x1, y1);
                if (match)
                {
                    monsters++;
                }
            }
        }
        if (monsters > 0)
            P2 = totalHashes-(monsters*MonsterHashes);
        gridNumber++;
    }
    
    return 0;
    
    bool MonsterMatch(char[,] grid, int x1, int y1)
    {
        for (int y2 = y1; y2 < MonsterHeight+y1; y2++)
        {
            for (int x2 = x1; x2 < MonsterWidth+x1; x2++)
            {
                if (MonsterGrid[y2-y1, x2-x1] == '#' && grid[y2, x2] != '#') return false;
            }
        }
        
        return true;
    }
}

public void Solve(List<KeyValuePair<long, char[,]>> currentGrids, List<Grid> remainingGrids)
{
    for (int i = 0; i < remainingGrids.Count; i++)
    {
        var currentGrid = remainingGrids[i];
        for (int p = 0; p < currentGrid.Permutations.Count; p++)
        {
            var newCurrentGrids = new List<KeyValuePair<long, char[,]>>(currentGrids);
            newCurrentGrids.Add(new KeyValuePair<long, char[,]>(currentGrid.N, currentGrid.Permutations[p]));
            if (PermsAreValid(newCurrentGrids))
            {
                var newRemainingGrids = new List<Grid>(remainingGrids);
                newRemainingGrids.RemoveAt(i);
                if (newRemainingGrids.Count == 0)
                {
                    long p1 = (newCurrentGrids[0].Key *
                    newCurrentGrids[Size-1].Key *
                    newCurrentGrids[Size*Size-Size].Key *
                    newCurrentGrids[Size * Size - 1].Key);
                    P1 = p1;
                     
                    char[,] largeGrid = new char[8*Size,8*Size];
                    for (int g = 0; g < newCurrentGrids.Count; g++)
                    {
                        int plusX = (g % Size) * 8;
                        int plusY = (int)(Math.Floor((double)g / Size)) * 8;

                        for (int y = 0; y < 8; y++)
                        {
                            for (int x = 0; x < 8; x++)
                            {
                                largeGrid[y+plusY, x+plusX] = newCurrentGrids[g].Value[y+1, x+1];
                            }
                        }
                    }
                    LargeGrids.Add(largeGrid);
                }
                else
                {
                    Solve(newCurrentGrids, newRemainingGrids);
                }
            }
        }
    }
}

public bool PermsAreValid(List<KeyValuePair<long, char[,]>> perms)
{
    for (int p = 0; p < perms.Count; p++)
    {
        var perm = perms[p];
        if (p % Size != 0) // Check left
        {
            var otherPerm = perms[p - 1];
            for (int check = 0; check < 10; check++)
            {
                var c1 = otherPerm.Value[check, 9];
                var c2 = perm.Value[check, 0];
                if (c1 != c2) return false;
            }
        }
        if (p >= Size) // Check up
        {
            var otherPerm = perms[p - Size];
            for (int check = 0; check < 10; check++)
            {
                var c1 = otherPerm.Value[9, check];
                var c2 = perm.Value[0, check];
                if (c1 != c2) return false;
            }
        }
    }
    return true;
}

public List<Grid> ParseGrids(string[] inputStrings)
{
    List<Grid> grids = new();
    while (inputStrings.Length > 0)
    {
        long n = long.Parse(inputStrings[0].Split(new[] { "Tile ", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);
        string[] gridLines = inputStrings.Skip(1).Take(10).ToArray();

        List<char[,]> permutations = new();
        char[,] baseGrid = new char[10, 10];
        for (int i = 0; i <= 9; i++)
        {
            for (int j = 0; j <= 9; j++)
            {
                baseGrid[j, i] = gridLines[j][i];
            }
        }
        permutations.Add(baseGrid);
        for (int count = 1; count <= 3; count++)
        {
            char[,] nextGrid = new char[10, 10];
            for (int i = 9; i >= 0; i--)
            {
                for (int j = 0; j <= 9; j++)
                {
                    nextGrid[j, 9 - i] = permutations.Last()[i, j];
                }
            }
            permutations.Add(nextGrid);
        }
        for (int index = 0; index <= 3; index++)
        {
            char[,] nextGrid = new char[10, 10];
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    nextGrid[j, i] = permutations[index][i, j];
                }
            }
            permutations.Add(nextGrid);
        }

        var grid = new Grid { N = n, Permutations = permutations };
        grids.Add(grid);
        inputStrings = inputStrings[11..];
    }
    
    return grids;
}

public class Grid
{
    public long N { get; set;}
    public List<char[,]> Permutations { get; set; } 
}