<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string[] inputStrings = Utils.ParseStrings("17.txt");
	
	List<(int X, int Y, int Z)> points1 = new List<(int X, int Y, int Z)>();
	List<(int X, int Y, int Z, int W)> points2 = new List<(int X, int Y, int Z, int W)>();
	for (int y = 0; y < inputStrings.Length; y++)
	{
		for (int x = 0; x < inputStrings[y].Length; x++)
		{
			if (inputStrings[y][x] == '#')
			{
				points1.Add((x, y, 0));
				points2.Add((x, y, 0, 0));
			}
		}
	}
	
	for (int c = 1; c <= 6; c++)
	{
		List<(int X, int Y, int Z)> newPoints1 = new List<(int X, int Y, int Z)>();
		List<(int X, int Y, int Z, int W)> newPoints2 = new List<(int X, int Y, int Z, int W)>();
		for (int z = points1.Min(p => p.Z - 1); z <= points1.Max(p => p.Z + 1); z++)
		{
			for (int y = points1.Min(p => p.Y - 1); y <= points1.Max(p => p.Y + 1); y++)
			{
				for (int x = points1.Min(p => p.X - 1); x <= points1.Max(p => p.X + 1); x++)
				{
					{
						(int X, int Y, int Z) point1 = (x, y, z);
						bool active = points1.Contains(point1);
						int nearby = points1.Where(p => IsAdjacent(point1, p)).Count();
						if (active && (nearby == 2 || nearby == 3))
						{
							newPoints1.Add(point1);
						}
						else if (!active && nearby == 3)
						{
							newPoints1.Add(point1);
						}
					}
					
					for (int w = points2.Min(p => p.W - 1); w <= points2.Max(p => p.W + 1); w++)
					{
						(int X, int Y, int Z, int W) point2 = (x, y, z, w);
						bool active = points2.Contains(point2);
						int nearby = points2.Where(p => IsAdjacent(point2, p)).Count();
						if (active && (nearby == 2 || nearby == 3))
						{
							newPoints2.Add(point2);
						}
						else if (!active && nearby == 3)
						{
							newPoints2.Add(point2);
						}
					}

				}
			}
		}
		points1 = newPoints1;
		points2 = newPoints2;
	}
	
	points1.Count.Dump("P1");
	points2.Count.Dump("P2");
}

public static bool IsAdjacent((int X, int Y, int Z) a, (int X, int Y, int Z) b)
{
	if (a == b) return false;
	return Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1 && Math.Abs(a.Z - b.Z) <= 1;
}

public static bool IsAdjacent((int X, int Y, int Z, int W) a, (int X, int Y, int Z, int W) b)
{
	if (a == b) return false;
	return Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1 && Math.Abs(a.Z - b.Z) <= 1 && Math.Abs(a.W - b.W) <= 1;
}