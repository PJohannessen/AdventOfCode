<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("2.txt");

    Dictionary<char, Dictionary<char, char>> keyLookup = new Dictionary<char, System.Collections.Generic.Dictionary<char, char>>
    {
        { '1', new Dictionary<char, char> { { 'U', '1' }, { 'R', '1' }, { 'D', '3' }, { 'L', '1' } } },
        { '2', new Dictionary<char, char> { { 'U', '2' }, { 'R', '3' }, { 'D', '6' }, { 'L', '2' } } },
        { '3', new Dictionary<char, char> { { 'U', '1' }, { 'R', '4' }, { 'D', '7' }, { 'L', '2' } } },
        { '4', new Dictionary<char, char> { { 'U', '4' }, { 'R', '4' }, { 'D', '8' }, { 'L', '3' } } },
        { '5', new Dictionary<char, char> { { 'U', '5' }, { 'R', '6' }, { 'D', '5' }, { 'L', '5' } } },
        { '6', new Dictionary<char, char> { { 'U', '2' }, { 'R', '7' }, { 'D', 'A' }, { 'L', '5' } } },
        { '7', new Dictionary<char, char> { { 'U', '3' }, { 'R', '8' }, { 'D', 'B' }, { 'L', '6' } } },
        { '8', new Dictionary<char, char> { { 'U', '4' }, { 'R', '9' }, { 'D', 'C' }, { 'L', '7' } } },
        { '9', new Dictionary<char, char> { { 'U', '9' }, { 'R', '9' }, { 'D', '9' }, { 'L', '8' } } },
        { 'A', new Dictionary<char, char> { { 'U', '6' }, { 'R', 'B' }, { 'D', 'A' }, { 'L', 'A' } } },
        { 'B', new Dictionary<char, char> { { 'U', '7' }, { 'R', 'C' }, { 'D', 'D' }, { 'L', 'A' } } },
        { 'C', new Dictionary<char, char> { { 'U', '8' }, { 'R', 'C' }, { 'D', 'C' }, { 'L', 'B' } } },
        { 'D', new Dictionary<char, char> { { 'U', 'B' }, { 'R', 'D' }, { 'D', 'D' }, { 'L', 'D' } } },
    };

    string code = "";
    char currentKey = '5';
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
