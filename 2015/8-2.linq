<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory (Path.GetDirectoryName (Util.CurrentQueryPath));
    var inputLines = File.ReadAllText("8.txt").Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    int higherTotal = 0;
    int lowerTotal = 0;
    foreach (var input in inputLines)
    {
        lowerTotal += input.Length;
        higherTotal += input.Length + 2;
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '"') higherTotal += 1;
            if (input[i] == '\\') higherTotal += 1;
        }
        lowerTotal += count;
    }
    (higherTotal - lowerTotal).Dump();
}