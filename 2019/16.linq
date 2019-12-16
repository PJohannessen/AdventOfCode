<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string inputLine = System.IO.File.ReadAllLines("16.txt").First();
    
    string p1 = Part1(inputLine);
    string p2 = Part2(inputLine);
    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}

static string Part1(string input)
{
    int[] numbers = input.Select(c => int.Parse(c.ToString())).ToArray();
    for (int iteration = 1; iteration <= 100; iteration++)
    {
        int[] next = new int[numbers.Length];
        for (int outputIndex = 0; outputIndex < numbers.Length; outputIndex++)
        {
            int sum = 0;
            for (int partIndex = 0; partIndex < numbers.Length; partIndex++)
            {
                sum += (numbers[partIndex] * GetPatternPart(outputIndex+1, partIndex));
            }
            next[outputIndex] = Math.Abs(sum % 10);
        }
        numbers = next;
    }

    string result = string.Join(string.Empty, numbers.Take(8));
    return result;
}

static string Part2(string input)
{
    int messageOffset = int.Parse(input.Substring(0, 7));
    int[] numbers = new int[input.Length * 10000];
    for (int i = 0; i < numbers.Length; i++)
    {
        numbers[i] = int.Parse(input[i % input.Length].ToString());
    }

    for (int n = 1; n <= 100; n++)
    {
        int[] next = new int[numbers.Length];
        next[next.Length - 1] = numbers[next.Length - 1];
        for (int output = next.Length - 2; output >= messageOffset; output--)
        {
            int sum = numbers[output] + next[output + 1];
            next[output] = sum % 10;
        }
        numbers = next;
    }

    string result = string.Join(string.Empty, numbers.Skip(messageOffset).Take(8));
    return result;
}

static int GetPatternPart(int repeat, int index)
{
    
    int aLower = 0, aUpper = aLower+repeat-1;
    int bLower = aUpper+1, bUpper = bLower+repeat-1;
    int cLower = bUpper+1, cUpper = cLower+repeat-1;
    int dLower = cUpper+1, dUpper = dLower+repeat-1;
    int n = (index % (dUpper+1)) + 1;
    
    int multiplier = 0;
    if (n >= bLower && n <= bUpper) multiplier = 1;
    if (n >= dLower && n <= dUpper) multiplier = -1;
    
    return multiplier;
}