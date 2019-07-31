<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("10.txt");
    
    List<Scanner> scanners = new List<Scanner>();
    foreach (var il in inputLines)
    {
        var parts = il.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
        scanners.Add(new Scanner(int.Parse(parts[0]), int.Parse(parts[1])));
    }

    int totalSeverityFromZero = CalculateTotalSeverity(scanners, 0);
    $"Part 1: {totalSeverityFromZero}".Dump();
    
    for (int i = 10; i <= int.MaxValue; i++)
    {
        if (scanners.All(s => (i + s.Depth) % ((s.Range - 1) * 2) != 0))
        {
            $"Part 2: {i}".Dump();
            return;
        }
    }
}

public int CalculateTotalSeverity(List<Scanner> scanners, int startAt)
{
    int totalSeverity = 0;
    int maxDepth = scanners.Last().Depth;
    for (int i = 0; i <= maxDepth; i++)
    {
        var currentScanner = scanners.SingleOrDefault(s => s.Depth == i);
        if (currentScanner != null && currentScanner.Position == 0) totalSeverity += currentScanner.Severity;
        foreach (var scanner in scanners)
        {
            scanner.Move();
        }
    }
    
    return totalSeverity;
}

public class Scanner
{
    public Scanner(int depth, int range)
    {
        Depth = depth;
        Range = range;
    }
    
    private bool HeadingDown { get; set; }
    public int Position { get; private set; }
    public int Depth { get; }
    public int Range { get; }
    public int Severity => Depth * Range;
    
    public void Reset()
    {
        HeadingDown = true;
        Position = 0;
    }
    
    public void Move()
    {
        if (HeadingDown)
        {
            if (Position == Range - 1)
            {
                HeadingDown = false;
                Position--;
            }
            else Position++;
        }
        else
        {
            if (Position == 0)
            {
                HeadingDown = true;
                Position++;
            }
            else
            {
                Position--;
            }
        }
    }
    
}