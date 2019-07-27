<Query Kind="Program" />

static int _leastSteps = 200;
static Dictionary<string, string> _replacements = new Dictionary<string, string>();
static string _target = "e";

void Main()
{
    
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("19.txt");
    foreach (var inputLine in inputLines.Where(il => il.Contains("=>")))
    {
        string[] split = inputLine.Split(' ');
        _replacements.Add(split[2], split[0]);
    }
    
    string molecule = inputLines.Last();
    
    Process(molecule, 0);
    _leastSteps.Dump();
}

static void Process(string inputString, int counter)
{   
    if (counter >= _leastSteps) return;
    if (inputString == _target)
    {
        _leastSteps = counter;
        _leastSteps.Dump();
        return;
    }
    
    var replacements = Shuffle(_replacements.Select(kvp => kvp).ToList());
    
    foreach (var sub in replacements)
    {
        if (inputString.Contains(sub.Key))
        {
            for (int i = 0; i < inputString.Length + 1 - sub.Key.Length; i++)
            {
                if (new string(inputString.Skip(i).Take(sub.Key.Length).ToArray()) == sub.Key)
                {
                    Process(inputString.Substring(0, i) + sub.Value + inputString.Substring(i + sub.Key.Length), counter + 1);
                }
            }
        }
    }
}

public static IList<T> Shuffle<T>(IList<T> list)
{
    Random rng = new Random();
    int n = list.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
    }
    return list;
}