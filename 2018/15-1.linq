<Query Kind="Program">
  <NuGetReference>Dijkstra.NET</NuGetReference>
  <Namespace>Dijkstra.NET.Model</Namespace>
  <Namespace>Dijkstra.NET.Extensions</Namespace>
</Query>

int width = 0;
int height = 0;
List<Unit> units = new List<Unit>();
Cell[,] grid;
int completedLoops = 0;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllText("15.txt").Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    height = inputLines.Length;
    width = inputLines[0].Length;
    grid = new Cell[width, height];
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            switch (inputLines[y][x])
            {
                case '#':
                    grid[x,y] = Cell.Wall;
                    break;
                case '.':
                    break;
                case 'G':
                    units.Add(new Unit{Type = UnitType.Goblin, PosX = x, PosY = y});
                    break;
                case 'E':
                    units.Add(new Unit{Type = UnitType.Elf, PosX = x, PosY = y});
                    break;
            }
        }
    }
    GameLoop();
    (completedLoops * units.Where(u => u.Alive).Sum(u => u.HP)).Dump();
}

public void GameLoop()
{
    while (true)
    {
        units = units.OrderBy(u => u.PosY).ThenBy(u => u.PosX).ToList();
        foreach (var unit in units)
        {
            if (units.Where(u => u.Type == UnitType.Elf).All(u => !u.Alive) || units.Where(u => u.Type == UnitType.Goblin).All(u => !u.Alive))
            {
                return;
            }
            if (!unit.Alive) continue;
            if (units.Where(u2 => unit.Type != u2.Type && u2.Alive).Any(u2 => GetDistance(unit, u2) == 1))
            {
                Attack(unit);
            }
            else
            {
                var graph = new Graph<TargetSpace, string>();
                var availableSpaces = new List<TargetSpace>();
                BuildGraph(graph, availableSpaces, unit);
                var closestTarget = GetClosestTarget(graph, availableSpaces, unit);
                if (closestTarget != null)
                {
                    var nextSpace = GetClosestSpace(graph, availableSpaces, unit, closestTarget);
                    if (nextSpace != null)
                    {
                        unit.PosX = nextSpace.PosX;
                        unit.PosY = nextSpace.PosY;
                        Attack(unit);
                    }
                }
            }
        }
        completedLoops++;
    }
}

public void Attack(Unit unit)
{
    var target = units.Where(u2 => u2.Type != unit.Type && u2.Alive).Where(u2 => GetDistance(unit, u2) == 1).OrderBy(u2 => u2.HP).ThenBy(u2 => u2.PosY).ThenBy(u2 => u2.PosX).FirstOrDefault();
    if (target != null) target.HP = target.HP - 3;
}

public TargetSpace GetClosestTarget(Graph<TargetSpace, string> graph, List<TargetSpace> allSpaces, Unit sourceUnit)
{
    var a = allSpaces.SingleOrDefault(space => space.PosX == sourceUnit.PosX && space.PosY == sourceUnit.PosY);
    var targetUnits = units.Where(u => u.Type != sourceUnit.Type && u.Alive).ToList();
    List<TargetSpace> targetSpaces = new List<TargetSpace>();
    foreach (var targetUnit in targetUnits)
    {
        var b = allSpaces.SingleOrDefault(sp => sp.PosX == targetUnit.PosX - 1 && sp.PosY == targetUnit.PosY);
        var c = allSpaces.SingleOrDefault(sp => sp.PosX == targetUnit.PosX + 1 && sp.PosY == targetUnit.PosY);
        var d = allSpaces.SingleOrDefault(sp => sp.PosX == targetUnit.PosX && sp.PosY == targetUnit.PosY - 1);
        var e = allSpaces.SingleOrDefault(sp => sp.PosX == targetUnit.PosX && sp.PosY == targetUnit.PosY + 1);
        if (b != null) targetSpaces.Add(b);
        if (c != null) targetSpaces.Add(c);
        if (d != null) targetSpaces.Add(d);
        if (e != null) targetSpaces.Add(e);
    }

    var matches = targetSpaces.Where(ts => graph.Dijkstra(a.Id, ts.Id).IsFounded)
                       .OrderBy(ts => graph.Dijkstra(a.Id, ts.Id).Distance)
                       .ThenBy(ts => ts.PosY)
                       .ThenBy(ts => ts.PosX)
                       .ToList();
    return matches.FirstOrDefault();
}

