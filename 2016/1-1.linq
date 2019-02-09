<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputLine = File.ReadAllLines("1.txt").First();
    
    string[] inputs = inputLine.Split(new[] { ", "}, StringSplitOptions.None);
    char[] directions = new[] { 'N', 'E', 'S', 'W' };
    int directionIndex = 0;
    int x = 0;
    int y = 0;
    foreach (var input in inputs)
    {
        if (input[0] == 'L') directionIndex--;
        else directionIndex++;
        if (directionIndex == -1) directionIndex = 3;
        else if (directionIndex == 4) directionIndex = 0;

        int steps = int.Parse(input.Substring(1, input.Length - 1));
        if (directions[directionIndex] == 'N') y -= steps;
        else if (directions[directionIndex] == 'E') x += steps;
        else if (directions[directionIndex] == 'S') y += steps;
        else if (directions[directionIndex] == 'W') x -= steps;
    }
    Console.WriteLine(Math.Abs(x) + Math.Abs(y));
}
