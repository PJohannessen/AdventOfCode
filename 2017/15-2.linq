<Query Kind="Program" />

void Main()
{
    long a = 512;
    long b = 191;
    int iterations = 5000000;
    
    int counter = 0;
    
    for (int i = 1; i <= iterations; i++)
    {
        do {
            a = a * 16807 % 2147483647;
        } while (a % 4 != 0);
        do {
            b = b * 48271 % 2147483647;
        } while (b % 8 != 0);
        
        string aString = Convert.ToString(a, 2);
        string bString = Convert.ToString(b, 2);
        
        if (aString.Length < 16) aString = aString.PadLeft(16, '0');
        if (bString.Length < 16) bString = bString.PadLeft(16, '0');
        if (aString.Substring(aString.Length - 16, 16) == bString.Substring(bString.Length - 16, 16)) counter++;
    }
    
    counter.Dump();
}