public TargetSpace GetClosestSpace(Graph<TargetSpace, string> graph, List<TargetSpace> availableSpaces, Unit sourceUnit, TargetSpace targetSpace)
{
    var a = availableSpaces.SingleOrDefault(space => space.PosX == sourceUnit.PosX && space.PosY == sourceUnit.PosY);
    var b = availableSpaces.SingleOrDefault(sp => sp.PosX == sourceUnit.PosX && sp.PosY == sourceUnit.PosY - 1);
    var c = availableSpaces.SingleOrDefault(sp => sp.PosX == sourceUnit.PosX - 1 && sp.PosY == sourceUnit.PosY);
    var d = availableSpaces.SingleOrDefault(sp => sp.PosX == sourceUnit.PosX + 1 && sp.PosY == sourceUnit.PosY);
    var e = availableSpaces.SingleOrDefault(sp => sp.PosX == sourceUnit.PosX && sp.PosY == sourceUnit.PosY + 1);
    uint best = uint.MaxValue;
    double distance = double.MaxValue;
    if (b != null)
    {
        var currentPath = graph.Dijkstra(b.Id, targetSpace.Id);
        if (currentPath.Distance < distance)
        {
            distance = currentPath.Distance;
            best = b.Id;
        }
    }
    if (c != null)
    {
        var currentPath = graph.Dijkstra(c.Id, targetSpace.Id);
        if (currentPath.Distance < distance)
        {
            distance = currentPath.Distance;
            best = c.Id;
        }
    }
    if (d != null)
    {
        var currentPath = graph.Dijkstra(d.Id, targetSpace.Id);
        if (currentPath.Distance < distance)
        {
            distance = currentPath.Distance;
            best = d.Id;
        }
    }
    if (e != null)
    {
        var currentPath = graph.Dijkstra(e.Id, targetSpace.Id);
        if (currentPath.Distance < distance)
        {
            distance = currentPath.Distance;
            best = e.Id;
        }
    }
    return availableSpaces.Where(space => space.Id == best).Single();
}

public class TargetSpace
{
    public TargetSpace(int posX, int posY) { PosX = posX; PosY = posY; }
    public int PosX { get; }
    public int PosY { get; }
    public uint Id { get; set; }
}

public double GetDistance(Unit u1, Unit u2)
{
    return Math.Abs(u2.PosY - u1.PosY) + Math.Abs(u2.PosX - u1.PosX);
}

public enum Cell
{
    Space,
    Wall
}

public class Unit
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public UnitType Type { get; set; }
    public int HP { get; set; } = 200;
    public bool Alive
    {
        get
        {
            return HP > 0;
        }
    }
}

public enum UnitType
{
    Goblin,
    Elf
}

void Print()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var unit = units.SingleOrDefault(u => u.Alive && u.PosX == x && u.PosY == y);
            if (unit != null && unit.Type == UnitType.Goblin) Console.Write('G');
            else if (unit != null && unit.Type == UnitType.Elf) Console.Write('E');
            else if (grid[x, y] == Cell.Space) Console.Write('.');
            else if (grid[x, y] == Cell.Wall) Console.Write('#');
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

void BuildGraph(Graph<TargetSpace, string> graph, List<TargetSpace> availableSpaces, Unit sourceUnit)
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var isOtherUnit = units.Any(u => u != sourceUnit && u.Alive && u.PosX == x && u.PosY == y);
            var isWall = grid[x, y] == Cell.Wall;
            if (!isOtherUnit && !isWall)
            {
                var ts = new TargetSpace(x, y);
                availableSpaces.Add(ts);
                ts.Id = graph.AddNode(ts);
            }
        }
    }
    foreach (var availableSpace in availableSpaces)
    {
        var a = availableSpaces.SingleOrDefault(ts2 => ts2.PosX == availableSpace.PosX && ts2.PosY == availableSpace.PosY);
        var b = availableSpaces.SingleOrDefault(ts2 => ts2.PosX == a.PosX - 1 && ts2.PosY == a.PosY);
        var c = availableSpaces.SingleOrDefault(ts2 => ts2.PosX == a.PosX + 1 && ts2.PosY == a.PosY);
        var d = availableSpaces.SingleOrDefault(ts2 => ts2.PosX == a.PosX && ts2.PosY == a.PosY - 1);
        var e = availableSpaces.SingleOrDefault(ts2 => ts2.PosX == a.PosX && ts2.PosY == a.PosY + 1);
        if (b != null) graph.Connect(a.Id, b.Id, 1, null);
        if (c != null) graph.Connect(a.Id, c.Id, 1, null);
        if (d != null) graph.Connect(a.Id, d.Id, 1, null);
        if (e != null) graph.Connect(a.Id, e.Id, 1, null);
    }
}