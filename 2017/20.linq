<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] steps = File.ReadAllLines("20.txt");
    List<Particle> particles = new List<Particle>();

    int id = 0;
    foreach (var s in steps)
    {
        var regex = new Regex(@"-?\d+");
        var numbers = regex.Matches(s);
        particles.Add(new Particle(id,
            new Tuple<long, long, long>(long.Parse(numbers[0].Value), long.Parse(numbers[1].Value), long.Parse(numbers[2].Value)),
            new Tuple<long, long, long>(long.Parse(numbers[3].Value), long.Parse(numbers[4].Value), long.Parse(numbers[5].Value)),
            new Tuple<long, long, long>(long.Parse(numbers[6].Value), long.Parse(numbers[7].Value), long.Parse(numbers[8].Value))
        ));
        id++;
    }
    
    for (int i = 1; i <= 10000; i++)
    {
        var grouped = particles.GroupBy(p => $"{p.Position.Item1},{p.Position.Item2},{p.Position.Item3}");
        foreach (var p in grouped.Where(g => g.Count() > 1).SelectMany(h => h.Select(j => j)))
        {
            particles.Remove(p);
        }
        foreach (var p in particles) p.Tick();
    }
    
    // P1 (without removing clashes) particles.OrderBy(p => p.Distance()).First().Id.Dump();
    particles.Count.Dump();
}

public class Particle
{
    public Particle(int id, Tuple<long, long, long> position, Tuple<long, long, long> velocity, Tuple<long, long, long> acceleration)
    {
        Id = id;
        Position = position;
        Velocity = velocity;
        Acceleration = acceleration;
    }
    
    public int Id { get; private set; }
    public Tuple<long, long, long> Position { get; private set; }
    public Tuple<long, long, long> Velocity { get; private set; }
    public Tuple<long, long, long> Acceleration { get; private set; }
    
    public void Tick()
    {
        Velocity = new Tuple<long, long, long>(
            Acceleration.Item1 + Velocity.Item1,
            Acceleration.Item2 + Velocity.Item2,
            Acceleration.Item3 + Velocity.Item3);
        Position = new Tuple<long, long, long>(
            Velocity.Item1 + Position.Item1,
            Velocity.Item2 + Position.Item2,
            Velocity.Item3 + Position.Item3);
        
    }
    
    public long Distance()
    {
        return Math.Abs(Position.Item1) + Math.Abs(Position.Item2) + Math.Abs(Position.Item3);
    }
}