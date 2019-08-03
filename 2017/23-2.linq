<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("23.txt");
    
    int f, h = 0;
    
    for (int b = 106700; b <= 123700; b += 17)
    {
        f = 1;
        for (int d = 2; d != b; d++)
        {
            bool isPrime = IsPrime(b);
            if (!isPrime)
            {
                f = 0;
                break;
            }
        }
        
        if (f == 0) h++;
    }
    
    h.Dump();
}

public bool IsPrime(int n)
{
    for (int i = 2; i <= Math.Sqrt(n); i++)
    {
        if (n % i == 0) return false;
    }
    return n != 1;
}