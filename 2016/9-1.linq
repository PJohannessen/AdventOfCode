<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("9.txt");
    string input = inputLines.First();
    
    StringBuilder builder = new StringBuilder();
    
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
            while (repeat > 0)
            {
                builder.Append(input.Substring(i + 1, int.Parse(markers[0])));
                repeat--;
            }
            i += length;
        }
        else if (readingMarker)
        {
            currentMarker += input[i];
        }
        else
        {
            builder.Append(input[i]);
        }
    }

    string output = builder.ToString();
    $"Length of {output.Length} for {output}".Dump();
}