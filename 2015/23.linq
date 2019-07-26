<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("23.txt");

    int a = 1;
    int b = 0;
    int idx = 0;
    
    while (true)
    {
        if (idx < 0 || idx >= inputLines.Length)
        {
            b.Dump();
            return;
        }
        else
        {
            string line = inputLines[idx];
            string instruction = line.Substring(0, 3);
            switch (instruction)
            {
                case "hlf":
                    if (line[4] == 'a') a = a / 2;
                    else b = b / 2;
                    idx++;
                    break;
                case "tpl":
                    if (line[4] == 'a') a = a * 3;
                    else b = b * 3;
                    idx++;
                    break;
                case "inc":
                    if (line[4] == 'a') a++;
                    else b++;
                    idx++;
                    break;
                case "jmp":
                    idx += int.Parse(line.Substring(4));
                    break;
                case "jie":
                    if (line[4] == 'a' && a % 2 == 0) idx += int.Parse(line.Substring(7));
                    else if (line[4] == 'b' && b % 2 == 0) idx += int.Parse(line.Substring(7));
                    else idx++;
                    break;
                case "jio":
                    if (line[4] == 'a' && a == 1) idx += int.Parse(line.Substring(7));
                    else if (line[4] == 'b' && b == 1) idx += int.Parse(line.Substring(7));
                    else idx++;
                    break;
            }
        }
    }
}