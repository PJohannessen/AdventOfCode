<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	string[] inputStrings = Utils.ParseStrings("22.txt", true);
	List<int> player1Start = new List<int>();
	List<int> player2Start = new List<int>();
	
	int player = 0;
	foreach (var s in inputStrings)
	{
		if (s == "Player 1:") player = 1;
		else if (s == "Player 2:") player = 2;
		else if (player == 1) player1Start.Insert(0, int.Parse(s));
		else if (player == 2) player2Start.Insert(0, int.Parse(s));
		else throw new Exception();
	}
	(int part1Winner, List<int> part1WinningDeck) = Play(new List<int>(player1Start), new List<int>(player2Start), false);
	(int part2Winner, List<int> part2WinningDeck) = Play(new List<int>(player1Start), new List<int>(player2Start), true);

	int part1Total = 0, part2Total = 0;
	int multipler = 1;
	for (int i = 0; i < part1WinningDeck.Count; i++)
	{
		part1Total += (multipler * part1WinningDeck[i]);
		part2Total += (multipler * part2WinningDeck[i]);
		multipler++;
	}
	
	part1Total.Dump("P1");
	part2Total.Dump("P2");
}

(int player, List<int> winningDeck) Play(List<int> p1Deck, List<int> p2Deck, bool recursive)
{
	int deckSize = p1Deck.Count + p2Deck.Count;
	HashSet<string> completedRounds = new();
	while (p1Deck.Count != 0 && p2Deck.Count != 0)
	{
		string order = string.Join(',', p1Deck.Select(i => i.ToString()).Concat(new string[] { ":" }).Concat(p2Deck.Select(i => i.ToString())));
		if (completedRounds.Contains(order))
		{
			return (1, null);
		}
		completedRounds.Add(order);
		
		int p1card = p1Deck.Last();
		p1Deck.RemoveAt(p1Deck.Count - 1);
		int p2card = p2Deck.Last();
		p2Deck.RemoveAt(p2Deck.Count - 1);

		int winner;
		List<int> winningDeck;
		if (recursive && p1card <= p1Deck.Count && p2card <= p2Deck.Count)
		{
			var newP1List = p1Deck.Skip(p1Deck.Count-p1card).Take(p1card).ToList();
			var newP2List = p2Deck.Skip(p2Deck.Count-p2card).Take(p2card).ToList();
			(winner, winningDeck) = Play(newP1List, newP2List, true);
		}
		else
		{
			winner = p1card > p2card ? 1 : 2;
		}

		if (winner == 1)
		{
			p1Deck.Insert(0, p1card);
			p1Deck.Insert(0, p2card);
		}
		else if (winner == 2)
		{
			p2Deck.Insert(0, p2card);
			p2Deck.Insert(0, p1card);
		}
		else throw new Exception();
		if (p1Deck.Count + p2Deck.Count != deckSize)
		{
			throw new Exception();
		}
	}
	return p1Deck.Count > 0 ? (1, p1Deck) : (2, p2Deck);
}