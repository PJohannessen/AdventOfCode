<Query Kind="Program" />

void Main()
{
    string input = "1113222113";
    for (int i = 1; i <= 50; i++)
    {
        StringBuilder builder = new StringBuilder();
        for (int j = 0; j < input.Length;)
        {
            int total = 0;
            char c = input[j];
            while (j < input.Length && input[j] == c)
            {
                total++;
                j++;
            }
            builder.Append(total.ToString());
            builder.Append(input[j - 1]);
        }
        input = builder.ToString();
        if (i == 40) $"1: {input.Length}".Dump();
        if (i == 50) $"2: {input.Length}".Dump();
    }
}