<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputLine = File.ReadAllLines("1.txt").First();

    string[] inputs = inputLine.Split(new[] { ", " }, StringSplitOptions.None);
    char[] directions = new[] { 'N', 'E', 'S', 'W' };
    int directionIndex = 0;
    int x = 0;
    int y = 0;
    HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
    visited.Add(new Tuple<int, int>(0, 0));
    while (true)
    {
        foreach (var input in inputs)
        {
            if (input[0] == 'L') directionIndex--;
            else directionIndex++;
            if (directionIndex == -1) directionIndex = 3;
            else if (directionIndex == 4) directionIndex = 0;

            int steps = int.Parse(input.Substring(1, input.Length - 1));
            for (int i = 1; i <= steps; i++)
            {
                if (directions[directionIndex] == 'N') y--;
                else if (directions[directionIndex] == 'E') x++;
                else if (directions[directionIndex] == 'S') y++;
                else if (directions[directionIndex] == 'W') x--;

                var pos = new Tuple<int, int>(x, y);
                if (!visited.Contains(pos))
                {
                    visited.Add(pos);
                }
                else
                {
                    Console.WriteLine(Math.Abs(x) + Math.Abs(y));
                    return;
                }
            }
        }
    }
}
