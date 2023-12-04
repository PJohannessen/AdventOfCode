namespace aoc;

public class Day4
{
    public static void Process()
    {
        string[] games = Utils.ParseStrings("4.txt");
        int numberOfGames = games.Length;
        Dictionary<int, int> scorecardCopies = new Dictionary<int, int>();
        for (int i = 1; i <= numberOfGames; i++) scorecardCopies.Add(i, 1);

        double totalScore = 0;
        foreach (var game in games)
        {
            var split = game.Split(new[] { "Card ", ":", " " }, StringSplitOptions.RemoveEmptyEntries);
            int gameNumber = int.Parse(split[0]);
            HashSet<int> myNumbers = new HashSet<int>();
            HashSet<int> winningNumbers = new HashSet<int>();
            bool finishedMyNumbers = false;
            for (int i = 1; i < split.Length; i++)
            {
                string value = split[i];
                if (value == "|") finishedMyNumbers = true;
                else if (!finishedMyNumbers) myNumbers.Add(int.Parse(value));
                else winningNumbers.Add(int.Parse(value));
            }
            
            int matchingNumbers = myNumbers.Intersect(winningNumbers).Count();
            double gameScore = matchingNumbers > 0 ? Math.Pow(2, matchingNumbers - 1) : 0;
            totalScore += gameScore;

            int extraCopies = scorecardCopies[gameNumber];
            for (int extraGame = gameNumber+1; extraGame <= gameNumber+matchingNumbers && extraGame <= numberOfGames; extraGame++)
            {
                scorecardCopies[extraGame] += extraCopies;
            }
        }
        
        Console.WriteLine($"P1: {totalScore}");
        Console.WriteLine($"P2: {scorecardCopies.Values.Sum()}");
    }
}