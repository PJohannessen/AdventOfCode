<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string input = File.ReadAllLines("9.txt").First();
    
    int currentGroup = 0;
    int totalPoints = 0;
    int totalGarbage = 0;
    bool inGarbage = false;
    for (int i = 0; i < input.Length; i++)
    {
        if (inGarbage)
        {
            if (input[i] == '!')
            {
                i++;
            }
            else if (input[i] == '>')
            {
                inGarbage = false;
            }
            else
            {
                totalGarbage++;
            }
        }
        else
        {
            if (input[i] == '{')
            {
                currentGroup++;
                totalPoints += currentGroup;
            }
            else if (input[i] == '}')
            {
                currentGroup--;
            }
            else if (input[i] == '<')
            {
                inGarbage = true;
            }
        }
    }

    $"P1: {totalPoints}".Dump();
    $"P2: {totalGarbage}".Dump();
}