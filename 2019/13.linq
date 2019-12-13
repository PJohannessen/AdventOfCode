<Query Kind="Program" />

static void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("13.txt").First();
    long[] memory = inputString.Split(',').Select(n => long.Parse(n)).ToArray();
    memory[0] = 2;
    
    var arcadeMachine = new Machine(memory);
    Dictionary<(long X, long Y), long> tiles = new Dictionary<(long X, long Y), long>();
    
    long score = 0;
    int? totalBlocks = null;
    
    while (!arcadeMachine.Complete) {
    
        (long X, long Y)? ball = null;
        (long X, long Y)? paddle = null;
        if (tiles.Any(p => p.Value == 4)) ball = tiles.SingleOrDefault(p => p.Value == 4).Key;
        if (tiles.Any(p => p.Value == 3)) paddle = tiles.SingleOrDefault(p => p.Value == 3).Key;
        
        int joystockPosition = 0;
        if (ball != null && paddle != null)
        {
            if (ball.Value.X < paddle.Value.X) joystockPosition = -1;
            else if (ball.Value.X > paddle.Value.X) joystockPosition = 1;
        }
    
        long a = 0, b = 0, c = 0;
        arcadeMachine.Calc(joystockPosition);
        a = arcadeMachine.Output;
        arcadeMachine.Calc(joystockPosition);
        b = arcadeMachine.Output;
        arcadeMachine.Calc(joystockPosition);
        c = arcadeMachine.Output;

        if (a == -1 && b == 0) {
            score = c;
            if (!totalBlocks.HasValue) totalBlocks = tiles.Where(t => t.Value == 2).Count();
        }
        else tiles[(a, b)] = c;
    }

    $"P1: {totalBlocks}".Dump();
    $"P2: {score}".Dump();
}

public enum Mode
{
    Position = 0,
    Immediate = 1,
    Relative = 2
}

public class Machine
{
    public Machine(long[] initialMemory)
    {
        for (int i = 0; i < initialMemory.Length; i++)
        {
            Memory[i] = initialMemory[i];
        }
    }
    
    private long IP = 0;
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
    
    public void Calc(int input)
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
                        Memory[a] = input;
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