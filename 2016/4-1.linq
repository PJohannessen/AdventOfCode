<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("4.txt");

    int sectorIdSum = 0;
    foreach (var inputLine in inputLines)
    {
        Dictionary<char, int> occurences = new Dictionary<char, int>();
        int i = 0;
        while (!char.IsNumber(inputLine[i]))
        {
            if (char.IsLetter(inputLine[i]))
            {
                char c = inputLine[i];
                if (occurences.ContainsKey(c)) occurences[c] = occurences[c] + 1;
                else occurences.Add(c, 1);
            }
            i++;
        }
        string sectorId = "";
        while (char.IsNumber(inputLine[i]))
        {
            sectorId += inputLine[i];
            i++;
        }
        string checksum = inputLine.Substring(i + 1, 5);
        string actualChecksum = new string(occurences.OrderByDescending(o => o.Value).ThenBy(o => o.Key).Select(o => o.Key).Take(5).ToArray());
        if (checksum == actualChecksum)
            sectorIdSum += int.Parse(sectorId);
    }
    sectorIdSum.Dump();
}