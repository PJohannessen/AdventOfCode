<Query Kind="Program" />

Dictionary<string, Action<int, int, int>> opcodes = new Dictionary<string, Action<int, int, int>>();
Dictionary<int, Action> instructions = new Dictionary<int, Action>();
int[] registers = new int[6];
int instructionPointer = 0;
int instructionPointerRegister = 0;

void Main()
{
    opcodes.Add("addr", addr);
    opcodes.Add("addi", addi);
    opcodes.Add("mulr", mulr);
    opcodes.Add("muli", muli);
    opcodes.Add("banr", banr);
    opcodes.Add("bani", bani);
    opcodes.Add("borr", borr);
    opcodes.Add("bori", bori);
    opcodes.Add("setr", setr);
    opcodes.Add("seti", seti);
    opcodes.Add("gtir", gtir);
    opcodes.Add("gtri", gtri);
    opcodes.Add("gtrr", gtrr);
    opcodes.Add("eqir", eqir);
    opcodes.Add("eqri", eqri);
    opcodes.Add("eqrr", eqrr);
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("21.txt");

    int count = 0;
    foreach (var inputLine in inputLines)
    {
        if (inputLine.StartsWith("#"))
        {
            instructionPointerRegister = inputLine[4] - 48;
        }
        else
        {
            string[] instructionParts = inputLine.Split(' ').ToArray();
            opcodes[instructionParts[0]](int.Parse(instructionParts[1]), int.Parse(instructionParts[2]), int.Parse(instructionParts[3]));
            instructions.Add(count, () => opcodes[instructionParts[0]](int.Parse(instructionParts[1]), int.Parse(instructionParts[2]), int.Parse(instructionParts[3])));
            count++;
        }
    }

    while (true)
    {
        registers = new int[6];
        int instructionsExecuted = 0;
        instructionPointer = 0;
        while (true)
        {
            registers[instructionPointerRegister] = instructionPointer;
            instructions[instructionPointer]();
            instructionPointer = registers[instructionPointerRegister];
            instructionPointer++;
            instructionsExecuted++;
        }
    }
}

void addr(int a, int b, int c)
{
    registers[c] = registers[a] + registers[b];
}

void addi(int a, int b, int c)
{
    registers[c] = registers[a] + b;
}

void mulr(int a, int b, int c)
{
    registers[c] = registers[a] * registers[b];
}

void muli(int a, int b, int c)
{
    registers[c] = registers[a] * b;
}

void banr(int a, int b, int c)
{
    registers[c] = registers[a] & registers[b];
}

void bani(int a, int b, int c)
{
    registers[c] = registers[a] & b;
}

void borr(int a, int b, int c)
{
    registers[c] = registers[a] | registers[b];
}

void bori(int a, int b, int c)
{
    registers[c] = registers[a] | b;
}

void setr(int a, int b, int c)
{
    registers[c] = registers[a];
}

void seti(int a, int b, int c)
{
    registers[c] = a;
}

void gtir(int a, int b, int c)
{
    if (a > registers[b]) registers[c] = 1;
    else registers[c] = 0;
}

void gtri(int a, int b, int c)
{
    if (registers[a] > b) registers[c] = 1;
    else registers[c] = 0;
}

void gtrr(int a, int b, int c)
{
    if (registers[a] > registers[b]) registers[c] = 1;
    else registers[c] = 0;
}

void eqir(int a, int b, int c)
{
    if (a == registers[b]) registers[c] = 1;
    else registers[c] = 0;
}

void eqri(int a, int b, int c)
{
    if (registers[a] == b) registers[c] = 1;
    else registers[c] = 0;
}

HashSet<int> set = new HashSet<int>();

void eqrr(int a, int b, int c)
{
    if (set.Count == 1) $"P1: {registers[a]}".Dump();
    if (set.Contains(registers[a]))
    {
        $"P2: {set.Last()}".Dump();
        Console.Read();
    }
    set.Add(registers[a]);
    if (registers[a] == registers[b]) registers[c] = 1;
    else registers[c] = 0;
}