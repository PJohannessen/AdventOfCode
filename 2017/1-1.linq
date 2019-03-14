<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputLine = File.ReadAllLines("1.txt").First();
    
    int sum = 0;
    for (int i = 0; i < inputLine.Length; i++)
    {
        if (i == inputLine.Length - 1)
        {
            if (inputLine[i] == inputLine[0])
            {
                sum += int.Parse(inputLine[i].ToString());
            }
        }
        else if (inputLine[i] == inputLine[i+1])
        {
            sum += int.Parse(inputLine[i].ToString());
        }
    }
    sum.Dump();
}