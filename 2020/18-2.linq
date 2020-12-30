<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string[] inputStrings = Utils.ParseStrings("18.txt");
	long sum = 0;

	foreach (var s in inputStrings)
	{
		var things = Parse(s);
		while (things.Count > 3)
		{
			bool currentDeepest = false;
			int current = 1;
			int deepest = 1;
			int processFrom = 0;
			int processTo = things.Count;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].Type == ThingType.Open)
				{
					current++;
					if (current > deepest)
					{
						currentDeepest = true;
						deepest = current;
						processFrom = i;
					}
				}
				else if (things[i].Type == ThingType.Close)
				{
					if (currentDeepest)
					{
						processTo = i;
					}
					currentDeepest = false;
					current--;
				}
			}

			var slicedThings = things.GetRange(processFrom + 1, processTo - processFrom - 1);
			while (slicedThings.Any(t => t.Type == ThingType.Add))
			{
				int i = slicedThings.IndexOf(new Thing { Type = ThingType.Add });
				long result = slicedThings[i - 1].Value + slicedThings[i + 1].Value;
				slicedThings[i] = new Thing { Type = ThingType.Number, Value = result };
				slicedThings.RemoveAt(i + 1);
				slicedThings.RemoveAt(i - 1);
			}
			while (slicedThings.Any(t => t.Type == ThingType.Multiply))
			{
				int i = slicedThings.IndexOf(new Thing { Type = ThingType.Multiply });
				long result = slicedThings[i - 1].Value * slicedThings[i + 1].Value;
				slicedThings[i] = new Thing { Type = ThingType.Number, Value = result };
				slicedThings.RemoveAt(i + 1);
				slicedThings.RemoveAt(i - 1);
			}
			if (slicedThings.Count != 1) throw new Exception("Something went wrong");
			things[processFrom] = slicedThings.Single();
			things.RemoveRange(processFrom + 1, processTo - processFrom);
		}
		sum += things[0].Value;
	}
	sum.Dump();

	List<Thing> Parse(string s)
	{
		List<Thing> things = new();
		StringBuilder sb = new();
		things.Add(new Thing { Type = ThingType.Open });
		for (int i = 0; i < s.Length; i++)
		{
			switch (s[i])
			{
				case '(':
					things.Add(new Thing { Type = ThingType.Open });
					break;
				case ')':
					if (sb.Length > 0)
					{
						things.Add(new Thing { Type = ThingType.Number, Value = long.Parse(sb.ToString()) });
						sb = new StringBuilder();
					}
					things.Add(new Thing { Type = ThingType.Close });
					break;
				case char digit when char.IsDigit(digit):
					sb.Append(digit);
					break;
				case '+':
					things.Add(new Thing { Type = ThingType.Add });
					i++;
					break;
				case '*':
					things.Add(new Thing { Type = ThingType.Multiply });
					i++;
					break;
				case ' ':
					if (sb.Length > 0)
					{
						things.Add(new Thing { Type = ThingType.Number, Value = long.Parse(sb.ToString()) });
						sb = new StringBuilder();
					}
					break;
				default:
					throw new NotImplementedException();
			}
		}
		if (sb.Length > 0)
		{
			things.Add(new Thing { Type = ThingType.Number, Value = long.Parse(sb.ToString()) });
		}
		things.Add(new Thing { Type = ThingType.Close });
		return things;
	}

	(long, int) Process(string s, int index)
	{
		StringBuilder sb = new();
		Op op = Op.None;
		long current = 0;
		for (int i = index; i < s.Length; i++)
		{
			switch (s[i])
			{
				case '(':
					(long evaled, int finishedIndex) = Process(s, i + 1);
					if (op == Op.None) current = evaled;
					else if (op == Op.Add) current += evaled;
					else if (op == Op.Multiply) current *= evaled;
					i = finishedIndex;
					break;
				case ')':
					if (op == Op.None && sb.Length > 0)
					{
						current = long.Parse(sb.ToString());
					}
					else if (op == Op.Add && sb.Length > 0)
					{
						current += long.Parse(sb.ToString());
					}
					else if (op == Op.Multiply && sb.Length > 0)
					{
						current *= long.Parse(sb.ToString());
					}
					sb = new StringBuilder();
					return (current, i);
				case char digit when char.IsDigit(digit):
					sb.Append(digit);
					break;
				case '+':
					op = Op.Add;
					break;
				case '*':
					op = Op.Multiply;
					break;
				case ' ':
					if (op == Op.None && sb.Length > 0)
					{
						current = long.Parse(sb.ToString());
					}
					else if (op == Op.Add && sb.Length > 0)
					{
						current += long.Parse(sb.ToString());
					}
					else if (op == Op.Multiply && sb.Length > 0)
					{
						current *= long.Parse(sb.ToString());
					}
					sb = new StringBuilder();
					break;
				default:
					throw new NotImplementedException();
			}
		}
		if (op == Op.None && sb.Length > 0)
		{
			current = long.Parse(sb.ToString());
		}
		else if (op == Op.Add && sb.Length > 0)
		{
			current += long.Parse(sb.ToString());
		}
		else if (op == Op.Multiply && sb.Length > 0)
		{
			current *= long.Parse(sb.ToString());
		}
		return (current, s.Length);
	}
}

public record Thing
{
	public ThingType Type { get; init; }
	public long Value { get; init; }
}

public enum ThingType
{
	Number,
	Add,
	Multiply,
	Open,
	Close
}

public enum Op
{
	None,
	Add,
	Multiply,
}