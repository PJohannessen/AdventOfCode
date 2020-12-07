<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var inputStrings = Utils.ParseStrings("7.txt");
	
	Dictionary<string, List<KeyValuePair<string, int>>> lookup = new ();
	foreach (var bagString in inputStrings)
	{
		List<KeyValuePair<string, int>> bagsInside = new ();
		string[] bagsStringSplit = bagString.Split(new[] { " contain ", ", ", "." }, StringSplitOptions.RemoveEmptyEntries);
		string bagType = bagsStringSplit[0];
		bagType = bagType.Replace(" bags", "").Replace(" bag", "");
		for (int i = 1; i < bagsStringSplit.Length; i++)
		{
			if (bagString.Contains("no other bags")) continue;
			string currentBagString = bagsStringSplit[i];
			string[] currentBagStringSplit = currentBagString.Split(" ", StringSplitOptions.None);
			int numberOfBags = int.Parse(currentBagStringSplit[0]);
			string bag = $"{currentBagStringSplit[1]} {currentBagStringSplit[2]}";
			bagsInside.Add(new KeyValuePair<string, int>(bag, numberOfBags));
		}
		lookup.Add(bagType, bagsInside);
	}
		
	string targetBag = "shiny gold";
	
	int bagsWithTargetBag = lookup.Sum(bag => BagHasTargetBag(bag.Value, targetBag) ? 1 : 0);
	int bagsInTargetBag = CountBags(targetBag) - 1;

	$"P1: {bagsWithTargetBag}".Dump();
	$"P2: {bagsInTargetBag}".Dump();

	bool BagHasTargetBag(List<KeyValuePair<string, int>> bags, string target)
	{
		if (bags.Any(s => s.Key == target)) return true;
		return bags.Any(bag => lookup.ContainsKey(bag.Key) && BagHasTargetBag(lookup[bag.Key], target));
	}

	int CountBags(string target)
	{
		return lookup[target].Count == 0 ?
			1 :
			1 + lookup[target].Sum(bag => (bag.Value * CountBags(bag.Key)));
	}
}