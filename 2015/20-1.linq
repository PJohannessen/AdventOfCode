<Query Kind="Program" />

void Main()
{
    int target = 34000000;
    for (int i = 2; i <= int.MaxValue; i++)
    {
        List<int> factors = new List<int> { 1, i };
        for (int j = 2; j <= Math.Sqrt(i) + 1; j++)
        {
            if (i % j == 0)
            {
                factors.Add(j);
                factors.Add(i / j);
            }
        }
        
        int sum = factors.Distinct().Sum(n => n * 10);
        if (sum >= target)
        {
            i.Dump();
            return;
        }
    }
}