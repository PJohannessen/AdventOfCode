<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("21.txt");

    string p1Unscrambled = "abcdefgh";
    string p1Scrambled = Scramble(p1Unscrambled);
    $"P1: {p1Scrambled}".Dump();
    
    // Just brute force it =D
    string p2Scrambled = "fbgdceah";
    foreach (var p in Permutations(p1Unscrambled))
    {
        var unscrambledPerm = new string(p.ToArray());
        var scrambledPerm = Scramble(unscrambledPerm);
        if (p2Scrambled == scrambledPerm)
        {
            $"P2: {unscrambledPerm}".Dump();
            return;
        }
    }
    
    string Scramble(string password)
    {
        foreach (var input in inputLines)
        {
            if (input.StartsWith("swap position")) password = SwapPosition(input, password);
            else if (input.StartsWith("swap letter")) password = SwapLetter(input, password);
            else if (input.StartsWith("rotate based")) password = RotatePosition(input, password);
            else if (input.StartsWith("rotate")) password = RotateLeftRight(input, password);
            else if (input.StartsWith("reverse")) password = ReversePositions(input, password);
            else if (input.StartsWith("move")) password = Move(input, password);
        }
        return password;
    }
}

// swap position X with position Y
static string SwapPosition(string input, string password)
{
    char[] passwordArray = password.ToCharArray();
    int[] positions = input.Split(new[] { "swap position ", " with position "}, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
    char a = passwordArray[positions[0]];
    char b = passwordArray[positions[1]];
    passwordArray[positions[1]] = a;
    passwordArray[positions[0]] = b;
    return new string(passwordArray);
}

// swap letter X with letter Y
static string SwapLetter(string input, string password)
{
    char[] letters = input.Split(new[] { "swap letter ", " with letter "}, StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToArray();
    char a = letters[0];
    char b = letters[1];
    password = password.Replace(a, '*');
    password = password.Replace(b, a);
    password = password.Replace('*', b);
    return password;
}

// rotate left/right X steps
static string RotateLeftRight(string input, string password)
{
    if (input.StartsWith("rotate left"))
    {
        int shift = input.Split(new[] { "rotate left ", " steps", " step"}, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray().Single();
        var p1 = password.Substring(shift);
        var p2 = password.Substring(0, shift);
        password = new string(p1.Concat(p2).ToArray());
    }
    else
    {
        int shift = input.Split(new[] { "rotate right ", " steps", " step"}, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray().Single();
        var p1 = password.Substring(password.Length - shift);
        var p2 = password.Substring(0, password.Length - shift);
        password = new string(p1.Concat(p2).ToArray());
    }
    return password;
}

// rotate based on position of letter X
static string RotatePosition(string input, string password)
{
    char c = input.Last();
    int idx = password.IndexOf(c);
    int shift = (idx+1+(idx >= 4 ? 1 : 0)) % password.Length;
    var p1 = password.Substring(password.Length - shift);
    var p2 = password.Substring(0, password.Length - shift);
    password = new string(p1.Concat(p2).ToArray());
    return password;
}

// reverse positions 0 through 4 // causes the entire string to be reversed, producing abcde.
static string ReversePositions(string input, string password)
{
    int[] positions = input.Split(new[] { "reverse positions ", " through "}, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
    int a = positions[0];
    int b = positions[1];
    
    char[] p1 = password.Substring(0, a).ToArray();
    char[] p2 = password.Substring(a, b-a+1).Reverse().ToArray();
    char[] p3 = password.Substring(b+1).ToArray();
    password = new string(p1.Concat(p2).Concat(p3).ToArray());
    return password;
}

// move position X to position Y
static string Move(string input, string password)
{
    int[] positions = input.Split(new[] { "move position ", " to position "}, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
    List<char> passwordList = new List<char>(password);
    char a = passwordList[positions[0]];
    passwordList.RemoveAt(positions[0]);
    if (positions[1] == passwordList.Count) passwordList.Add(a);
    else passwordList.Insert(positions[1], a);
    return new string(passwordList.ToArray());
}

public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> source)
{
    if (source == null) throw new ArgumentNullException("source");
    return permutations(source.ToArray());
}

private static IEnumerable<IEnumerable<T>> permutations<T>(IEnumerable<T> source)
{
    var c = source.Count();
    if (c == 1) yield return source;
    else
    {
        for (int i = 0; i < c; i++)
        {
            foreach (var p in permutations(source.Take(i).Concat(source.Skip(i + 1))))
            {
                yield return source.Skip(i).Take(1).Concat(p);
            }
        }
    }

}
