<Query Kind="Program" />

LinkedList<int> recipes;

void Main()
{
    int numOfRecipes = 157901;   
    recipes = new LinkedList<int>();
    var firstElf = recipes.AddLast(3);
    var secondElf = recipes.AddLast(7);
    
    while (recipes.Count < numOfRecipes + 10)
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
    
    var currentLastRecipe = recipes.Last.Previous.Previous.Previous.Previous.Previous.Previous.Previous.Previous.Previous;
    while (currentLastRecipe != null)
    {
        Console.Write(currentLastRecipe.Value);
        currentLastRecipe = currentLastRecipe.Next;
    }
}

LinkedListNode<int> Next(LinkedListNode<int> currentRecipe)
{
    if (currentRecipe.Next != null) return currentRecipe.Next;
    return recipes.First;
}