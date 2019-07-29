<Query Kind="Program" />

void Main()
{
    List<Disc> discs = new List<Disc> {
        new Disc(1, 13, 10),
        new Disc(2, 17, 15),
        new Disc(3, 19, 17),
        new Disc(4, 7, 1),
        new Disc(5, 5, 0),
        new Disc(6, 3, 1),
        new Disc (7, 11, 0)
    };
    
    int t = 0;
    
    while (discs.Any(d => d.Position != d.RequiredPosition))
    {
        foreach (var d in discs) d.Rotate();
        t++;
    }
    
    t.Dump();
}

public class Disc
{
    public int Placement { get; private set; }
    public int TotalPositions { get; private set; }
    public int Position { get; private set; }
    public int RequiredPosition { get; private set; }
    
    public Disc(int placement, int totalPositions, int startingPosition)
    {
        Placement = placement;
        TotalPositions = totalPositions;
        Position = startingPosition;
        for (int i = 1; i <= Placement; i++)
        {
            if (RequiredPosition == 0) RequiredPosition = TotalPositions - 1;
            else RequiredPosition--;
        }
    }
    
    public void Rotate()
    {
        if (Position == TotalPositions - 1) Position = 0;
        else Position++;
    }
}
