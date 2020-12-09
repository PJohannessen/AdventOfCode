<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var numbers = Utils.ParseLongs("9.txt");
	
	int preamble = 25;
	long p1 = P1();
	long p2 = P2();
	
	long P1()
	{
		for (int i = preamble; i < numbers.Length; i++)
		{
			long target = numbers[i];
			long[] selection = numbers[(i - preamble)..(i)];

			bool match = false;

			for (int j = 0; j < selection.Length; j++)
			{
				for (int k = j + 1; k < selection.Length; k++)
				{
					if (selection[j] + selection[k] == target) match = true;
				}
			}

			if (!match)
			{
				return target;
			}
		}
		return -1;
	}
	
	long P2()
	{
		for (int i = 0; i < numbers.Length; i++)
		{
			int j = i + 1;
			long[] range = numbers[i..j];
			long sum = range.Sum();
			while (sum <= p1)
			{
				if (sum == p1)
				{
					return range.Min() + range.Max();
				}

				j++;
				range = numbers[i..j];
				sum = range.Sum();
			}
		}
		return -1;
	}

	$"P1: {p1}".Dump();
	$"P2: {p2}".Dump();
}