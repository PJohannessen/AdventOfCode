<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("16.txt").Single().Split(',');
    
    string startingPosition = "abcdefghijklmnop";
    string position = startingPosition;

    List<string> positions = new List<string>();
    
    while (!positions.Contains(position))
    {
        positions.Add(position);
        foreach (var step in steps)
        {
            StringBuilder sb = new StringBuilder();
            if (step[0] == 's')
            {
                int spin = int.Parse(step.Substring(1));
                sb.Append(position.Substring(position.Length - spin));
                sb.Append(position.Substring(0, position.Length - spin));
            }
            else if (step[0] == 'x')
            {
                var parts = step.Substring(1).Split('/');
                var pA = int.Parse(parts[0]);
                var pB = int.Parse(parts[1]);
                for (int i = 0; i < position.Length; i++)
                {
                    if (i == pA) sb.Append(position[pB]);
                    else if (i == pB) sb.Append(position[pA]);
                    else sb.Append(position[i]);
                }
            }
            else if (step[0] == 'p')
            {
                var cA = step[1];
                var cB = step[3];
                foreach (var c in position)
                {
                    if (c == cA) sb.Append(cB);
                    else if (c == cB) sb.Append(cA);
                    else sb.Append(c);
                }
            }
            position = sb.ToString();
        }
    }

    $"P1: {positions[1]}".Dump();
    $"P2: {positions[(1000000000 % positions.Count)]}".Dump();
}