<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
	var strings = Utils.ParseStrings("12.txt");
	
    char direction = 'E';
    int x = 0, y = 0, shipX = 0, shipY = 0, waypointX = 10, waypointY = -1;

    foreach (var s in strings)
    {
        char c = s[0];
        int n = int.Parse(s[1..]);

        switch (c)
        {
            case 'N':
                waypointY -= n;
                break;
            case 'E':
                waypointX += n;
                break;
            case 'S':
                waypointY += n;
                break;
            case 'W':
                waypointX -= n;
                break;
            case 'L':
            case 'R':
                for (int i = 1; i <= n / 90; i++)
                {
                    int xDiff = waypointX - shipX;
                    int yDiff = waypointY - shipY;

                    int applyX = c == 'L' ? -yDiff : yDiff;
                    int applyY = c == 'L' ? xDiff : -xDiff;

                    waypointX = shipX - applyX;
                    waypointY = shipY - applyY;
                }
                break;
            case 'F':
                int xDiff2 = waypointX - shipX;
                int yDiff2 = waypointY - shipY;
                shipX = (shipX + xDiff2 * n);
                shipY = (shipY + yDiff2 * n);
                waypointX = (waypointX + xDiff2 * n);
                waypointY = (waypointY + yDiff2 * n);
                break;
        }
        
        if (c == 'F') c = direction;

        switch (c)
        {
            case 'N':
                y -= n;
                break;
            case 'E':
                x += n;
                break;
            case 'S':
                y += n;
                break;
            case 'W':
                x -= n;
                break;
            case 'L':
                for (int i = 1; i <= n / 90; i++)
                {
                    direction = TurnLeft(direction);
                }
                break;
            case 'R':
                for (int i = 1; i <= n / 90; i++)
                {
                    direction = TurnRight(direction);
                }
                break;
            case 'F':
                c = direction;
                break;
        }
    }

    int p1 = (Math.Abs(x) + Math.Abs(y));
    int p2 = (Math.Abs(shipX) + Math.Abs(shipY));

    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}


char TurnLeft(char d) => d switch {
    'N' => 'W',
    'W' => 'S',
    'S' => 'E',
    'E' => 'N',
     _  => d
};

char TurnRight(char d) => d switch {
    'N' => 'E',
    'E' => 'S',
    'S' => 'W',
    'W' => 'N',
     _ => d
};