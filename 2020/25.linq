<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	long[] inputs = Utils.ParseLongs("25.txt");
	long cardPublicKey = inputs[0], doorPublicKey = inputs[1];
	
	const long subject = 7;
	long value = 1, a = 0, b = 0;
	
	for (long i = 1; i < long.MaxValue; i++)
	{
		value *= subject;
		value = value % 20201227;
		if (value == cardPublicKey) a = i;
		if (value == doorPublicKey) b = i;
		
		if (a > 0 && b > 0) break;
	}
	
	value = 1;
	for (long i = 1; i <= b; i++)
	{
		value *= cardPublicKey;
		value = value % 20201227;
	}
	
	value.Dump();
}