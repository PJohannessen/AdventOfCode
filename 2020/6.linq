<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var answerStrings = Utils.ParseStrings("6.txt");
	
	StringBuilder sb = new ();
	List<List<string>> groups = new ();
	List<string> answers = new ();
	foreach (var answerString in answerStrings)
	{
		if (answerString.Length > 0)
		{
			answers.Add(answerString);
			sb.Append(answerString);
		}
		else
		{
			string group = sb.ToString();
			groups.Add(answers);
			sb = new StringBuilder();
			answers = new List<string>();
		}
	}
	
	int p1 = groups.Sum(group => group.SelectMany(answer => answer).Distinct().Count());	
	int p2 = 0;
	for (char answer = 'a'; answer <= 'z'; answer++)
	{
		p2 += groups.Sum(group => group.All(answers => answers.Contains(answer)) ? 1 : 0);
	}

	$"P1: {p1}".Dump();
	$"P2: {p2}".Dump();
}