<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("4.txt");
	
	int counter = 0;
	
	Dictionary<string, string> fields = new Dictionary<string, string>();
	foreach (var s in strings)
	{
		if (s == null || s.Length == 0)
		{
			if (Contains("byr") && InIntRange("byr", 1920, 2002) &&
			    Contains("iyr") && InIntRange("iyr", 2010, 2020) &&
				Contains("eyr") && InIntRange("eyr", 2020, 2030) &&
				Contains("hgt") && ValidHeight() &&
				Contains("hcl") && ValidHairColor() &&
				Contains("ecl") && ValidEyeColor() &&
				Contains("pid") && ValidPassportId())
				counter++;
				
			fields = new Dictionary<string, string>();
		}
		else
		{
			string[] split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			foreach (var sp in split)
			{
				string[] split2 = sp.Split(':', StringSplitOptions.RemoveEmptyEntries);
				fields.Add(split2[0], split2[1]);
			}
		}
	}

	$"P2: {counter}".Dump();
	
	bool Contains(string field)
	{
		return fields.ContainsKey(field);
	}
	
	bool InIntRange(string field, int lower, int upper)
	{
		int value = 0;
		bool valid = int.TryParse(fields[field], out value);
		return valid && value >= lower && value <= upper;
	}
	
	bool ValidHeight()
	{
		string stringValue = fields["hgt"];
		string numberPart = stringValue.Substring(0, stringValue.Length - 2);
		string measurement = stringValue.Substring(stringValue.Length - 2);
		int value = 0;
		bool valid = int.TryParse(numberPart, out value);
		
		if (measurement == "cm")
			return valid && value >= 150 && value <= 193;
		else return valid && value >= 59 && value <= 76;
	}
	
	bool ValidHairColor()
	{
		string stringValue = fields["hcl"];
		if (stringValue.Length == 7 && stringValue[0] == '#' &&
			stringValue.Substring(1).All(c => char.IsDigit(c) || c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e'	 || c == 'f'))
			return true;
		return false;
		
	}
	
	bool ValidEyeColor()
	{
		string sv = fields["ecl"];
		string[] colors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
		return colors.Contains(sv);
	}
	
	bool ValidPassportId()
	{
		string stringValue = fields["pid"];
		return stringValue.Length == 9 && stringValue.All(c => char.IsNumber(c));
	}
}