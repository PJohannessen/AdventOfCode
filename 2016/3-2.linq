<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("3.txt");
    int[] numbers = inputLines.SelectMany(il => il.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).Select(i => int.Parse(i)).ToArray();
    
    int valid = 0;
    for (int i = 0; i < numbers.Length;)
    {
        for (int j = 0; j <= 2; j++)
        {
            int[] orderedNumbers = new[] { numbers[i+j], numbers[i+j+3], numbers[i+j+6] }.OrderBy(n => n).ToArray();
            if ((orderedNumbers[0] + orderedNumbers[1]) > orderedNumbers[2]) valid++;
        }
        i = i+9;
    }
    valid.Dump();
}