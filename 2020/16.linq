<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string[] inputStrings = Utils.ParseStrings("16.txt", true);
	List<Rule> rules = new();
	List<int[]> nearbyTickets = new();
	List<List<string>> validRules = new();
	List<long> myTicket = new();
	List<string> resolvedRules = new();
	Section section = Section.Rules;
	
	long p1 = 0;
	long p2 = 1;
	
	foreach (var s in inputStrings)
	{
		if (s == "your ticket:") section = Section.MyTicket;
		else if (s == "nearby tickets:") section = Section.NearbyTickets;
		else
		{
			if (section == Section.Rules)
			{
				string[] parts = s.Split(new[] { ": ", " or ", "-" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
				string field = parts[0];
				int[] ranges = parts[1..].Select(n => int.Parse(n)).ToArray();
				rules.Add(new Rule{Field = field, LowerRange1 = ranges[0], UpperRange1 = ranges[1], LowerRange2 = ranges[2], UpperRange2 = ranges[3]});
			}
			else if (section == Section.MyTicket)
			{
				myTicket = s.Split(',').Select(n => long.Parse(n)).ToList();
			}
			else if (section == Section.NearbyTickets)
			{
				int[] values = s.Split(',').Select(n => int.Parse(n)).ToArray();
				bool valid = true;
				for (int i = 0; i < values.Length; i++)
				{
					int value = values[i];
					if (rules.All(r => (value < r.LowerRange1 || value > r.UpperRange1) && (value < r.LowerRange1 || value > r.UpperRange2)))
					{
						valid = false;
						p1 += value;
					}
				}
				if (valid) nearbyTickets.Add(values);
			}
		}
	}
	
	for (int i = 0; i < nearbyTickets[0].Length; i++)
	{
		List<string> vr = new List<string>();
		int[] columnValues = nearbyTickets.Select(r => r[i]).ToArray();
		foreach (var r in rules)
		{
			if (columnValues.All(v => (v >= r.LowerRange1 && v <= r.UpperRange1) || (v >= r.LowerRange2 && v <= r.UpperRange2)))
			{
				vr.Add(r.Field);
			}
		}
		validRules.Add(vr);
	}
	
	while (validRules.Any(r => r.Count > 1))
	{
		var singles = validRules.First(vr => vr.Count == 1 && !resolvedRules.Contains(vr[0]));
		string field = singles.Single();
		foreach (var l in validRules.Where(vr => vr.Count > 1 && vr.Contains(field)))
		{
			l.Remove(field);
		}
		resolvedRules.Add(field);
	}
	
	for (int i = 0; i < validRules.Count; i++)
	{
		string field = validRules[i].Single();
		if (field.StartsWith("departure"))
		{
			p2 *= myTicket[i];
		}
	}
	
	p1.Dump("Part 1:");
	p2.Dump("Part 2:");
}

public enum Section
{
	Rules,
	MyTicket,
	NearbyTickets
}

public record Rule
{
	public string Field { get; init; }
	public int LowerRange1 { get; init; }
	public int UpperRange1 { get; init; }
	public int LowerRange2 { get; init; }
	public int UpperRange2 { get; init; }
}