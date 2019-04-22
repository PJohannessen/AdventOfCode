<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("5.txt");
    List<int> instructions = inputLines.Select(il => int.Parse(il)).ToList();  
    int index = 0;
    int count = 0;
    while (true)
    {
        if (index < 0 || index >= inputLines.Length)
        {
            Console.Write(count);
            return;
        }
        else
        {
            int jump = instructions[index];
            instructions[index] = instructions[index] + 1;
            index += jump;
        }
        count++;
    }
}