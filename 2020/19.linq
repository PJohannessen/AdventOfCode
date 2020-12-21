<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

public static Dictionary<int, Rule> Rules;

void Main()
{
    var inputStrings = Utils.ParseStrings("19.txt", true);
    var ruleStrings = inputStrings.Where(s => char.IsDigit(s[0])).ToList();
    var messages = inputStrings.Where(s => !char.IsDigit(s[0])).ToList();
    Rules = ParseRules(ruleStrings);

    int p1 = messages.Where(m => TestString(m)).Count();

    Rules[8] = new PointerRule
    {
        SubRules = new List<int[]>
        {
            new int[] { 42 },
            new int[] { 42, 8 }
        }
    };
    Rules[11] = new PointerRule
    {
        SubRules = new List<int[]>
        {
            new int[] { 42, 31 },
            new int[] { 42, 11, 31 }
        }
    };
    int p2 = messages.Where(m => TestString(m)).Count();

    $"P1: {p1}".Dump();
    $"P2: {p2}".Dump();
}

public bool TestString(string s)
{
    Stack<(int StringIndex, List<int[]> Stack)> stack = new();
    List<int[]> prevStack = new List<int[]>();
    prevStack.Add(new int[] { 0 });
    stack.Push((0, prevStack));
    while (stack.Count > 0)
    {
        var check = stack.Pop();
        var currentRules = check.Stack.Last();
        check.Stack = check.Stack.GetRange(0, check.Stack.Count - 1);
        var ruleId = currentRules.First();
        var nextRules = currentRules[1..];
        var rule = Rules[currentRules.First()];
        if (rule is LetterRule)
        {
            if (check.StringIndex >= s.Length || s[check.StringIndex] != ((LetterRule)rule).C) continue;
            int newIndex = check.StringIndex+1;
            while (nextRules.Length == 0)
            {
                if (check.Stack.Count == 0)
                {
                    if (newIndex == s.Length) return true;
                    break;
                }
                currentRules = check.Stack.Last();
                check.Stack = check.Stack.GetRange(0, check.Stack.Count - 1);
                nextRules = currentRules;
            }
            if (check.Stack.Count > 0)
            {
                var next = new List<int[]>(check.Stack);
                if (nextRules.Length > 0)
                {
                    next.Add(nextRules);
                    stack.Push((newIndex, next));
                }
            }
        }
        else
        {
            var pointerRule = (PointerRule)rule;
            foreach (var sr in pointerRule.SubRules)
            {
                var newStack = new List<int[]>(check.Stack);
                newStack.Add(nextRules);
                newStack.Add(sr);
                stack.Push((check.StringIndex, newStack));
            }
        }
    }
    return false;
}

public Dictionary<int, Rule> ParseRules(List<string> ruleStrings)
{
    Dictionary<int, Rule> rules = new();
    foreach (var s in ruleStrings)
    {
        var split = s.Split(new[] { ": ", " " }, StringSplitOptions.RemoveEmptyEntries);
        var ruleId = int.Parse(split[0]);

        int secondRuleId = 0;
        bool isInt = int.TryParse(split[1], out secondRuleId);
        if (!isInt)
        {
            rules.Add(ruleId, new LetterRule { C = split[1][1] });
        }
        else
        {
            List<int[]> subRules = new();
            List<int> currentRules = new();
            for (int i = 1; i < split.Length; i++)
            {
                if (split[i] == "|")
                {
                    subRules.Add(currentRules.ToArray());
                    currentRules = new();
                }
                else
                {
                    currentRules.Add(int.Parse(split[i]));
                }
            }
            subRules.Add(currentRules.ToArray());
            rules.Add(ruleId, new PointerRule { SubRules = subRules });
        }
    }
    
    return rules;
}

public class LetterRule : Rule
{
    public char C { get; init; }
}

public class PointerRule : Rule
{
    public List<int[]> SubRules { get; init; }
}

public abstract class Rule
{
}
