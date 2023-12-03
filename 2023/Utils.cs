namespace aoc;

public static class Utils
{   
    public static string[] ParseStrings(string file, bool removeEmpty = false)
    {
        var split = removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
        var strings = File.ReadAllText(file).Split(Environment.NewLine, split).ToArray();
        return strings;
    }
	
    public static int[] ParseInts(string file)
    {
        var ints = File.ReadAllText(file).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
        return ints;
    }

    public static long[] ParseLongs(string file)
    {
        var longs = File.ReadAllText(file).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToArray();
        return longs;
    }
}