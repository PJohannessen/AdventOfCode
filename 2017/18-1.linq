<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("18.txt");
    Dictionary<char, long> registers = new Dictionary<char, long> { { 'a', 0 }, { 'b', 0 }, { 'f', 0 }, { 'i', 0 }, { 'p', 0 } };
  
    long position = 0;
    long lastSound = 0;
    long recoveredSound = 0;
    
    while (position >= 0 && position < steps.Length)
    {
        string[] parts = steps[position].Split(' ');
        string command = parts[0];
        
        switch (command)
        {
            case "snd":
                lastSound = registers[parts[1].Single()];
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
                if (registers[parts[1].Single()] != 0)
                {
                    recoveredSound = lastSound;
                    recoveredSound.Dump();
                    return;
                }
                position++;
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