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
		(long n, int finishedIndex) = Process(s, 0);
		sum += n;
	}
	sum.Dump();
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
				(long evaled, int finishedIndex) = Process(s, i+1);
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

public enum Op
{
	None,
	Add,
	Multiply,
}

public enum Reading
{
	FirstNumber,
	Operation,
	OpenBracket,
	CloseBracket
}