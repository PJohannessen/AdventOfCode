<Query Kind="Program" />

public static class Utils
{
	public static string[] ParseStrings(string file)
	{
		Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
		var strings = File.ReadAllText(file).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
		return strings;
	}
	
	public static int[] ParseInts(string file)
	{
		Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
		var ints = File.ReadAllText("1.txt").Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
		return ints;
	}
}

public record Point
{
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
	
	public int X { get; init; }
	public int Y { get; init; }
	
	public int Distance(Point p2)
	{
		return Math.Abs(p2.X - X) + Math.Abs(p2.Y - Y);
	}
}