<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
    var strings = Utils.ParseStrings("13.txt");
    var allBuses = strings[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();
    
    int n = int.Parse(strings[0]);
    int[] buses = allBuses.Where(s => s != "x").Select(s => int.Parse(s)).ToArray();

    Dictionary<int, int> departures = new();
    foreach (var bus in buses)
    {
        departures.Add(bus, Array.IndexOf(allBuses, bus.ToString()));
    }

    int b = 0;
    int m = int.MaxValue;
    for (int i = 0; i < buses.Length; i++)
    {
        int tripNumber = (int)Math.Ceiling((double)n / buses[i]);
        int mins = tripNumber * buses[i];
        if (mins < m)
        {
            m = mins;
            b = buses[i];
        }
    }

    $"P1: {((m - n) * b)}".Dump();
    
    int[] orderedBuses = buses.OrderByDescending(b => b).ToArray();
    int highestBus = orderedBuses[0];
    int secondHighestBus = orderedBuses[1];
    int start = 0;
    int interval = highestBus*secondHighestBus;
    for (int i = 0; i <= int.MaxValue; i++)
    {
        if ((i+departures[secondHighestBus]) % secondHighestBus == 0 &&
            (i+departures[highestBus]) % highestBus == 0)
        {
            start = i;
            break;
        }
    }
    
    long check = start;
    bool match = false;
    while (!match)
    {
        check += interval;
        match = buses.All(bus => (check+departures[bus]) % bus == 0);
    }

    $"P2: {check}".Dump();
}