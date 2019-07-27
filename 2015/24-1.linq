<Query Kind="Program" />

static List<int> packages;
static int minPackages = 6;
static ulong smallestQE = ulong.MaxValue;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("24.txt");
    packages = inputLines.Select(il => int.Parse(il)).Reverse().ToList();
    int totalWeight = packages.Sum();
    int targetWeight = totalWeight / 3;
    
    SubsetSum(packages, targetWeight, new List<int>());

    $"{minPackages} packages with {smallestQE} QE".Dump();
}

static void SubsetSum(List<int> numbers, int target, List<int> partial)
{
    if (partial.Count > minPackages) return;
    int sum = partial.Sum();
    if (sum == target &&
        partial.Distinct().Count() == partial.Count &&
        partial.Count <= minPackages &&
        RemainingCanBeWeighted(packages.Where(p => !partial.Contains(p)).ToList(), target, new List<int>()))
    {
        ulong qe = 1;
        foreach (int i in partial)
        {
            qe *= (ulong)i;
        }
        if (partial.Count < minPackages)
        {
            minPackages = partial.Count;
            smallestQE = qe;
        }
        else if (qe < smallestQE)
        {
            smallestQE = qe;
        }
    }
    if (sum >= target) return;
    
    foreach (int i in Enumerable.Range(0, numbers.Count))
    {
        int n = numbers[i];
        List<int> remaining = numbers.GetRange(1, numbers.Count - 1);
        SubsetSum(remaining, target, partial.Concat(new[] { n }).ToList());
    }
}

static bool RemainingCanBeWeighted(List<int> numbers, int target, List<int> partial)
{
    int sum = partial.Sum();
    if (sum == target && partial.Distinct().Count() == partial.Count)
    {
        return true;
    }
    if (sum >= target) return false;

    foreach (int i in Enumerable.Range(0, numbers.Count))
    {
        int n = numbers[i];
        List<int> remaining = numbers.GetRange(1, numbers.Count - 1);
        bool canBeWeighted = RemainingCanBeWeighted(remaining, target, partial.Concat(new[] { n }).ToList());
        if (canBeWeighted) return true;
    }

    return false;
}