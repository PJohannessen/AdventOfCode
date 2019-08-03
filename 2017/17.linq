<Query Kind="Program" />

void Main()
{
    int input = 367;
    int finalLength = 50000000;
    
    LinkedList<int> ll = new LinkedList<int>();
    var currentNode = ll.AddLast(0);
    
    for (int i = 1; i <= finalLength; i++)
    {
        for (int j = 1; j <= input; j++)
        {
            currentNode = currentNode.Next != null ? currentNode.Next : ll.First;
        }
        currentNode = ll.AddAfter(currentNode, i);
    }
    
    currentNode = ll.First;
    while (true)
    {
        if (currentNode.Value == 0)
        {
            currentNode.Next.Value.Dump();
            return;
        }
        else
        {
            currentNode = currentNode.Next;
        }
    }
}