<Query Kind="Program" />

static string[] _steps;
static int _tally;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    _steps = File.ReadAllLines("18.txt");
    
    Dictionary<char, long> aRegisters = new Dictionary<char, long> { { 'a', 0 }, { 'b', 0 }, { 'f', 0 }, { 'i', 0 }, { 'p', 0 } };
    Dictionary<char, long> bRegisters = new Dictionary<char, long> { { 'a', 0 }, { 'b', 0 }, { 'f', 0 }, { 'i', 0 }, { 'p', 1 } };
    Queue<long> aQueue = new Queue<long>();
    Queue<long> bQueue = new Queue<long>();
    long aPosition = 0;
    long bPosition = 0;

    do
    {
        aPosition = ProcessCommand(aPosition, bQueue, aQueue, aRegisters, false);
        bPosition = ProcessCommand(bPosition, aQueue, bQueue, bRegisters, true);
    } while (aQueue.Count > 0 || bQueue.Count > 0);
    
    _tally.Dump();
}

public long ProcessCommand(long position, Queue<long> readQueue, Queue<long> writeQueue, Dictionary<char, long> registers, bool tally)
{
    while (true)
    {
        string[] parts = _steps[position].Split(' ');
        string command = parts[0];

        switch (command)
        {
            case "snd":
                writeQueue.Enqueue(registers[parts[1].Single()]);
                if (tally) _tally++;
                position++;
                break;
            case "set":
            case "add":
            case "mul":
            case "mod":
                char target = parts[1].Single();
                long source = 0;
                if (char.IsLetter(parts[2].First())) source = registers[parts[2].Single()];
                else source = long.Parse(parts[2]);
                if (command == "set") registers[target] = source;
                else if (command == "add") registers[target] = registers[target] + source;
                else if (command == "mul") registers[target] = registers[target] * source;
                else if (command == "mod") registers[target] = registers[target] % source;
                position++;
                break;
            case "rcv":
                if (readQueue.Count == 0) return position;
                else
                {
                    registers[parts[1].Single()] = readQueue.Dequeue();
                    position++;
                }
                break;
            case "jgz":
                long jumpCheck = 0;
                long jumpAmount = 0;
                if (char.IsLetter(parts[1].First())) jumpCheck = registers[parts[1].Single()];
                else jumpCheck = long.Parse(parts[1]);
                if (char.IsLetter(parts[2].First())) jumpAmount = registers[parts[2].Single()];
                else jumpAmount = long.Parse(parts[2]);
                if (jumpCheck > 0) position += jumpAmount;
                else position++;
                break;
        }
    }
}