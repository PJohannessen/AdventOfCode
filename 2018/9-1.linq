<Query Kind="Program" />

void Main()
{
    int players = 466;
    int lastMarbleWorth = 71436;
    List<int> marbles = new List<int> { 0 };
    Dictionary<int, int> points = new Dictionary<int, int>();
    for (int i = 1; i <= players; i++)
    {
        points.Add(i, 0);
    }
    int currentMarble = 0;
    int currentPlayer = 1;
    int currentMarbleIndex = 0;
    while (currentMarble <= lastMarbleWorth)
    {        
        int nextMarbleIndex = currentMarbleIndex + 2;
        if ((currentMarble + 1) % 23 == 0)
        {
            points[currentPlayer] = points[currentPlayer] + currentMarble + 1;
            int indexForRemoval = currentMarbleIndex - 7;
            while (indexForRemoval < 0)
            {
                indexForRemoval = indexForRemoval + marbles.Count;
            }
            points[currentPlayer] = points[currentPlayer] + marbles[indexForRemoval];
            marbles.RemoveAt(indexForRemoval);
            currentMarbleIndex = indexForRemoval;
            currentMarble++;
        }
        else
        {
            while (nextMarbleIndex > marbles.Count)
            {
                nextMarbleIndex = nextMarbleIndex - marbles.Count;
            }
            marbles.Insert(nextMarbleIndex, currentMarble + 1);
            currentMarble++;
            currentMarbleIndex = marbles.IndexOf(currentMarble);
        }
        if (currentPlayer == players) currentPlayer = 1;
        else currentPlayer++;
    }
    points.Values.Max().Dump();
}