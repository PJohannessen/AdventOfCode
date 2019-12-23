<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = System.IO.File.ReadAllLines("22.txt");
    int totalCards = 10007;

    int[] cards = Enumerable.Range(0, totalCards).ToArray();

    foreach (var line in inputLines)
    {
        if (line.StartsWith("deal into"))
        {
            cards = DealInto(cards);
        }
        else if (line.StartsWith("cut"))
        {
            cards = Cut(cards, int.Parse(line.Split(' ').Last()));
        }
        else
        {
            cards = DealWith(cards, int.Parse(line.Split(' ').Last()));
        }
    }
    cards.ToList().IndexOf(2019).Dump();
    
    int[] DealInto(int[] deck)
    {
        return deck.Reverse().ToArray();
    }

    int[] Cut(int[] deck, int n)
    {
        if (n > 0)
            return deck[n..].Concat(deck[0..n]).ToArray();
        else
        {
            n = Math.Abs(n);
            return deck[^n..].Concat(deck[0..^n]).ToArray();
        }
            
    }
    
    int[] DealWith(int[] oldDeck, int n)
    {
        int[] newDeck = Enumerable.Range(0, oldDeck.Length).Select(i => -1).ToArray();
        
        int counter = 0;
        for (int i = 0; i < oldDeck.Length; i++)
        {
            newDeck[counter] = oldDeck[i];
            counter += n;
            if (counter >= oldDeck.Length)
            {
                counter = counter - oldDeck.Length;
            }
        }
        
        return newDeck;
    }
}