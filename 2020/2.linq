<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("2.txt");
	
	int p1 = 0;
	int p2 = 0;
	
	foreach (string s in strings)
	{
		string[] parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		string[] rangeParts = parts[0].Split('-', StringSplitOptions.RemoveEmptyEntries);
		
		int lower = int.Parse(rangeParts[0]);
		int upper = int.Parse(rangeParts[1]);
		char c = parts[1][0];
		string password = parts[2];

		var occurences = password.Count(l => l == c);
		
		if (occurences >= lower && occurences <= upper) p1++;
		
		if (password[lower-1] == c && password[upper-1] != c ||
			password[lower-1] != c && password[upper-1] == c) p2++;
	}

	$"P1: {p1}".Dump();
	$"P2: {p2}".Dump();
}