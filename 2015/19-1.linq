<Query Kind="Program" />

void Main()
{
    Dictionary<string, List<string>> replacements = new Dictionary<string, List<string>>();
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("19.txt");
    foreach (var inputLine in inputLines.Where(il => il.Contains("=>")))
    {
        string[] split = inputLine.Split(' ');
        if (!replacements.ContainsKey(split[0])) replacements.Add(split[0], new List<string>());
        replacements[split[0]].Add(split[2]);
    }
    
    string inputString = inputLines.Last();
    char[] input = inputString.ToCharArray();
    List<string> perms = new List<string>();
    for (int i = 0; i < input.Length; i++)
    {
        List<string> currentReplacements = new List<string>();
        string lookup1 = new string(input, i, 1);
        if (replacements.ContainsKey(lookup1)) currentReplacements = replacements[lookup1];
        foreach (var r in currentReplacements)
        {
            perms.Add(
                inputString.Substring(0, i) + r + inputString.Substring(i+1)
            );
        }
        
        if (i != input.Length - 1)
        {
            currentReplacements = new List<string>();
            string lookup2 = new string(input, i, 2);
            if (replacements.ContainsKey(lookup2)) currentReplacements = replacements[lookup2];
            foreach (var r in currentReplacements)
            {
                perms.Add(
                    inputString.Substring(0, i) + r + inputString.Substring(i+2)
                );
            }
        }
    }
    perms.Distinct().Count().Dump();
}