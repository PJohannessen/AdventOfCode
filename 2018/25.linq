<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("25.txt");
    List<Coordinate> coordinates = new List<UserQuery.Coordinate>();
    foreach (var il in inputLines)
    {
        int[] split = il.Split(',').Select(n => int.Parse(n)).ToArray();
        coordinates.Add(new Coordinate { A = split[0], B = split[1], C = split[2], D = split[3]});
    }
    Dictionary<int, List<Coordinate>> constellations = new Dictionary<int, System.Collections.Generic.List<UserQuery.Coordinate>>();
    int count = 0;
    while (coordinates.Count > 0)
    {
        constellations.Add(count, new List<Coordinate> { coordinates.First() });
        coordinates.RemoveAt(0);
        while (true)
        {
            List<Coordinate> inRange = new List<Coordinate>();
            foreach (var c in constellations[count])
            {
                inRange = inRange.Concat(coordinates.Where(co => co.Distance(c) <= 3)).ToList();
            }
            inRange = inRange.Distinct().ToList();
            if (inRange.Any())
            {
                coordinates.RemoveAll(c => inRange.Contains(c));
                constellations[count] = constellations[count].Concat(inRange).ToList();
            }
            else
            {
                count++;
                break;
            }
        }
    }
    constellations.Count.Dump();
}

public class Coordinate
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public int D { get; set; }
    public int Distance(Coordinate c)
    {
        return Math.Abs(A - c.A) + Math.Abs(B - c.B) + Math.Abs(C - c.C) + Math.Abs(D - c.D);
    }
}
