<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var input = Utils.ParseStrings("11.txt");
	Solve(input, 4, false).Dump("P1");
	Solve(input, 5, true).Dump("P2");
}

int Solve(string[] inputLines, int threshold, bool keepLooking)
{
	int width;
	int height;
	char[,] grid;

	width = inputLines[0].Length;
	height = inputLines.Length;
	grid = new char[width, height];
	grid = new char[width, height];
	for (int j = 0; j < height; j++)
	{
		for (int i = 0; i < width; i++)
		{
			grid[i, j] = inputLines[j][i];
		}
	}

	HashSet<string> hashes = new();
	Dictionary<string, int> dict = new Dictionary<string, int>();

	while (true)
	{
		char[,] newGrid = new char[width, height];
		for (int j = 0; j < height; j++)
		{
			for (int i = 0; i < width; i++)
			{
				if (grid[i, j] == 'L')
				{
					int surroundingOn = GetSurrounding(grid, i, j, '#', keepLooking);
					if (surroundingOn == 0) newGrid[i, j] = '#';
					else newGrid[i, j] = 'L';
				}
				else if (grid[i, j] == '#')
				{
					int surroundingOn = GetSurrounding(grid, i, j, '#', keepLooking);
					if (surroundingOn >= threshold) newGrid[i, j] = 'L';
					else newGrid[i, j] = '#';
				}
				else
				{
					newGrid[i, j] = '.';
				}
			}
		}
		grid = newGrid;
		int occupied = 0;
		StringBuilder sb = new();
		for (int j = 0; j < height; j++)
		{
			for (int i = 0; i < width; i++)
			{
				if (grid[i, j] == '#') occupied++;
				sb.Append(grid[i, j]);
			}
		}
		var hash = sb.ToString();
		if (hashes.Contains(hash))
		{
			return occupied;
		}
		hashes.Add(hash);
	}

	int GetSurrounding(char[,] grid, int x, int y, char c, bool keepLooking)
	{
		List<char> surrounding = new List<char>();
		List<(int, int)> checks = new List<(int, int)> {
		(0, -1),
		(1, -1),
		(1, 0),
		(1, 1),
		(0, 1),
		(-1, 1),
		(-1, 0),
		(-1, -1)
	};

		for (int i = 0; i < checks.Count; i++)
		{
			int x2 = x + checks[i].Item1, y2 = y + checks[i].Item2;
			while (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height)
			{
				char c2 = grid[x2, y2];
				if (keepLooking && c2 == '.')
				{
					x2 = x2 + checks[i].Item1;
					y2 = y2 + checks[i].Item2;
					continue;
				}
				else surrounding.Add(c2);
				break;
			}
		}

		return surrounding.Where(c2 => c2 == c).Count();
	}
}
