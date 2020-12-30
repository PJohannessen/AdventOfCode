<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string[] inputStrings = Utils.ParseStrings("24.txt");
	
	HashSet<(int X, int Y, int Z)> flipped = new();
	
	foreach (var s in inputStrings)
	{
		(int X, int Y, int Z) current = (0, 0, 0);
		for (int i = 0; i < s.Length; i++)
		{
			Direction d;
			if (s[i] == 'e')
				d = Direction.e;
			else if (s[i] == 'w')
				d = Direction.w;
			else
			{
				if (s[i] == 's' && s[i + 1] == 'e')
					d = Direction.se;
				else if (s[i] == 's' && s[i + 1] == 'w')
					d = Direction.sw;
				else if (s[i] == 'n' && s[i + 1] == 'w')
					d = Direction.nw;
				else if (s[i] == 'n' && s[i + 1] == 'e')
					d = Direction.ne;
				else throw new Exception();
					
				i++;
			}

			current = d switch
			{
				Direction.e => (current.X+1, current.Y-1, current.Z),
				Direction.se => (current.X, current.Y-1, current.Z+1),
				Direction.sw => (current.X-1, current.Y, current.Z+1),
				Direction.w => (current.X-1, current.Y+1, current.Z),
				Direction.nw => (current.X, current.Y+1, current.Z-1),
				Direction.ne => (current.X+1, current.Y, current.Z-1),
				_ => throw new Exception()
			};
		}
		if (flipped.Contains(current)) flipped.Remove(current);
		else flipped.Add(current);
	}
	
	flipped.Count.Dump("P1");
	
	for (int day = 1; day <= 100; day++)
	{
		HashSet<(int X, int Y, int Z)> newFlipped = new();

		var points = flipped.SelectMany(p => new (int X, int Y, int Z)[] {
			p,
			(p.X + 1, p.Y - 1, p.Z),
			(p.X, p.Y - 1, p.Z + 1),
			(p.X - 1, p.Y, p.Z + 1),
			(p.X - 1, p.Y + 1, p.Z),
			(p.X, p.Y + 1, p.Z - 1),
			(p.X + 1, p.Y, p.Z - 1)
		}).Distinct();
		
		foreach (var point in points)
		{
			if (point.X + point.Y + point.Z != 0) throw new Exception();
			bool isFlipped = flipped.Contains(point);
			var adjacent = flipped.Where(p2 => Distance(point, p2) == 1);
			int adjacentFlipped = adjacent.Count();
			if (isFlipped && (adjacentFlipped == 0 || adjacentFlipped > 2))
			{
				isFlipped = false;
			}
			else if (!isFlipped && adjacentFlipped == 2)
			{
				isFlipped = true;
			}

			if (isFlipped && !newFlipped.Contains(point))
			{
				newFlipped.Add(point);
			}
		}

		flipped = newFlipped;
	}
	
	flipped.Count.Dump("P2");
}

int Distance((int X, int Y, int Z) a, (int X, int Y, int Z) b)
{
	int xDiff = Math.Abs(a.X - b.X);
	int yDiff = Math.Abs(a.Y - b.Y);
	int zDiff = Math.Abs(a.Z - b.Z);
	int distance = Math.Max(xDiff, Math.Max(yDiff, zDiff));
	return distance;
	
}

enum Direction
{
	e,
	se,
	sw,
	w,
	nw,
	ne
}