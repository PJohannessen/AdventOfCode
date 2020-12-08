<Query Kind="Program" />

#load ".\shared"

void Main()
{
	var inputStrings = Utils.ParseStrings("8.txt");
	
	List<Instruction> defaultInstructions = inputStrings.Select(s =>
	{
		string[] split = s.Split(" ", StringSplitOptions.None);
		return new Instruction { Op = split[0], N = int.Parse(split[1]) };
	}).ToList();
	
	int p1;
	Process(defaultInstructions, out p1);
	$"P1: {p1}".Dump();
	
	for (int i = 0; i < defaultInstructions.Count; i++)
	{
		var modifiedInstructions = defaultInstructions.ToList();
		var modifiedInstruction = modifiedInstructions[i];
		if (modifiedInstruction.Op == "acc") continue;
		string newOp = modifiedInstruction.Op == "jmp" ? "nop" : "jmp";
		modifiedInstructions[i] = modifiedInstructions[i] with { Op = newOp };
		
		int p2;
		bool completed = Process(modifiedInstructions, out p2);
		if (completed)
		{
			$"P2: {p2}".Dump();
			return;
		}
	}
	
	bool Process(List<Instruction> currentInstructions, out int output)
	{
		HashSet<int> seenInstructions = new();
		output = 0;
		for (int i = 0; i >= 0 && i < currentInstructions.Count;)
		{
			if (!seenInstructions.Contains(i)) seenInstructions.Add(i);
			else return false;
			Instruction instruction = currentInstructions[i];
			switch (instruction.Op)
			{
				case "nop":
					i++;
					break;
				case "acc":
					i++;
					output += instruction.N;
					break;
				case "jmp":
					i += instruction.N;
					break;
			}
		}

		return true;
	}
}

public record Instruction
{
	public string Op { get; init; }
	public int N { get; init; }
}