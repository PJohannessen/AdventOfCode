<Query Kind="Program" />

void Main()
{
    int targetR = 2947;
    int targetC = 3029;
    
    int r = 1;
    int c = 1;
    
    ulong code = 20151125;
    
    bool complete = false;
    while (!complete)
    {
        if (r == targetR && c == targetC)
        {
            code.Dump();
            return;
        }
        else if (r == 1)
        {
            r = c + 1;
            c = 1;
        }
        else
        {
            r -= 1;
            c += 1;
        }
        code = (code * 252533) % 33554393;
    }
}
