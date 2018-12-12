<Query Kind="Program" />

void Main()
{
    string initialState = @"##...#......##......#.####.##.#..#..####.#.######.##..#.####...##....#.#.####.####.#..#.######.##...";
    string instructionsString = @"#.... => .;#..## => #;....# => .;...#. => .;...## => #;#.#.# => .;.#... => #;##.#. => .;..#.# => .;.##.# => #;###.# => #;.#.## => .;..... => .;##### => #;###.. => .;##..# => #;#.### => #;#.#.. => .;..### => .;..#.. => .;.#..# => #;.##.. => #;##... => #;.#.#. => #;.###. => #;#..#. => .;####. => .;.#### => #;#.##. => #;##.## => .;..##. => .;#...# => #";
    long iterations = 20;
    Dictionary<int, bool> plants = new Dictionary<int, bool>();
    for (int i = 0; i < initialState.Length; i++)
    {
        bool hasPlant = initialState[i] == '#';
        plants.Add(i, hasPlant);
    }
    Dictionary<int, bool> instructions = new Dictionary<int, bool>();
    foreach (var instruction in instructionsString.Split(';'))
    {
        int a = (instruction[0] == '#' ? 1 : 0) << 0;
        int b = (instruction[1] == '#' ? 1 : 0) << 1;
        int c = (instruction[2] == '#' ? 1 : 0) << 2;
        int d = (instruction[3] == '#' ? 1 : 0) << 3;
        int e = (instruction[4] == '#' ? 1 : 0) << 4;
        int instructionFlag = a | b | c | d | e;
        bool leadsTo = instruction[9] == '#' ? true : false;
        instructions.Add(instructionFlag, leadsTo);
    }
    
    for (int iteration = 1; iteration <= iterations; iteration++)
    {
        Dictionary<int, bool> newPlants = new Dictionary<int, bool>();
        for (int i = plants.Keys.Min() - 3; i < plants.Keys.Max() + 3; i++)
        {
            int a = ((plants.ContainsKey(i - 2) ? plants[i - 2] : false) ? 1 : 0) << 0;
            int b = ((plants.ContainsKey(i - 1) ? plants[i - 1] : false) ? 1 : 0) << 1;
            int c = ((plants.ContainsKey(i ) ? plants[i] : false) ? 1 : 0) << 2;
            int d = ((plants.ContainsKey(i + 1) ? plants[i + 1] : false) ? 1 : 0) << 3;
            int e = ((plants.ContainsKey(i + 2) ? plants[i + 2] : false) ? 1 : 0) << 4;
            int instructionFlag = a | b | c | d | e;
            if (instructions.ContainsKey(instructionFlag) && instructions[instructionFlag]) newPlants.Add(i, true);
            else newPlants.Add(i, false);
        }
        
        plants = newPlants;
    }
    
    plants.Where(kvp => kvp.Value).Sum(kvp => kvp.Key).Dump();
}