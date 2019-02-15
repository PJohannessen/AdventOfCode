<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("3.txt");
    
    int valid = 0;
    foreach (var inputLine in inputLines)
    {
        string[] inputs = inputLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int[] numbers = inputs.Select(i => int.Parse(i)).OrderBy(n => n).ToArray();
        if ((numbers[0] + numbers[1]) > numbers[2]) valid++;
    }
    valid.Dump();
}