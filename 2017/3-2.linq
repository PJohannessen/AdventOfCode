<Query Kind="Program" />

int count = 1;
int x = 0;
int y = 0;
char direction = 'R';

List<Point> points = new List<Point>();

void Main()
{
    int input = 277678;
    int shift = 1;
    while (true)
    {
        for (int i = 1; i <= 2; i++)
        {
            for (int j = shift; j > 0; j--)
            {
                int value = CalculateValue(x, y);
                if (value > input)
                {
                    value.Dump();
                    return;
                }
                points.Add(new Point { X = x, Y = y, Value = value});
                count++;
                Move();
            }
            SetDirection();
        }
        shift++;
    }
}

void Move()
{
    if (direction == 'R') x++;
    else if (direction == 'U') y++;
    else if (direction == 'L') x--;
    else if (direction == 'D') y--;
}

void SetDirection()
{
    if (direction == 'R') direction = 'U';
    else if (direction == 'U') direction = 'L';
    else if (direction == 'L') direction = 'D';
    else if (direction == 'D') direction = 'R';
}

int CalculateValue(int x, int y)
{
    return Math.Max(1, points.Where(p => p.IsAdjacent(x, y)).Sum(p => p.Value));
}

public class Point
{
    public int X { get; set;}
    public int Y { get; set; }
    public int Value { get; set; }
    public bool IsAdjacent(int x, int y)
    {
        return (
            (X == x + 1 && Y == y - 1) ||
            (X == x + 1 && Y == y) ||
            (X == x + 1 && Y == y + 1) ||
            (X == x && Y == y - 1) ||
            (X == x && Y == y + 1) ||
            (X == x - 1 && Y == y - 1) ||
            (X == x - 1 && Y == y) ||
            (X == x - 1 && Y == y + 1)
        );
            
    }
}