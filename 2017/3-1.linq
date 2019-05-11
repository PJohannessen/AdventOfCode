<Query Kind="Program" />

int count = 1;
int x = 0;
int y = 0;
char direction = 'R';

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
                if (count == input)
                {
                    (Math.Abs(x) + Math.Abs(y)).Dump();
                    return;
                }
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