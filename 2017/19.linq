<Query Kind="Program" />

char[,] _grid;
int _xMax = 0;
int _yMax = 0;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("19.txt");
    
    _yMax = steps.Length;
    _xMax = steps.First().Length;
    _grid = new char[_yMax, _xMax];
    
    
    int currentX = 0;
    int currentY = 0;
    List<char> seen = new List<char>();
    Direction direction = Direction.South;
    
    
    for (int y = 0; y < _yMax; y++)
    {
        for (int x = 0; x < _xMax; x++)
        {
            _grid[y,x] = steps[y][x];
            if (y == 0 && _grid[y,x] == '|') currentX = x;
        }
    }
    
    int counter = 0;
    while (true)
    {
        counter++;
        char c = _grid[currentY, currentX];
        if (char.IsLetter(c))
        {
            seen.Add(c);
            if (c == 'U')
            {
                $"P1: {new string(seen.ToArray())}".Dump();
                $"P2: {counter}".Dump();
                return;
            }
        }
        
        if (c == '+')
        {
            switch (direction)
            {
                case Direction.North:
                    if (currentX > 0 && (_grid[currentY, currentX-1] == '-' || char.IsLetter(_grid[currentY, currentX-1])))
                    {
                        currentX--;
                        direction = Direction.West;
                    }
                    else
                    {
                        currentX++;
                        direction = Direction.East;
                    }
                    break;
                case Direction.South:
                    if (currentX > 0 && (_grid[currentY, currentX-1] == '-' || char.IsLetter(_grid[currentY, currentX-1])))
                    {
                        currentX--;
                        direction = Direction.West;
                    }
                    else
                    {
                        currentX++;
                        direction = Direction.East;
                    }
                    break;
                case Direction.East:
                    if (currentY > 0 && (_grid[currentY - 1, currentX] == '|' || char.IsLetter(_grid[currentY-1, currentX])))
                    {
                        currentY--;
                        direction = Direction.North;
                    }
                    else
                    {
                        currentY++;
                        direction = Direction.South;
                    }
                    break;
                case Direction.West:
                    if (currentX > 0 && (_grid[currentY - 1, currentX] == '|' || char.IsLetter(_grid[currentY-1, currentX])))
                    {
                        currentY--;
                        direction = Direction.North;
                    }
                    else
                    {
                        currentY++;
                        direction = Direction.South;
                    }
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.North:
                    currentY--;
                    break;
                case Direction.South:
                    currentY++;
                    break;
                case Direction.East:
                    currentX++;
                    break;
                case Direction.West:
                    currentX--;
                    break;
            }
        }
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}