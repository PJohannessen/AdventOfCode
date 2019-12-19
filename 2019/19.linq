<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("19.txt").First();
    long[] memory = inputString.Split(',').Select(i => long.Parse(i)).ToArray();
    
    $"P1: {Part1(memory)}".Dump();
    $"P2: {Part2(memory)}".Dump();
}


long Part1(long[] memory)
{
    long counter = 0;
    for (long y = 0; y < 50; y++)
    {
        for (long x = 0; x < 50; x++)
        {
            var machine = new Machine(memory, new int[] { (int)x, (int)y });
            machine.Calc();
            if (machine.Output == 1) counter++;
        }
    }
    return counter;
}

long Part2(long[] memory)
{
    Dictionary<long, (long x1, long x2)> set = new Dictionary<long, (long x1, long x2)>();
    (long x, long y) left = (0, 0);
    (long x, long y) right = (0, 0);
    long y = 6, x = 8, minX = 0;
    bool haveHit = false;
    int side = 100;

    while (true)
    {
        var machine = new Machine(memory, new int[] { (int)x, (int)y });
        machine.Calc();
        long output = machine.Output;
        if (output == 1)
        {
            if (!haveHit)
            {
                left = (x, y);
                haveHit = true;
                minX = x;
            }
            x++;
        }
        else if (output == 0)
        {
            if (!haveHit)
            {
                x++;
            }
            else if (haveHit)
            {
                right = (x - 1, y);
                x = minX;
                y++;
                haveHit = false;

                if (right.x >= left.x + side - 1)
                {
                    long lookAtY = left.y - side + 1;
                    set.Add(right.y, (left.x, right.x));
                    if (!set.ContainsKey(lookAtY)) continue;
                    var previousRow = set[lookAtY];
                    var startX = left.x;
                    var startY = lookAtY;
                    if (previousRow.x1 <= startX && previousRow.x2 >= startX + side - 1)
                    {
                        return (startX * 10000 + startY);
                    }
                }
            }
        }
        else throw new Exception();
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