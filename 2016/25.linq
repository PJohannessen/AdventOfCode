<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

List<string[]> _instructions;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    _instructions = File.ReadAllLines("25.txt").Select(s => s.Split(new[] { ' ' })).ToList();

    int a = 0;
    bool match = false;
    string target = "0101010101010101010101010101";
    while (!match)
    {
        a++;
        match = Run(a, target);
    }
    
    a.Dump();
}

bool Run(int a, string finalTarget)
{
    Dictionary<string, long> registers = new Dictionary<string, long> {
        {"a", a}, {"b", 0}, {"c", 0}, {"d", 0}
    };

    string currentOutput = string.Empty;
    int idx = 0;
    while (idx >= 0 && idx < _instructions.Count)
    {
        string[] parts = _instructions[idx];

        switch (parts[0])
        {
            case "cpy":
                if (registers.ContainsKey(parts[1]))
                    registers[parts[2]] = registers[parts[1]];
                else registers[parts[2]] = int.Parse(parts[1]);
                idx++;
                break;
            case "inc":
                registers[parts[1]] = registers[parts[1]] + 1;
                idx++;
                break;
            case "dec":
                registers[parts[1]] = registers[parts[1]] - 1;
                idx++;
                break;
            case "jnz":
                if (registers.ContainsKey(parts[1]))
                {
                    if (registers[parts[1]] != 0)
                        idx += int.Parse(parts[2]);
                    else
                        idx++;
                }
                else if (int.Parse(parts[1]) != 0)
                {
                    if (registers.ContainsKey(parts[2]))
                        idx += (int)registers[parts[2]];
                    else idx += int.Parse(parts[2]);
                }

                else idx++;
                break;
            case "out":
                currentOutput += registers[parts[1]];
                if (currentOutput == finalTarget) return true;
                if (!finalTarget.StartsWith(currentOutput)) return false;
                idx++;
                break;
        }
    }
    
    return false;
}