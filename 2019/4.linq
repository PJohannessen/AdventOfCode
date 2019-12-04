<Query Kind="Program" />

void Main()
{
    int a = 0, b = 0;
    for (int n = 134792; n <= 675810; n++)
    {
        var result = Test(n);
        if (result.a) a++;
        if (result.b) b++;
    }

    $"P1: {a}".Dump();
    $"P2: {b}".Dump();
}

(bool a, bool b) Test(int number)
{
    bool a = false, b = false;
    string str = number.ToString();
    if (str.Length == 6 && str == new string(str.OrderBy(c => c).ToArray()))
    {
        var group = str.GroupBy(c => c);
        a = group.Any(c => c.Count() >= 2);
        b = group.Any(c => c.Count() == 2);
    }
    
    return (a, b);
}