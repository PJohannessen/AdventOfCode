<Query Kind="Program" />

static void Main()
{
    int targetCycles = 1000;
    (int X, int Y, int Z) a0 = (-6, 2, -9), a = (-6, 2, -9);
    (int X, int Y, int Z) b0 = (12, -14, -4), b = (12, -14, -4);
    (int X, int Y, int Z) c0 = (9, 5, -6), c = (9, 5, -6);
    (int X, int Y, int Z) d0 = (-1, -4, 9), d = (-1, -4, 9);

    (int X, int Y, int Z)[] moons = new[] { a, b, c, d };
    (int X, int Y, int Z)[] velocities = Enumerable.Range(0, 4).Select(i => (0, 0, 0)).ToArray();
    
    int p1 = int.MinValue;
    long? p2XCycle = null;
    long? p2YCycle = null;
    long? p2ZCycle = null;
    
    for (long cycle = 1; !p2XCycle.HasValue || !p2YCycle.HasValue || !p2ZCycle.HasValue; cycle++)
    {
        for (int m1idx = 0; m1idx < moons.Length; m1idx++)
        {
            for (int m2idx = m1idx + 1; m2idx < moons.Length; m2idx++)
            {
                var m1 = moons[m1idx];
                var m2 = moons[m2idx];
                
                if (m1.X > m2.X)
                {
                    velocities[m1idx].X--;
                    velocities[m2idx].X++;
                }
                else if (m2.X > m1.X)
                {
                    velocities[m1idx].X++;
                    velocities[m2idx].X--;
                }

                if (m1.Y > m2.Y)
                {
                    velocities[m1idx].Y--;
                    velocities[m2idx].Y++;
                }
                else if (m2.Y > m1.Y)
                {
                    velocities[m1idx].Y++;
                    velocities[m2idx].Y--;
                }

                if (m1.Z > m2.Z)
                {
                    velocities[m1idx].Z--;
                    velocities[m2idx].Z++;
                }
                else if (m2.Z > m1.Z)
                {
                    velocities[m1idx].Z++;
                    velocities[m2idx].Z--;
                }
            }
        }

        for (int m1idx = 0; m1idx < moons.Length; m1idx++)
        {
            moons[m1idx].X += velocities[m1idx].X;
            moons[m1idx].Y += velocities[m1idx].Y;
            moons[m1idx].Z += velocities[m1idx].Z;
        }
        
        if (cycle == targetCycles)
        {
            p1 = Enumerable.Range(0, 4).Sum(i =>
            {
                var m = moons[i];
                var v = velocities[i];
                return (Math.Abs(m.X) + Math.Abs(m.Y) + Math.Abs(m.Z)) * (Math.Abs(v.X) + Math.Abs(v.Y) + Math.Abs(v.Z));
            });
        }

        if (p2XCycle == null && moons[0].X == a0.X && moons[1].X == b0.X && moons[2].X == c0.X && moons[3].X == d0.X
        && velocities[0].X == 0 && velocities[1].X == 0 && velocities[2].X == 0 && velocities[3].X == 0)
        {
            p2XCycle = cycle;
        }

        if (p2YCycle == null && moons[0].Y == a0.Y && moons[1].Y == b0.Y && moons[2].Y == c0.Y && moons[3].Y == d0.Y
        && velocities[0].Y == 0 && velocities[1].Y == 0 && velocities[2].Y == 0 && velocities[3].Y == 0)
        {
            p2YCycle = cycle;
        }

        if (p2ZCycle == null && moons[0].Z == a0.Z && moons[1].Z == b0.Z && moons[2].Z == c0.Z && moons[3].Z == d0.Z
        && velocities[0].Z == 0 && velocities[1].Z == 0 && velocities[2].Z == 0 && velocities[3].Z == 0)
        {
            p2ZCycle = cycle;
        }
    }

    long p2 = LCM(new long[] { p2XCycle.Value, p2YCycle.Value, p2ZCycle.Value});
    
    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}

// Shamelessly borrowed: https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers/29717490#29717490
static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}