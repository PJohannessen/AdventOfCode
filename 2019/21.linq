<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("21.txt").First();
    long[] memory = inputString.Split(',').Select(i => long.Parse(i)).ToArray();

    Machine m1 = new Machine(memory);
    while (!m1.Complete)
    {
        // If no ground at A, J=true.
        // If no ground at B, T=true.
        // IF no ground at A(J) OR B(T), J=true.
        // If no ground at C, T=true.
        // IF no ground at AB(J) or C(T), J=true.
        // If no ground at ABC(J), BUT no ground at D either, then J=false.
        // Walk!
        string input = "NOT A J\nNOT B T\n OR T J\nNOT C T\nOR T J\nAND D J\nWALK\n";
        int[] inputArray = input.Select(c => (int)c).ToArray();
        m1.Calc(inputArray);
    }

    // If no ground at A, jump.
    // If no ground at B, T=true.
    // IF no ground at A(J) OR B(T), J=true.
    // If no ground at C, T=true.
    // IF no ground at AB(J) or C(T), J=true.
    // If no ground at ABC(J), BUT no ground at D either, then J=false.
    // If no ground at E, T=true;
    // If ground at E(T), T=true;
    // If ground at H or E(T), T=true;
    // If "IF no ground at AB(J) or C(T)" AND "ground at E or H", J=true;
    // Run!
    Machine m2 = new Machine(memory);
    while (!m2.Complete)
    {
        string input = "NOT A J\nNOT B T\n OR T J\nNOT C T\nOR T J\nAND D J\nNOT E T\nNOT T T\nOR H T\n AND T J\nRUN\n";
        int[] inputArray = input.Select(c => (int)c).ToArray();
        m2.Calc(inputArray);
    }
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
    
    public Machine(long[] initialMemory, int[] input = null)
    {
        for (int i = 0; i < initialMemory.Length; i++)
        {
            Memory[i] = initialMemory[i];
        }
        inputArray = input != null ? input : null;
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
    
    public void Calc(int[] input = null)
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
                        if (inputPointer >= input.Length) return;
                        inputValue = input[inputPointer];
                        Memory[a] = inputValue;
                        inputPointer++;
                        IP += 2;
                    }
                    break;
                case 4:
                    {
                        long a = GetLocation(aMode, 1);
                        Output = GetMemory(a);
                        if (Output > 255) Output.Dump();
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