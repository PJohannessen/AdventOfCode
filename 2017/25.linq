<Query Kind="Program" />

void Main()
{
    LinkedList<bool> ll = new LinkedList<bool>();
    var current = ll.AddFirst(false);
    
    int target = 12523873;
    State state = State.A;
    
    for (int i = 1; i <= target; i++)
    {
        switch(state)
        {
            case State.A:
                if (current.Value)
                {
                    current.Value = true;
                    Left();
                    state = State.E;
                }
                else
                {
                    current.Value = true;
                    Right();
                    state = State.B;
                }
                break;

            case State.B:
                if (current.Value)
                {
                    current.Value = true;
                    Right();
                    state = State.F;
                }
                else
                {
                    current.Value = true;
                    Right();
                    state = State.C;
                }
                break;

            case State.C:
                if (current.Value)
                {
                    current.Value = false;
                    Right();
                    state = State.B;
                }
                else
                {
                    current.Value = true;
                    Left();
                    state = State.D;
                }
                break;

            case State.D:
                if (current.Value)
                {
                    current.Value = false;
                    Left();
                    state = State.C;
                }
                else
                {
                    current.Value = true;
                    Right();
                    state = State.E;
                }
                break;

            case State.E:
                if (current.Value)
                {
                    current.Value = false;
                    Right();
                    state = State.D;
                }
                else
                {
                    current.Value = true;
                    Left();
                    state = State.A;
                }
                break;

            case State.F:
                if (current.Value)
                {
                    current.Value = true;
                    Right();
                    state = State.C;
                }
                else
                {
                    current.Value = true;
                    Right();
                    state = State.A;
                }
                break;
        }
    }
    
    ll.Count(v => v).Dump();
    
    void Right()
    {
        if (current.Next == null) ll.AddAfter(current, false);
        current = current.Next;
    }

    void Left()
    {
        if (current.Previous == null) ll.AddBefore(current, false);
        current = current.Previous;
    }
}

public enum State
{
    A,
    B,
    C,
    D,
    E,
    F
}