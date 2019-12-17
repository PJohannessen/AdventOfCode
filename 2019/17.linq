<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("17.txt").First();
    long[] memory = inputString.Split(',').Select(i => long.Parse(i)).ToArray();
    
    string p1 = Part1(memory);  
    memory[0] = 2;
    string p2 = Part2(memory);

    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}

string Part1(long[] memory)
{
    Machine m = new Machine(memory);
    int x = 0;
    int y = 0;
    Dictionary<(int x, int y), char> dictionary = new Dictionary<(int x, int y), char>();
    while (!m.Complete)
    {
        m.Calc();
        char c = (char)m.Output;
        if (c == 10)
        {
            x = 0;
            y++;
        }
        else
        {
            dictionary.Add((x, y), c);
            x++;
        }
    }

    int sum = 0;
    for (int y2 = 0; y2 <= dictionary.Keys.Max(k => k.y); y2++)
    {
        for (int x2 = 0; x2 <= dictionary.Keys.Max(k => k.x); x2++)
        {
            (int x, int y)[] siblings = new (int x, int y)[] { (x2 - 1, y2), (x2 + 1, y2), (x2, y2 - 1), (x2, y2 + 1) };
            if (dictionary[(x2, y2)] == '#' &&
                siblings.All(s => dictionary.ContainsKey(s)) &&
                siblings.All(s => dictionary[s] == '#'))
            {
                sum += (x2 * y2);
            }
        }
    }
    return sum.ToString();
}

string Part2(long[] memory)
{
    // Determined manually in Excel by writing out the full path of the solution then finding the 3 common sets of inputs
    string input = "A,B,A,C,B,A,C,A,C,B\nL,12,L,8,L,8\nL,12,R,4,L,12,R,6\nR,4,L,12,L,12,R,6\nn\n";
    Machine m = new Machine(memory, input);
    while (!m.Complete)
    {
        m.Calc();
    }
    return m.Output.ToString();
}

public enum Mode
{
    Position = 0,
    Immediate = 1,
    Relative = 2
}

public class Machine
{
    private int[] inputArray;
    private int inputPointer = 0;
    
    public Machine(long[] initialMemory, string input = null)
    {
        for (int i = 0; i < initialMemory.Length; i++)
        {
            Memory[i] = initialMemory[i];
        }
        inputArray = input != null ? input.Select(c => (int)c).ToArray() : null;
    }
    
    public Machine(Machine machineToClone)
    {
        Memory = machineToClone.Memory.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        IP = machineToClone.IP;
    }

    public long IP { get; private set; } = 0;
    private long RelativeBase = 0;
    private Dictionary<long, long> Memory = new Dictionary<long, long>();
    public bool Complete { get; private set; }
    public long Output { get; private set; } = 0;
    
    private long GetMemory(long i)
    {
        if (i < 0) throw new Exception();
        if (Memory.ContainsKey(i)) return Memory[i];
        else return 0;
    }
    
    private long GetLocation(Mode m, int i)
    {
        if (m == Mode.Position) return Memory[IP + i];
        else if (m == Mode.Immediate) return IP + i;
        else if (m == Mode.Relative) return Memory[IP + i] + RelativeBase;
        else throw new Exception("Error!");
    }
    
    public void Calc(int? input = null)
    {
        while (true)
        {
            string instruction = Memory[IP].ToString();
            int opcode = instruction.Length >= 2 ? int.Parse(instruction.Substring(instruction.Length - 2)) : int.Parse(instruction);
            Mode aMode = instruction.Length >= 3 ? (Mode)int.Parse(instruction.Substring(instruction.Length - 3, 1)) : 0;
            Mode bMode = instruction.Length >= 4 ? (Mode)int.Parse(instruction.Substring(instruction.Length - 4, 1)) : 0;
            Mode cMode = instruction.Length >= 5 ? (Mode)int.Parse(instruction.Substring(instruction.Length - 5, 1)) : 0;

            switch (opcode)
            {
                case 1:
                case 2:
                    {
                        bool multiply = opcode == 2;
                        long a = GetLocation(aMode, 1);
                        long b = GetLocation(bMode, 2);
                        long c = GetLocation(cMode, 3);
                        if (multiply) Memory[c] = GetMemory(a) * GetMemory(b);
                        else Memory[c] = GetMemory(a) + GetMemory(b);
                        IP += 4;
                    }
                    break;
                case 3:
                    {
                        long a = GetLocation(aMode, 1);
                        int inputValue = 0;
                        if (input.HasValue) inputValue = input.Value;
                        else if (inputArray != null && inputPointer < inputArray.Length)
                        {
                            Memory[a] = inputArray[inputPointer];
                            inputPointer++;
                        }
                        IP += 2;
                    }
                    break;
                case 4:
                    {
                        long a = GetLocation(aMode, 1);
                        Output = GetMemory(a);
                        IP += 2;
                        return;
                    }
                case 5:
                case 6:
                    {
                        bool checkEqual = opcode == 6;
                        long a = GetLocation(aMode, 1);
                        long b = GetLocation(bMode, 2);
                        if (checkEqual && GetMemory(a) == 0 || !checkEqual && GetMemory(a) != 0) IP = GetMemory(b);
                        else IP += 3;
                    }
                    break;
                case 7:
                case 8:
                    {
                        bool checkEqual = opcode == 8;
                        long a = GetLocation(aMode, 1);
                        long b = GetLocation(bMode, 2);
                        long c = GetLocation(cMode, 3);
                        if (checkEqual && GetMemory(a) == GetMemory(b) || !checkEqual && GetMemory(a) < GetMemory(b)) Memory[c] = 1;
                        else Memory[c] = 0;
                        IP += 4;
                    }
                    break;
                case 9:
                    {
                        long a = GetLocation(aMode, 1);
                        RelativeBase += GetMemory(a);
                        IP += 2;
                    }
                    break;
                case 99:
                    {
                        Complete = true;
                        return;
                    }
                default:
                    throw new Exception("Error!");
            }
        }

        throw new Exception("Error!");
    }
}