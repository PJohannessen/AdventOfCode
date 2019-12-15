<Query Kind="Program" />

static Solver solver1;
static Solver solver2;

static void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("15.txt").First();
    long[] memory = inputString.Split(',').Select(n => long.Parse(n)).ToArray();
    var machine = new Machine(memory);
    
    solver1 = new Solver(machine, Block.Free);
    solver1.Solve();
    solver2.Solve();

    $"P1: {solver1.StepsToTarget}".Dump();
    $"P2: {solver2.TotalSteps}".Dump();
}

class Solver
{
    Dictionary<(long X, long Y), Block> _blocks = new Dictionary<(long X, long Y), UserQuery.Block>();
    private Machine _startingMachine;
    public int StepsToTarget { get; private set; }
    public int TotalSteps { get; private set; }
    
    public Solver(Machine m, Block startingBlock)
    {
        _startingMachine = m;
        _blocks.Add((0, 0), startingBlock);
    }
    
    public void Solve()
    {
        Navigate(_startingMachine, (0, 0), Direction.North, 1);
        Navigate(_startingMachine, (0, 0), Direction.South, 1);
        Navigate(_startingMachine, (0, 0), Direction.East, 1);
        Navigate(_startingMachine, (0, 0), Direction.West, 1);
    }

    private void Navigate(Machine prevMachine, (long X, long Y) location, Direction direction, int steps)
    {
        var c = GetNewLocation(location, direction);
        if (_blocks.ContainsKey(c)) return;
        var machine = new Machine(prevMachine);
        machine.Calc((int)direction);
        Block r = (Block)machine.Output;
        bool shouldContinue = false;
        switch (r)
        {
            case Block.Free:
                {
                    shouldContinue = true;
                    _blocks[c] = Block.Free;
                }
                break;
            case Block.Oxygen:
                {
                    shouldContinue = true;
                    StepsToTarget = steps;
                    _blocks[c] = Block.Oxygen;
                    solver2 = new Solver(machine, Block.Oxygen);
                }
                break;
            case Block.Wall:
                {
                    shouldContinue = false;
                    _blocks[c] = Block.Wall;
                }
                break;
        }

        if (shouldContinue)
        {
            if (steps > TotalSteps) TotalSteps = steps;
            Navigate(machine, c, Direction.North, steps + 1);
            Navigate(machine, c, Direction.South, steps + 1);
            Navigate(machine, c, Direction.East, steps + 1);
            Navigate(machine, c, Direction.West, steps + 1);
        }
    }

    private (long X, long Y) GetNewLocation((long X, long Y) c, Direction d)
    {
        if (d == Direction.North) return (c.X, c.Y - 1);
        else if (d == Direction.South) return (c.X, c.Y + 1);
        else if (d == Direction.East) return (c.X - 1, c.Y);
        else return (c.X + 1, c.Y);
    }
}
public enum Direction
{
    North = 1,
    South = 2,
    West = 3,
    East = 4
}

public enum Block
{
    Wall = 0,
    Free = 1,
    Oxygen = 2
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