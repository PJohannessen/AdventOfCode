<Query Kind="Program" />

Dictionary<string, Action<int, int, int>> opcodes = new Dictionary<string, Action<int, int, int>>();
Dictionary<int, string> instructions = new Dictionary<int, string>();
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
    var inputLines = File.ReadAllLines("19.txt");
    
    int count = 0;
    foreach (var inputLine in inputLines)
    {
        if (inputLine.StartsWith("#"))
        {
            instructionPointerRegister = inputLine[4] - 48;
        }
        else
        {
            instructions.Add(count, inputLine);
            count++;
        }
    }

    while (instructions.ContainsKey(instructionPointer))
    {
        registers[instructionPointerRegister] = instructionPointer;
        string[] instructionParts = instructions[instructionPointer].Split(' ').ToArray();
        opcodes[instructionParts[0]](int.Parse(instructionParts[1]), int.Parse(instructionParts[2]), int.Parse(instructionParts[3]));
        instructionPointer = registers[instructionPointerRegister];
        instructionPointer++;
    }
    registers[0].Dump();
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

void eqrr(int a, int b, int c)
{
    if (registers[a] == registers[b]) registers[c] = 1;
    else registers[c] = 0;
}