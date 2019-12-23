<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

// I cheated the hell out of this one and I don't feel bad.
// Thank you to u/mcpower_ for explaining something I still don't understand: https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbnkaju/

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("22.txt");
    
    BigInteger cards = 119315717514047;
    BigInteger repeats = 101741582076661;
    BigInteger lookAt = 2020;
    
    BigInteger increment_mul = 1;
    BigInteger offset_diff = 0;

    foreach (var line in inputLines)
    {
        if (line.StartsWith("deal into"))
        {
            increment_mul = (increment_mul * -1) % cards;
            offset_diff = (offset_diff + increment_mul) % cards;
        }
        else if (line.StartsWith("cut"))
        {
            var n = BigInteger.Parse(line.Split(' ').Last());
            offset_diff = (offset_diff + n * increment_mul) % cards;
        }
        else
        {
            var n = BigInteger.Parse(line.Split(' ').Last());
            increment_mul = (increment_mul * Inv(n)) % cards;
        }
    }
    
    (BigInteger increment, BigInteger offset) = Sequence(repeats);
    var answer = Nth(offset, increment, lookAt);
    if (answer < 0) answer = cards + answer;
    answer.Dump();
    
    BigInteger Inv(BigInteger n)
    {
        return BigInteger.ModPow(n, cards-2, cards);
    }
    
    BigInteger Nth(BigInteger offset, BigInteger increment, BigInteger i)
    {
        return (offset + i * increment) % cards;
    }
    
    (BigInteger increment, BigInteger offset) Sequence(BigInteger interations)
    {
        var increment = BigInteger.ModPow(increment_mul, interations, cards);
        var offset = offset_diff * (1 - increment) * Inv((1 - increment_mul) % cards);
        offset = offset % cards;
        return (increment, offset);
    }
}