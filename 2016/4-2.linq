<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("4.txt");

    List<string> validRooms = new List<string>();
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
        {
            validRooms.Add(inputLine.Substring(0, i));
        }
    }
    
    foreach (var room in validRooms)
    {
        StringBuilder decryptedRoomBuilder = new StringBuilder();
        int sectorId = int.Parse(room.Substring(room.Length - 3, 3));
        for (int i = 0; i < room.Length - 4; i++)
        {
            if (room[i] == '-') decryptedRoomBuilder.Append(' ');
            else decryptedRoomBuilder.Append(Rotate(room[i], sectorId));
        }
        string decryptedRoom = decryptedRoomBuilder.ToString();
        if (decryptedRoom.Contains("pole")) sectorId.Dump();
    }
}

char Rotate(char c, int num)
{
    for (int i = 1; i <= num; i++)
    {
        if (c == 'z') c = 'a';
        else c++;
    }
    return c;
}