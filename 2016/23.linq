<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    List<string> instructions = File.ReadAllLines("23.txt").ToList();

    Dictionary<string, int> registers = new Dictionary<string, int> {
        {"a", 12}, {"b", 0}, {"c", 0}, {"d", 0}
    };

    int idx = 0;
    while (idx < instructions.Count)
    {
        string[] parts = instructions[idx].Split(new[] { ' ' });

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
                        idx += registers[parts[2]];
                    else idx += int.Parse(parts[2]);
                }
                    
                else idx++;
                break;
            case "tgl":
                int offset = 0;
                bool isInt = int.TryParse(parts[1], out offset);
                if (!isInt) offset = registers[parts[1]];
                int targetIndex = (idx + offset);
                if (targetIndex < instructions.Count)
                {
                    string target = instructions[targetIndex];
                    if (target.StartsWith("cpy")) instructions[targetIndex] = target.Replace("cpy", "jnz");
                    else if (target.StartsWith("inc")) instructions[targetIndex] = target.Replace("inc", "dec");
                    else if (target.StartsWith("dec")) instructions[targetIndex] = target.Replace("dec", "inc");
                    else if (target.StartsWith("jnz")) instructions[targetIndex] = target.Replace("jnz", "cpy");
                    else if (target.StartsWith("tgl")) instructions[targetIndex] = target.Replace("tgl", "inc");
                }
                idx++;
                break;
        }
    }

    registers["a"].Dump();
}