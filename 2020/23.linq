<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string inputString = Utils.ParseStrings("23.txt").First();
	
	var p1Node = Solve(inputString, 100, inputString.Length);
	var p2Node = Solve(inputString, 10_000_000, 1_000_000);
	
	p1Node = Next(p1Node);
	StringBuilder sb = new();
	while (p1Node.Value != 1)
	{
		sb.Append(p1Node.Value);
		p1Node = Next(p1Node);
	}
	sb.ToString().Dump("P1");
	
	(Next(p2Node).Value * Next(Next(p2Node)).Value).Dump("P2");
}

LinkedListNode<long> Solve(string inputString, long totalMoves, long totalCups)
{
	long[] inputs = inputString.Select(c => long.Parse(c.ToString())).ToArray();
	long max = inputs.Max();
	Dictionary<long, LinkedListNode<long>> lookup = new();

	LinkedList<long> linkedList = new();
	foreach (var n in inputs)
	{
		var node = linkedList.AddLast(n);
		lookup.Add(n, node);
	}
	for (long i = max + 1; linkedList.Count < totalCups; i++)
	{
		var node = linkedList.AddLast(i);
		lookup.Add(i, node);
	}
	
	max = linkedList.Count;

	var current = linkedList.First;
	for (int move = 1; move <= totalMoves; move++)
	{
		LinkedListNode<long>[] removed = new LinkedListNode<long>[3];
		removed[0] = Next(current);
		linkedList.Remove(Next(current));
		removed[1] = Next(current);
		linkedList.Remove(Next(current));
		removed[2] = Next(current);
		linkedList.Remove(Next(current));

		long target = current.Value - 1;
		while (removed.Select(lln => lln.Value).Contains(target) || target <= 0)
		{
			if (target == 0) target = max;
			else target--;
		}

		var targetCup = lookup[target];
		linkedList.AddAfter(targetCup, removed[2]);
		linkedList.AddAfter(targetCup, removed[1]);
		linkedList.AddAfter(targetCup, removed[0]);
		current = Next(current);
	}

	return lookup[1];
}

LinkedListNode<long> Next(LinkedListNode<long> current)
{
	if (current.Next != null) return current.Next;
	return current.List.First;
}