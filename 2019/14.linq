<Query Kind="Program" />

Dictionary<Pair, List<Pair>> _reactions;

void Main()
{
    ParseInput();
    
    long p1 = Ore(1);
    
    long targetOre = 1000000000000;
    long lowFuel = 1;
    long highFuel = 1000000000000;
    while ((highFuel - lowFuel) > 1)
    {
        long midpoint = (highFuel-lowFuel)/2+lowFuel;
        long actualFuel = Ore(midpoint);
        if (actualFuel > targetOre) { highFuel = midpoint; }
        else if (actualFuel < targetOre) { lowFuel = midpoint; }
        else { lowFuel = midpoint; highFuel = midpoint; }
    }

    $"P1: {p1}".Dump();
    $"P2: {lowFuel}".Dump();
}

long Ore(long startingFuel)
{
    var chemicals = _reactions.SelectMany(c => c.Value).Select(c => c.Chemical).Distinct().ToArray();
    Dictionary<string, long> amounts = new Dictionary<string, long>();
    foreach (var c in chemicals) { amounts[c] = 0; }
    amounts.Add(FUEL, startingFuel);
    while (amounts.Where(a => a.Key != ORE).Any(a => a.Value > 0))
    {
        var nextChemical = amounts.Where(a => a.Value > 0 && a.Key != ORE).First();
        var matchingReaction = _reactions.SingleOrDefault(c => c.Key.Chemical == nextChemical.Key);
        long multipler = (long)Math.Floor((double)(nextChemical.Value / matchingReaction.Key.Amount));
        if (multipler == 0) multipler = 1;
        amounts[nextChemical.Key] -= (matchingReaction.Key.Amount * multipler);
        foreach (var pair in matchingReaction.Value)
        {
            amounts[pair.Chemical] += (pair.Amount * multipler);
        }
    }
    long ore = amounts[ORE];
    return ore;
}

const string ORE = "ORE";
const string FUEL = "FUEL";

public class Pair
{
    public Pair(string e, int n)
    {
        Chemical = e;
        Amount = n;
    }
    public string Chemical { get; set; }
    public long Amount { get; set; }
}

void ParseInput()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("14.txt").ToArray();
    _reactions = new Dictionary<Pair, System.Collections.Generic.List<Pair>>();
    foreach (var il in inputLines)
    {
        List<Pair> pairs = new List<Pair>();
        string[] parts = il.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts[0].Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
        {
            int n = int.Parse(part.Split(' ')[0]);
            string e = part.Split(' ')[1];
            pairs.Add(new Pair(e, n));
        }

        int resultN = int.Parse(parts[1].Split(' ')[0]);
        string resultE = parts[1].Split(' ')[1];
        _reactions.Add(new Pair(resultE, resultN), pairs);
    }
}
