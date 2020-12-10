<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var numbers = Utils.ParseInts("10.txt");
	
	var ordered = numbers.Concat(new[] { 0, numbers.Max() + 3 }).OrderBy(n => n).ToList();
	
	int ones = 0;
	int threes = 0;
	for (int i = 1; i < ordered.Count; i++)
	{
		int diff = ordered[i] - ordered[i-1];
		if (diff == 1) ones++;
		else if (diff == 3) threes++;
	}
	
	long[] memoized = new long[ordered.Count];
	memoized[0] = 1;
	
	for (var i = 1; i < ordered.Count; i++)
	{
		long count = 0;
		for (var connected = i-1; connected >= 0 && ordered[i] - ordered[connected] <= 3; connected--)
		{
			count += memoized[connected];
		}
		memoized[i] = count;
	}

	$"P1: {ones * threes}".Dump();
	$"P2: {memoized[ordered.Count - 1]}".Dump();
}