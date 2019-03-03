<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("7.txt");
    
    int a = 0;
    int b = 0;
    foreach (var line in inputLines)
    {
        if (SupportsTLS(line)) a++;
        if (SupportsSSL(line)) b++;
    }
    $"Part A: {a}".Dump();
    $"Part B: {b}".Dump();
}

bool SupportsTLS(string input)
{
    bool hypernet = false;
    bool match = false;
    for (int i = 0; i < input.Length - 3; i++)
    {
        if (input[i] == '[') hypernet = true;
        else if (input[i] == ']') hypernet = false;
        else
        {
            if (input[i] == input[i + 3] && input[i + 1] == input[i + 2] && input[i] != input[i+1])
            {
                if (hypernet) return false;
                else match = true;
            }
        }
    }
    return match;
}

bool SupportsSSL(string input)
{
    List<System.Tuple<char, char>> outerPairs = new List<System.Tuple<char, char>>();
    List<System.Tuple<char, char>> innerPairs = new List<System.Tuple<char, char>>();
    bool hypernet = false;
    for (int i = 0; i < input.Length - 2; i++)
    {
        if (input[i] == '[') hypernet = true;
        else if (input[i] == ']') hypernet = false;
        else
        {
            if (input[i] == input[i+2] && char.IsLetter(input[i+1]))
            {
                if (hypernet)
                {
                    var pair = new Tuple<char, char>(input[i+1], input[i]);
                    if (outerPairs.Contains(pair)) return true;
                    else innerPairs.Add(pair);
                }
                else
                {
                    var pair = new Tuple<char, char>(input[i], input[i+1]);
                    if (innerPairs.Contains(pair)) return true;
                    else outerPairs.Add(pair);
                }
            }
        }
    }
    return false;
}