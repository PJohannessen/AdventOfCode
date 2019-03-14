<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputLine = File.ReadAllLines("1.txt").First();
    
    int sum = 0;
    for (int i = 0; i < inputLine.Length; i++)
    {
        int index = ((i + (inputLine.Length / 2)) % inputLine.Length);
        char other = inputLine[index];
        if (inputLine[i] == inputLine[index])
        {
            sum += int.Parse(inputLine[i].ToString());
        }
    }
    sum.Dump();
}