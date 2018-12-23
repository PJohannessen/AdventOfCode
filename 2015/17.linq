<Query Kind="Program" />

Dictionary<int, int> containers = new Dictionary<int, int>();
int total = 0;

void Main()
{
    int[] numbers = new int[] { 33, 14, 18, 20, 45, 35, 16, 35, 1, 13, 18, 13, 50, 44, 48, 6, 24, 41, 30, 42 };
    SubsetSum(numbers, 150, new int[] {});
    $"P1: {total}".Dump();
    $"P2: {containers[containers.Keys.Min()]}".Dump();
}

void SubsetSum(int[] numbers, int target, int[] partial)
{
    int sum = partial.Sum();
    if (sum == target)
    {
        total++;
        if (containers.ContainsKey(partial.Length)) containers[partial.Length] = containers[partial.Length] + 1;
        else containers.Add(partial.Length, 1);
    }
    if (sum >= target) return;
    for (int i = 0; i < numbers.Length; i++)
    {
        int n = numbers[i];
        int[] remaining = numbers.Skip(i + 1).ToArray();
        SubsetSum(remaining, target, partial.Concat(new[] { n }).ToArray());
    }
}
