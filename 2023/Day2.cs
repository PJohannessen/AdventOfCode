namespace aoc;

public static class Day2
{
    public static void Process()
    {
        string[] games = Utils.ParseStrings("2.txt");
        int p1 = 0;
        int p2 = 0;
        foreach (var game in games)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 }
            };
            var details = game.Split(": ")[0];
            var outcomes = game.Split(": ")[1].Split(new string[]{"; ", ", ", " "}, StringSplitOptions.None);
            var gameNumber = int.Parse(details.Substring(5));
            for (int i = 0; i < outcomes.Length; i = i + 2)
            {
                int count = int.Parse(outcomes[i]);
                string colour = outcomes[i + 1];
                if (dict[colour] < count) dict[colour] = count;
            }

            if ((!dict.ContainsKey("red") || dict["red"] <= 12) &&
                (!dict.ContainsKey("green") || dict["green"] <= 13) &&
                (!dict.ContainsKey("blue") || dict["blue"] <= 14))
            {
                p1 += gameNumber;
            }

            p2 += (dict["red"] * dict["blue"] * dict["green"]);
        }

        Console.WriteLine($"P1: {p1}");
        Console.WriteLine($"P2: {p2}");
    }    
}