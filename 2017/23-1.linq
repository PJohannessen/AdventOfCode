<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("23.txt");
    Dictionary<char, long> registers = new Dictionary<char, long> { { 'a', 0 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 }, { 'e', 0 }, { 'f', 0 }, { 'g', 0 }, { 'h', 0 } };
  
    long position = 0;
    int counter = 0;
    while (position >= 0 && position < steps.Length)
    {
        string[] parts = steps[position].Split(' ');
        string command = parts[0];

        switch (command)
        {
            case "set":
            case "sub":
            case "mul":
                char target = parts[1].Single();
                long source = 0;
                if (char.IsLetter(parts[2].First())) source = registers[parts[2].Single()];
                else source = long.Parse(parts[2]);
                if (command == "set") registers[target] = source;
                else if (command == "sub") registers[target] = registers[target] - source;
                else if (command == "mul")
                {
                    registers[target] = registers[target] * source;
                    counter++;
                }
                position++;
                break;
            case "jnz":
                long jumpCheck = 0;
                long jumpAmount = 0;
                if (char.IsLetter(parts[1].First())) jumpCheck = registers[parts[1].Single()];
                else jumpCheck = long.Parse(parts[1]);
                if (char.IsLetter(parts[2].First())) jumpAmount = registers[parts[2].Single()];
                else jumpAmount = long.Parse(parts[2]);
                if (jumpCheck != 0) position += jumpAmount;
                else position++;
                break;
        }
    }
    counter.Dump();
}