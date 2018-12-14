<Query Kind="Program" />

LinkedList<int> recipes;

void Main()
{
    int numOfRecipes = 30000000;
    recipes = new LinkedList<int>();
    var firstElf = recipes.AddLast(3);
    var secondElf = recipes.AddLast(7);
    
    while (recipes.Count < numOfRecipes)
    {
        var sum = firstElf.Value + secondElf.Value;
        foreach (char c in sum.ToString())
        {
            recipes.AddLast(Convert.ToInt32(new string(new[] { c })));
        }
        int e1 = firstElf.Value + 1;
        int e2 = secondElf.Value + 1;
        for (int i = 1; i <= e1; i++)
        {
            firstElf = Next(firstElf);
        }
        for (int j = 1; j <= e2; j++)
        {
            secondElf = Next(secondElf);
        }
    }
    
    var currentRecipe = recipes.First;
    int count = 0;
    while (true)
    {
        if (currentRecipe.Value == 1 &&
        currentRecipe.Next.Value == 5 &&
        currentRecipe.Next.Next.Value == 7 &&
        currentRecipe.Next.Next.Next.Value == 9 &&
        currentRecipe.Next.Next.Next.Next.Value == 0 &&
        currentRecipe.Next.Next.Next.Next.Next.Value == 1)
        {
            count.Dump();
            break;
        }
        count++;
        currentRecipe = currentRecipe.Next;
    }

}

LinkedListNode<int> Next(LinkedListNode<int> currentRecipe)
{
    if (currentRecipe.Next != null) return currentRecipe.Next;
    return recipes.First;
}