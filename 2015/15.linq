<Query Kind="Program" />

void Main()
{
    int[][] inputs = new int[4][];
    inputs[0] = new [] { 3, 0, 0, -3, 2 };
    inputs[1] = new [] { -3, 3, 0, 0, 9 };
    inputs[2] = new [] { -1, 0, 4, 0, 1 };
    inputs[3] = new [] { 0, 0, -2, 2, 8 };
    
    int p1 = 0;
    int p2 = 0;
    
    for (int i = 100; i >= 0; i--)
    {
        for (int j = 100-i; j >= 0; j--)
        {
            for (int k = 100-i-j; k >= 0; k--)
            {
                int l = 100-i-j-k;
                int capacity = (inputs[0][0] * i + inputs[1][0] * j + inputs[2][0] * k + inputs[3][0] * l);
                int durability = (inputs[0][1] * i + inputs[1][1] * j + inputs[2][1] * k + inputs[3][1] * l);
                int flavor = (inputs[0][2] * i + inputs[1][2] * j + inputs[2][2] * k + inputs[3][2] * l);
                int texture = (inputs[0][3] * i + inputs[1][3] * j + inputs[2][3] * k + inputs[3][3] * l);
                int calories = (inputs[0][4] * i + inputs[1][4] * j + inputs[2][4] * k + inputs[3][4] * l);
                int total = Math.Max(0, capacity) * Math.Max(0, durability) * Math.Max(0, flavor) * Math.Max(0, texture);
                if (total > p1) p1 = total;
                if (calories == 500 && total > p2) p2 = total;
            }
        }
    }
    
    p1.Dump();
    p2.Dump();
}
