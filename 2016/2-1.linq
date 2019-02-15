<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("2.txt");
    
    Dictionary<int, Dictionary<char, int>> keyLookup = new Dictionary<int, System.Collections.Generic.Dictionary<char, int>>
    {
        { 1, new Dictionary<char, int> { { 'U', 1 }, { 'R', 2 }, { 'D', 4 }, { 'L', 1 } } },
        { 2, new Dictionary<char, int> { { 'U', 2 }, { 'R', 3 }, { 'D', 5 }, { 'L', 1 } } },
        { 3, new Dictionary<char, int> { { 'U', 3 }, { 'R', 3 }, { 'D', 6 }, { 'L', 2 } } },
        { 4, new Dictionary<char, int> { { 'U', 1 }, { 'R', 5 }, { 'D', 7 }, { 'L', 4 } } },
        { 5, new Dictionary<char, int> { { 'U', 2 }, { 'R', 6 }, { 'D', 8 }, { 'L', 4 } } },
        { 6, new Dictionary<char, int> { { 'U', 3 }, { 'R', 6 }, { 'D', 9 }, { 'L', 5 } } },
        { 7, new Dictionary<char, int> { { 'U', 4 }, { 'R', 8 }, { 'D', 7 }, { 'L', 7 } } },
        { 8, new Dictionary<char, int> { { 'U', 5 }, { 'R', 9 }, { 'D', 8 }, { 'L', 7 } } },
        { 9, new Dictionary<char, int> { { 'U', 6 }, { 'R', 9 }, { 'D', 9 }, { 'L', 8 } } }
    };
    
    string code = "";
    int currentKey = 5;
    foreach (string line in inputLines)
    {
        foreach (char direction in line)
        {
            currentKey = keyLookup[currentKey][direction];
        }
        code += currentKey.ToString();
    }
    
    code.Dump();
}
