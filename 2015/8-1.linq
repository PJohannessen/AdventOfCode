<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory (Path.GetDirectoryName (Util.CurrentQueryPath));
    var inputLines = File.ReadAllText("8.txt").Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    int higherTotal = 0;
    int lowerTotal = 0;
    foreach (var input in inputLines)
    {
        higherTotal += input.Length;
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '"') continue;
            if (input[i] == '\\')
            {
                count++;
                if (input[i+1] == 'x')
                {
                    i += 3;
                }
                else
                {
                    i += 1;
                }
                continue;
            }
            count++;
        }
        lowerTotal += count;
    }
    (higherTotal - lowerTotal).Dump();
}