<Query Kind="Program" />

void Main()
{
    string initialState = @"##...#......##......#.####.##.#..#..####.#.######.##..#.####...##....#.#.####.####.#..#.######.##...";
    string instructionsString = @"#.... => .;#..## => #;....# => .;...#. => .;...## => #;#.#.# => .;.#... => #;##.#. => .;..#.# => .;.##.# => #;###.# => #;.#.## => .;..... => .;##### => #;###.. => .;##..# => #;#.### => #;#.#.. => .;..### => .;..#.. => .;.#..# => #;.##.. => #;##... => #;.#.#. => #;.###. => #;#..#. => .;####. => .;.#### => #;#.##. => #;##.## => .;..##. => .;#...# => #";
    long iterations = 250;
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
        PrintPattern(iteration - 1, plants);
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
}

// Used this (in main) to identify that there was a repeating pattern
void PrintPattern(int iteration, Dictionary<int, bool> plants)
{
    for (int i = -100; i <= 500; i++)
    {
        if (plants.ContainsKey(i) && plants[i]) Console.Write('#');
        else Console.Write('.');
    }
    Console.WriteLine();
}

// Used this (in main) to determine the indexes for each plant
void PrintNumbers(int iteration, Dictionary<int, bool> plants)
{
    Console.Write(iteration);
    Console.Write(": ");
    for (int i = 0; i <= 500; i++)
    {
        if (plants.ContainsKey(i) && plants[i])
        {
            Console.Write(i);
            Console.Write(';');
        }
    }
    Console.WriteLine();
}

// Used this to determine what the indexes would be after all the iterations and print the calculation so I can plug it in somewhere else (as even ulong wasn't big enough) 
void Main2()
{
    long need = 50_000_000_000;
    long iteration = 200;
    string numbers = "154;156;159;161;164;166;169;171;174;176;179;181;184;187;190;193;196;199;202;205;208;211;214;217;220;223;226;229;232;235;238;241;244;247;250;253;256;259;262;265;267;270;273;276;279;282;285;288;290;293;296";
    foreach (string number in numbers.Split(';'))
    {
        long sum = ((need - iteration) + long.Parse(number));
        Console.Write(sum);
        Console.Write(" + ");
    }
}