<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var numbers = Utils.ParseInts("1.txt");
	for (int i = 0; i < numbers.Length; i++)
	{
		for (int j = i + 1; j < numbers.Length; j++)
		{
			if (numbers[i] + numbers[j] == 2020)
				$"P1: {numbers[i] * numbers[j]}".Dump();
				
			for (int k = j + 1; k < numbers.Length; k++)
			{	
				if (numbers[i] + numbers[j] + numbers[k] == 2020)
					$"P2: {numbers[i] * numbers[j] * numbers[k]}".Dump();
			}
		}
	}
}