<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string input = File.ReadAllText("8.txt");
    int w = 25;
    int h = 6;
    int layerSize = w*h;
      
    List<int[]> layers = new List<int[]>();
    for (int i = 0; i < input.Length; i += layerSize)
    {
        var layer = input.Select(c => int.Parse(c.ToString()))
                         .Skip(i)
                         .Take(layerSize)
                         .ToArray();
        layers.Add(layer);
    }
    var layerWithMostZeros = layers.OrderBy(l => l.Where(n => n == 0).Count())
                                   .First();

    var p1 = (layerWithMostZeros.Where(n => n == 1).Count() * layerWithMostZeros.Where(n => n == 2).Count());
    $"P1: {p1}".Dump();
    
    "P2:".Dump();
    for (int y = 0; y < h; y++)
    {
        for (int x = 0; x < w; x++)
        {
            int cell = 2;
            int cellIndex = (y * w) + x;
            for (int layerIndex = 0; layerIndex < layers.Count; layerIndex++)
            {
                cell = layers[layerIndex][cellIndex];
                if (cell == 2) { continue; }
                else { break; }
            }
            
            if (cell == 2) Console.Write(" ");
            if (cell == 1) Console.Write(" ");
            if (cell == 0) Console.Write("█");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}