<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("5.txt");
	
	List<int> answers = new();
	foreach (var s in strings)
	{
		var binaryString = s.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
		var rowString = binaryString.Substring(0, 7);
		var columnString = binaryString.Substring(7);
		int row = Convert.ToInt32(rowString, 2);
		int column = Convert.ToInt32(columnString, 2);
		int id = row * 8 + column;
		answers.Add(id);
	}
	
	int max = answers.Max();
	int missing = Enumerable.Range(answers.Min(), answers.Max() - answers.Min()).First(id => !answers.Contains(id));

	$"P1: {max}".Dump();
	$"P2: {missing}".Dump();
}