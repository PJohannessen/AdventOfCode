<Query Kind="Program" />

static void Main()
{
    int p1 = GreatestSignal(new int[] { 0, 1, 2, 3, 4 }, false);
    int p2 = GreatestSignal(new int[] { 5, 6, 7, 8, 9 }, true);
    
    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}

static int GreatestSignal(int[] inputs, bool shouldLoop)
{
    int greatestSignal = int.MinValue;
    foreach (var set in permutations(inputs))
    {
        int signal = CalculateSignal(set.ToArray(), shouldLoop);
        if (signal > greatestSignal) greatestSignal = signal;
    }
    return greatestSignal;
}

static int CalculateSignal(int[] values, bool shouldLoop)
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputString = File.ReadAllLines("7.txt").First();
    int[] initialMemory = inputString.Split(',').Select(i => int.Parse(i)).ToArray();
    
    var machines = values.Select(v => new Machine(initialMemory, v)).ToList();
    do {
        for (int i = 0; i < machines.Count; i++)
        {
            var previousMachine = i == 0 ? machines.Last() : machines[i-1];
            machines[i].Calc(previousMachine.Output);
        }
    } while (shouldLoop && !machines.Last().Complete);
    return machines.Last().Output;
}

public class Machine
{
    const int ImmediateMode = 1;
    public Machine(int[] initialMemory, int p)
    {
        Memory = (int[])initialMemory.Clone();
        Phase = p;
    }
    
    private bool ProvidedP = false;
    private int IP = 0;
    private int Phase;
    private int[] Memory;
    public bool Complete { get; private set; }
    public int Output { get; private set; } = 0;
    
    public void Calc(int input)
    {
        while (IP >= 0 && IP < Memory.Length)
        {
            string instruction = Memory[IP].ToString();
            int opcode = instruction.Length >= 2 ? int.Parse(instruction.Substring(instruction.Length - 2)) : int.Parse(instruction);
            int aMode = instruction.Length >= 3 ? int.Parse(instruction.Substring(instruction.Length - 3, 1)) : 0;
            int bMode = instruction.Length >= 4 ? int.Parse(instruction.Substring(instruction.Length - 4, 1)) : 0;
            int cMode = instruction.Length >= 5 ? int.Parse(instruction.Substring(instruction.Length - 5, 1)) : 0;

            switch (opcode)
            {
                case 1:
                case 2:
                    {
                        bool multiply = opcode == 2;
                        int a = aMode == ImmediateMode ? Memory[IP + 1] : Memory[Memory[IP + 1]];
                        int b = bMode == ImmediateMode ? Memory[IP + 2] : Memory[Memory[IP + 2]];
                        int c = cMode == ImmediateMode ? IP + 3 : Memory[IP + 3];
                        if (multiply) Memory[c] = a * b;
                        else Memory[c] = a + b;
                        IP += 4;
                    }
                    break;
                case 3:
                    {
                        int nextInput = ProvidedP ? input : Phase;
                        ProvidedP = true;
                        Memory[Memory[IP + 1]] = nextInput;
                        IP += 2;
                    }
                    break;
                case 4:
                    {
                        Output = Memory[Memory[IP + 1]];
                        IP += 2;
                        return;
                    }
                case 5:
                case 6:
                    {
                        bool checkEqual = opcode == 6;
                        int a = aMode == ImmediateMode ? Memory[IP + 1] : Memory[Memory[IP + 1]];
                        int b = bMode == ImmediateMode ? Memory[IP + 2] : Memory[Memory[IP + 2]];
                        if (checkEqual && a == 0 || !checkEqual && a != 0) IP = b;
                        else IP += 3;
                    }
                    break;
                case 7:
                case 8:
                    {
                        bool checkEqual = opcode == 8;
                        int a = aMode == ImmediateMode ? Memory[IP + 1] : Memory[Memory[IP + 1]];
                        int b = bMode == ImmediateMode ? Memory[IP + 2] : Memory[Memory[IP + 2]];
                        int c = cMode == ImmediateMode ? IP + 3 : Memory[IP + 3];
                        if (checkEqual && a == b || !checkEqual && a < b) Memory[c] = 1;
                        else Memory[c] = 0;
                        IP += 4;
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

private static IEnumerable<IEnumerable<T>> permutations<T>(IEnumerable<T> source)
{
    var c = source.Count();
    if (c == 1) yield return source;
    else
    {
        for (int i = 0; i < c; i++)
        {
            foreach (var p in permutations(source.Take(i).Concat(source.Skip(i + 1))))
            {
                yield return source.Skip(i).Take(1).Concat(p);
            }
        }
    }
}