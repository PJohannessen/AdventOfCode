<Query Kind="Program" />

void Main()
{
    int players = 466;
    int lastMarbleWorth = 7143600;
    LinkedList<long> marbles = new LinkedList<long>();
    marbles.AddFirst(0);
    Dictionary<int, long> points = new Dictionary<int, long>();
    for (int i = 1; i <= players; i++)
    {
        points.Add(i, 0);
    }
    long currentMarble = 0;
    int currentPlayer = 1;
    LinkedListNode<long> currentMarbleNode = marbles.First;
    while (currentMarble <= lastMarbleWorth)
    {
        if ((currentMarble + 1) % 23 != 0)
        {
            currentMarbleNode = marbles.AddAfter(currentMarbleNode.Next ?? currentMarbleNode.List.First, currentMarble + 1);
        }
        else
        {
            for (int i = 1; i <= 6; i++)
            {
                currentMarbleNode = currentMarbleNode.Previous ?? currentMarbleNode.List.Last;
            }
            points[currentPlayer] = points[currentPlayer] + currentMarble + 1 + (currentMarbleNode.Previous != null ? currentMarbleNode.Previous.Value : currentMarbleNode.List.Last.Value);
            marbles.Remove(currentMarbleNode.Previous ?? currentMarbleNode.List.Last);
        }
        currentMarble++;
        if (currentPlayer == players) currentPlayer = 1;
        else currentPlayer++;
    }
    points.Values.Max().Dump();
}