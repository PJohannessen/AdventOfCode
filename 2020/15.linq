<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string input = Utils.ParseStrings("15.txt").First();
	int[] startingNumbers = input.Split(',').Select(n => int.Parse(n)).ToArray();
	Dictionary<int, (int, int)> lastSeens = new();
	
	for (int i = 0; i < startingNumbers.Length; i++)
	{
		lastSeens.Add(startingNumbers[i], (-1, i+1));
	}
	
	int lastSeen = startingNumbers.Last();
	int count = startingNumbers.Length;
	
	while (count < 30000000)
	{
		int next;
		if (lastSeens[lastSeen].Item1 < 0)
		{
			next = 0;
		}
		else
		{
			next = count-lastSeens[lastSeen].Item1;
		}
		
		count++;
		int n = lastSeens.ContainsKey(next) ? lastSeens[next].Item2 : -1;
		lastSeens[next] = (n, count);
		lastSeen = next;
		

		if (count == 2020) $"P1: {lastSeen}".Dump();
		else if (count == 30000000) $"P2: {lastSeen}".Dump();
	}
}