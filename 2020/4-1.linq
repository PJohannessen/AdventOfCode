<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("4.txt");
	
	int counter = 0;
	
	List<string> fields = new List<string>();
	foreach (var s in strings)
	{
		if (s == null || s.Length == 0)
		{
			if (Contains("byr") &&
			    Contains("iyr") &&
				Contains("eyr") &&
				Contains("hgt") &&
				Contains("hcl") &&
				Contains("ecl") &&
				Contains("pid"))
				counter++;
				
			fields = new List<string>();
		}
		else
		{
			string[] split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			foreach (var sp in split)
			{
				string[] split2 = sp.Split(':', StringSplitOptions.RemoveEmptyEntries);
				fields.Add(split2[0]);
			}
		}
	}

	$"P1: {counter}".Dump();
	
	bool Contains(string field)
	{
		return fields.Contains(field);
	}
}