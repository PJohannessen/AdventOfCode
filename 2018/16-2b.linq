<Query Kind="Program" />

Dictionary<int, Action<int, int, int>> opcodes = new Dictionary<int, Action<int, int, int>>();
int[] registers;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("16-2.txt");   
    opcodes.Add(12, addr);
    opcodes.Add(14, addi);
    opcodes.Add(7, mulr);
    opcodes.Add(3, muli);
    opcodes.Add(10, banr);
    opcodes.Add(15, bani);
    opcodes.Add(5, borr);
    opcodes.Add(6, bori);
    opcodes.Add(0, setr);
    opcodes.Add(9, seti);
    opcodes.Add(13, gtir);
    opcodes.Add(2, gtri);
    opcodes.Add(8, gtrr);
    opcodes.Add(4, eqir);
    opcodes.Add(11, eqri);
    opcodes.Add(1, eqrr);
    
    registers = new int[4];
    for (int i = 0; i < inputLines.Length; i++)
    {
        int[] instructions = inputLines[i].Split(' ').Select(n => int.Parse(n)).ToArray();
        opcodes[instructions[0]](instructions[1], instructions[2], instructions[3]);
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