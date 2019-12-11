<Query Kind="Program" />

static void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("11.txt").First();
    long[] memory = inputString.Split(',').Select(n => long.Parse(n)).ToArray();
    
    var machine1 = new Machine(memory, 0);
    var machine2 = new Machine(memory, 1);

    var machine1Panels = CalculatePainted(machine1);
    var machine2Panels = CalculatePainted(machine2);
    
    $"P1: {machine1Panels.Count}".Dump();
    "P2:".Dump();
    for (int y = machine2Panels.Min(p => p.Key.Y); y <= machine2Panels.Max(p => p.Key.Y); y++)
    {
        for (int x = machine2Panels.Min(p => p.Key.X); x <= machine2Panels.Max(p => p.Key.X); x++)
        {
            int l = 0;
            if (machine2Panels.ContainsKey((x, y))) l = machine2Panels[(x, y)];
            if (l == 1) Console.Write('█');
            else Console.Write(' ');
        }
        Console.WriteLine();
    }
}

static public Dictionary<(int X, int Y), int> CalculatePainted(Machine machine)
{
    (int X, int Y) location = (0, 0);
    Direction direction = Direction.Up;
    Dictionary<(int X, int Y), int> panels = new Dictionary<(int X, int Y), int>();

    while (!machine.Complete)
    {
        int current = 0;
        if (panels.ContainsKey(location)) current = panels[location];

        machine.Calc(current);
        long colorToPaint = machine.Output;
        machine.Calc(current);
        long turn = machine.Output;

        panels[location] = (int)colorToPaint;
        current = panels[location];

        if (turn == 0 && direction == Direction.Up) direction = Direction.Left;
        else if (turn == 0 && direction == Direction.Left) direction = Direction.Down;
        else if (turn == 0 && direction == Direction.Down) direction = Direction.Right;
        else if (turn == 0 && direction == Direction.Right) direction = Direction.Up;
        else if (turn == 1 && direction == Direction.Up) direction = Direction.Right;
        else if (turn == 1 && direction == Direction.Right) direction = Direction.Down;
        else if (turn == 1 && direction == Direction.Down) direction = Direction.Left;
        else if (turn == 1 && direction == Direction.Left) direction = Direction.Up;

        if (direction == Direction.Up) location = (location.X, location.Y - 1);
        else if (direction == Direction.Right) location = (location.X + 1, location.Y);
        else if (direction == Direction.Down) location = (location.X, location.Y + 1);
        else if (direction == Direction.Left) location = (location.X - 1, location.Y);
    }
    
    return panels;
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public enum Mode
{
    Position = 0,
    Immediate = 1,
    Relative = 2
}

public class Machine
{
    public Machine(long[] initialMemory, int p)
    {
        for (int i = 0; i < initialMemory.Length; i++)
        {
            Memory[i] = initialMemory[i];
        }
        Phase = p;
    }
    
    private bool ProvidedP = false;
    private long IP = 0;
    private long RelativeBase = 0;
    private int Phase;
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
                        int nextInput = ProvidedP ? input : Phase;
                        ProvidedP = true;
                        Memory[a] = nextInput;
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