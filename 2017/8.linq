<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("8.txt");

    Dictionary<string, int> registers = new Dictionary<string, int>();
    List<Instruction> instructions = new List<Instruction>();
    
    foreach (var line in inputLines)
    {
        var l = line.Split(' ').ToArray();
        instructions.Add(new Instruction(
            l[0], l[1], int.Parse(l[2]), l[4], l[5], int.Parse(l[6])
        ));
    }
    
    foreach (var register in instructions.Select(i => i.RegisterA).Union(instructions.Select(i => i.RegisterB)).Distinct())
    {
        registers.Add(register, 0);
    }
    
    int highest = 0;
    foreach (var i in instructions)
    {
        bool shouldRun = false;
        switch (i.Condition)
        {
            case ">":
                shouldRun = registers[i.RegisterB] > i.Value2;
                break;
            case ">=":
                shouldRun = registers[i.RegisterB] >= i.Value2;
                break;
            case "<":
                shouldRun = registers[i.RegisterB] < i.Value2;
                break;
            case "<=":
                shouldRun = registers[i.RegisterB] <= i.Value2;
                break;
            case "!=":
                shouldRun = registers[i.RegisterB] != i.Value2;
                break;
            default:
                shouldRun = registers[i.RegisterB] == i.Value2;
                break;
        }
        
        if (shouldRun) {
            if (i.Op == "inc") {
                registers[i.RegisterA] = registers[i.RegisterA] + i.Value1;
            } else if (i.Op == "dec") {
                registers[i.RegisterA] = registers[i.RegisterA] - i.Value1;
            }
        }
        int currentMax = registers.Values.Max();
        if (currentMax > highest) highest = currentMax;
    }
    $"P1: {registers.Values.Max()}".Dump();
    $"P2: {highest}".Dump();
}

public class Instruction
{
    public Instruction(string a, string op, int v1, string b, string condition, int value2)
    {
        RegisterA = a;
        Op = op;
        Value1 = v1;
        RegisterB = b;
        Condition = condition;
        Value2 = value2;
    }
    public string RegisterA { get; }
    public string Op { get; }
    public int Value1 { get; }
    public string RegisterB { get; }
    public string Condition { get; }
    public int Value2 { get; }
}