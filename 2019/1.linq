<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    int[] numbers = System.IO.File.ReadAllLines("1.txt").Select(i => int.Parse(i)).ToArray();
    
    One(numbers);
    Two(numbers);
}

void One(int[] numbers)
{
    int total = 0;
    foreach (var n in numbers)
    {
        int j = (n / 3) - 2;
        total += j;
    }

    total.Dump(total);
}

void Two(int[] numbers)
{
    int total = 0;
    foreach (var n in numbers)
    {
        int i = n;
        int j = 0;
        while (i > 0)
        {
            j = (i / 3) - 2;
            if (j <= 0) break;
            total += j;
            i = j;
        }
    }
    
    total.Dump(total);
}