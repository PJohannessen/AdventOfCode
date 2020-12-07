<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("3.txt");
	
	int height = strings.Length;
	int width = strings[0].Length;
	
	char[,] grid = new char[height, width];
	
	for (int y = 0; y < height; y++)
	{
		for (int x = 0; x < width; x++)
		{
			grid[y, x] = strings[y][x];
		}
	}
	
	$"P1: {CountTrees(3, 1)}".Dump();
	$"P2: {CountTrees(1, 1) * CountTrees(3, 1) * CountTrees(5, 1) * CountTrees(7, 1) * CountTrees(1, 2)}".Dump();
	
	long CountTrees(int right, int down)
	{
		long treeCount = 0;
		int y2 = down, x2 = right;
		while (y2 < height)
		{
			if (grid[y2, x2] == '#') treeCount++;
			y2 += down;
			x2 += right;

			x2 = x2 % width;
		}
		return treeCount;
	}	
}