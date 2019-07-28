<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    int elves = 3017957;
    
    Part1(elves);
    Part2(elves);
}

static void Part1(int elves)
{
    var ll = new LinkedList<int>();
    for (int i = 1; i <= elves; i++) ll.AddLast(i);

    var node = ll.First;
    while (ll.Count > 1)
    {
        ll.Remove(node.Next ?? ll.First);
        node = node.Next ?? ll.First;
    }

    ll.First.Value.Dump();
}

static void Part2(int elves)
{
    LinkedList<int> a = new LinkedList<int>();
    LinkedList<int> b = new LinkedList<int>();
    
    for (int i = 1; i <= elves; i++)
    {
        if (i <= elves / 2) a.AddLast(i);
        else b.AddLast(i);
    }
    
    while ((a.Count + b.Count) > 1)
    {
        int e = a.First.Value;
        a.RemoveFirst();
        if (a.Count != b.Count) b.RemoveFirst();
        else a.RemoveLast();
        b.AddLast(e);
        int e2 = b.First.Value;
        b.RemoveFirst();
        a.AddLast(e2);
    }
    
    a.Single().Dump();
}