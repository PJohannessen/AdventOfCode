<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("14.txt");
	
	Dictionary<long, long> memory1 = new();
	Dictionary<long, long> memory2 = new();
	string maskString = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
	List<string> masks = new();

	foreach (var s in strings)
	{
		if (s.StartsWith("mask"))
		{
			string[] maskParts = s.Split(new[] { "mask = " }, StringSplitOptions.RemoveEmptyEntries);
			maskString = maskParts[0];
			masks = Process(maskString).ToList();
		}
		else
		{
			string[] memParts = s.Split(new[] { "mem[", "] = " }, StringSplitOptions.RemoveEmptyEntries);
			long m1 = long.Parse(memParts[0]);
			long m2 = long.Parse(memParts[1]);

			Span<char> m2String = new Span<char>(Convert.ToString(m2, 2).PadLeft(36, '0').ToCharArray());
			for (int i = maskString.Length - 1; i >= 0; i--)
			{
				if (maskString[i] == 'X') continue;
				m2String[i] = maskString[i];
			}
			memory1[m1] = Convert.ToInt64(new string(m2String), 2);

			foreach (var m in masks)
			{
				Span<char> m1String = new Span<char>(Convert.ToString(m1, 2).PadLeft(36, '0').ToCharArray());
				for (int i = 0; i < m.Length; i++)
				{
					if (maskString[i] == 'X') m1String[i] = m[i];
					else if (m[i] == '1') m1String[i] = '1';
				}
				long address = Convert.ToInt64(new string(m1String), 2);
				memory2[address] = m2;
			}
		}
	}
	
	$"P1: {memory1.Values.Sum(v => v)}".Dump();
	$"P2: {memory2.Values.Sum(v => v)}".Dump();
}

IEnumerable<string> Process(string input)
{
	if (input.Any(c => c == 'X'))
	{
		int i = input.IndexOf('X');
		Span<char> s0 = new Span<char>(input.ToCharArray());
		Span<char> s1 = new Span<char>(input.ToCharArray());
		s0[i] = '0';
		s1[i] = '1';
		string s0String = new string(s0);
		string s1String = new string(s1);
		foreach (var s in Process(s0String)) yield return s;
		foreach (var s in Process(s1String)) yield return s;
	}
	else
	{
		yield return input;
	}
}