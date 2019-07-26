<Query Kind="Program" />

const int width = 50;
const int height = 6;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("12.txt");

    Dictionary<string, int> registers = new Dictionary<string, int> {
        {"a", 0}, {"b", 0}, {"c", 1}, {"d", 0}
    };
    
    int idx = 0;
    while (idx < inputLines.Length)
    {
        string[] parts = inputLines[idx].Split(new [] { ' ' });
        
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
                    idx += int.Parse(parts[2]);
                else idx++;
                break;
        }
    }
    
    registers["a"].Dump();
}