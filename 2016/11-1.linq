<Query Kind="Program" />

void Main()
{
    // Evens are M, Odds are G
    /*
    The first floor contains a strontium generator, a strontium-compatible microchip, a plutonium generator, and a plutonium-compatible microchip.
    The second floor contains a thulium generator, a ruthenium generator, a ruthenium-compatible microchip, a curium generator, and a curium-compatible microchip.
    The third floor contains a thulium-compatible microchip.
    The fourth floor contains nothing relevant.
    */
    //                                      E  SG SM PG PM TG TM RG RM CG CM
    int[] initialStateDetails = new int[] { 3, 3, 3, 3, 3, 2, 1, 2, 2, 2, 2 };
    //int[] initialStateDetails = new int[] { 3, 2, 3, 1, 3 };
    var initialState = new State(initialStateDetails);
    List<State> states = new List<State>() { initialState };
    HashSet<int> seen = new HashSet<int>();
    
    int maxDistance = initialState.DistanceFromGoal();
    int turns = 0;
    while (states.All(s => s.DistanceFromGoal() > 0))
    {
        // Update what we've seen
        foreach (var s in states) seen.Add(s.Hash);

        turns++;
        states = states.SelectMany(s => s.BFS()).Distinct(new StateComparer()).Where(s => s.DistanceFromGoal() <= maxDistance).Where(s => !seen.Contains(s.Hash)).ToList();

        int closest = states.Min(s => s.DistanceFromGoal());
        int upperLimit = closest + 6;
        if (upperLimit < maxDistance) maxDistance = upperLimit;
    }
    
    turns.Dump();
}

public class State
{
    public int[] _state;
    
    public State(int[] state)
    {
        _state = state;
    }

    public int Hash
    {
        get
        {
            int hash =
                1 * _state[0]+
                4 * _state[1]+
                16 * _state[2]+
                64 * _state[3]+
                256*_state[4]+
                1024*_state[5]+
                4096*_state[6]+
                16384*_state[7]+
                65536*_state[8]+
                262144*_state[9]+
                1048576*_state[10];
            return hash;
        }
    }
    
    
    public int DistanceFromGoal()
    {
        int distance = _state.Sum();
        return distance;
    }
    
    public bool IsValidState()
    {
        // Check if Ms without its G is left with another G
        /*
        if (_state[2] != _state[1] && _state[2] == _state[3]) return false;
        if (_state[4] != _state[3] && _state[4] == _state[1]) return false;
        return true;
        */
        if (_state[2] != _state[1] &&
           (_state[2] == _state[3] ||
            _state[2] == _state[5] ||
            _state[2] == _state[7] ||
            _state[2] == _state[9])) return false;
        if (_state[4] != _state[3] &&
           (_state[4] == _state[1] ||
            _state[4] == _state[5] ||
            _state[4] == _state[7] ||
            _state[4] == _state[9])) return false;
        if (_state[6] != _state[5] &&
           (_state[6] == _state[1] ||
            _state[6] == _state[3] ||
            _state[6] == _state[7] ||
            _state[6] == _state[9])) return false;
        if (_state[8] != _state[7] &&
           (_state[8] == _state[1] ||
            _state[8] == _state[3] ||
            _state[8] == _state[5] ||
            _state[8] == _state[9])) return false;
        if (_state[10] != _state[9] &&
           (_state[10] == _state[1] ||
            _state[10] == _state[3] ||
            _state[10] == _state[5] ||
            _state[10] == _state[7])) return false;
        return true;
    }
    
    public IEnumerable<State> BFS()
    {
        int[] onThisFloor = _state.Select((n, idx) => new Tuple<int, int>(n, idx)).Where(p => p.Item2 != 0 && p.Item1 == _state[0]).Select(p => p.Item2).ToArray();
        if (_state[0] > 0) // Can go up
        {
            bool canTakeTwoUp = false;
            if (onThisFloor.Length > 1)
            {
                for (int i = 0; i < onThisFloor.Length - 1; i++)
                {
                    for (int j = i+1; j < onThisFloor.Length; j++)
                    {
                        var newStateB = (int[])_state.Clone();
                        newStateB[0] = newStateB[0] - 1;
                        newStateB[onThisFloor[i]] = newStateB[onThisFloor[i]] - 1;
                        newStateB[onThisFloor[j]] = newStateB[onThisFloor[j]] - 1;
                        var sB = new State(newStateB);
                        if (sB.IsValidState())
                        {
                            yield return sB;
                            canTakeTwoUp = true;
                        }
                    }
                }
            }
            
            if (!canTakeTwoUp)
            {
                foreach (var n in onThisFloor)
                {
                    var newStateA = (int[])_state.Clone();
                    newStateA[0] = newStateA[0] - 1;
                    newStateA[n] = newStateA[n] - 1;
                    var sA = new State(newStateA);
                    if (sA.IsValidState()) yield return sA;
                }
            }
        }
        if (_state[0] < 3) // Can go down
        {
            bool canTakeOneDown = false;
            foreach (var n in onThisFloor)
            {
                var newStateA = (int[])_state.Clone();
                newStateA[0] = newStateA[0] + 1;
                newStateA[n] = newStateA[n] + 1;
                var sA = new State(newStateA);
                if (sA.IsValidState())
                {
                    yield return sA;
                }
            }
            
            if (!canTakeOneDown && onThisFloor.Length > 1)
            {
                for (int i = 0; i < onThisFloor.Length - 1; i++)
                {
                    for (int j = i+1; j < onThisFloor.Length; j++)
                    {
                        var newStateB = (int[])_state.Clone();
                        newStateB[0] = newStateB[0] + 1;
                        newStateB[onThisFloor[i]] = newStateB[onThisFloor[i]] + 1;
                        newStateB[onThisFloor[j]] = newStateB[onThisFloor[j]] + 1;
                        var sB = new State(newStateB);
                        if (sB.IsValidState()) yield return sB;
                    }
                }
            }
        }
    }
}

public class StateComparer : IEqualityComparer<State>
{
    public bool Equals(State x, State y)
    {
        return x.Hash == y.Hash;
    }

    public int GetHashCode(State obj)
    {
        return obj.Hash;
    }
}