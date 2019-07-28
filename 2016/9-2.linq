<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("9.txt");
    string input = inputLines.First();
    
    ulong[] weights = new ulong[input.Length];
    for (int i = 0; i < weights.Length; i++)
    {
        weights[i] = 1;
    }
    
    ulong totalLength = 0;
    bool readingMarker = false;
    string currentMarker = null;
    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] == '(')
        {
            readingMarker = true;
            currentMarker = new string(new[] { input[i] });
        }
        else if (input[i] == ')')
        {
            currentMarker += input[i];
            readingMarker = false;
            var markers = currentMarker.Split(new[] { '(', 'x', ')' }, StringSplitOptions.RemoveEmptyEntries);
            int length = int.Parse(markers[0]);
            int repeat = int.Parse(markers[1]);
            
            for (int j = 1; j <= length; j++)
            {
                weights[i + j] = weights[i + j] * (ulong)repeat;
            }
        }
        else if (readingMarker)
        {
            currentMarker += input[i];
        }
        else
        {
            totalLength += weights[i];
        }
    }

    $"Length of {totalLength}".Dump();
}