<Query Kind="Program" />

static List<(long computer, long x, long y)> queue = new List<(long computer, long x, long y)>();

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("23.txt").First();
    long[] memory = inputString.Split(',').Select(i => long.Parse(i)).ToArray();

    Machine[] machines = Enumerable.Range(0, 50).Select(i => new Machine(memory)).ToArray();

    long lastY = long.MaxValue;
    bool seenY = false;
    for (long i = 0; i < machines.Length; i++)
    {
        var machine = machines[i];
        machine.Calc(new long[] { i });
    }

    while (true)
    {
        for (long i = 0; i < machines.Length; i++)
        {
            var machine = machines[i];
            var inputs = queue.Where(q => q.computer == i);
            if (inputs.Count() > 0) {
                var input = inputs.Select(i => new[] { i.x, i.y }).SelectMany(i => i).ToArray();
                queue.RemoveAll(q => q.computer == i);
                machine.Calc(input);
            }
            else
            {
                machine.Calc(new long[] { -1 });
            }
        }
        
        if (queue.Any(q => q.computer == 255) && !seenY)
        {
            $"P1: {queue.First(q => q.computer == 255).y}".Dump();
            seenY = true;
        }
        
        if (queue.Count > 0 && queue.All(q => q.computer == 255))
        {
            var input = queue.Last();
            queue.Clear();
            if (input.y == lastY)
            {
                $"P2: {lastY}".Dump();
                return;
            }
            else
            {
                lastY = input.y;
                queue.Add((0, input.x, input.y));
            }
        }
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
    private long[] inputArray;
    private int inputPointer = 0;
    
    public Machine(long[] initialMemory, long[] input = null)
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
    
    List<long> outputs = new List<long>();
    
    public void Calc(long[] input = null)
    {
        inputPointer = 0;
        while (true)
        {
            if (outputs.Any() && outputs.Count % 3 == 0)
            {
                for (int i = 0; i < outputs.Count; i += 3)
                {
                    queue.Add((outputs[i], outputs[i+1], outputs[i+2]));
                    //queue.Last().Dump();
                }
                outputs.Clear();
            }
            
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
                        long inputValue = 0;
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
                        outputs.Add(Output);
                        IP += 2;
                    }
                    break;
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