<Query Kind="Program" />

 Dictionary<string, int> things = new Dictionary<string, int>();
 
 void Main()
{
    string tickerTape = @"children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1";
    foreach (var thingPairs in tickerTape.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
    {
        string[] split = thingPairs.Split(' ');
        string thing = split[0].Substring(0, split[0].Length - 1);
        int thingAmount = int.Parse(split[1]);
        things.Add(thing, thingAmount);
    }

    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("16.txt");
    
    foreach (var inputLine in inputLines)
    {
        var strings = Regex.Matches(inputLine, @"[a-zA-z]+");
        var numbers = Regex.Matches(inputLine, @"[+-]?\d+(\.\d+)?");
        if (Compare(strings[1].Value, int.Parse(numbers[1].Value)) &&
            Compare(strings[2].Value, int.Parse(numbers[2].Value)) &&
            Compare(strings[3].Value, int.Parse(numbers[3].Value)))
        {
            numbers[0].Value.Dump();
            return;
        }
    }
}

bool Compare(string thing, int number)
{
    return things[thing] == number;
}