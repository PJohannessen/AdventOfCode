<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    string startingPasscode = "vwbaicqe";
    int startingX = 0;
    int startingY = 0;
    int worstSolution = int.MinValue;
    string worstPath = string.Empty;
    int maxAttempt = 1000;
    
    ProcessPosition(startingX, startingY, startingPasscode);
    
     $"P1: {worstSolution - startingPasscode.Length}".Dump();
    
    void ProcessPosition(int x, int y, string passcode)
    {
        if (passcode.Length > maxAttempt) return;
        if (x == 3 && y == 3)
        {
            if (passcode.Length > worstSolution)
            {
                worstSolution = passcode.Length;
                worstPath = passcode;
            }
            return;
        }
        
        string hash = MD5Hash(passcode);
        bool canUp = "bcdef".Contains(hash[0]);
        bool canDown = "bcdef".Contains(hash[1]);
        bool canLeft = "bcdef".Contains(hash[2]);
        bool canRight = "bcdef".Contains(hash[3]);
        
        if (canUp && y > 0) ProcessPosition(x, y-1, passcode + "U");
        if (canDown && y < 3) ProcessPosition(x, y+1, passcode + "D");
        if (canLeft && x > 0) ProcessPosition(x-1, y, passcode + "L");
        if (canRight && x < 3) ProcessPosition(x+1, y, passcode + "R");
    }
    
}

public string MD5Hash(string inputString)
{
    using (var md5 = MD5.Create())
    {
        var inputBytes = Encoding.UTF8.GetBytes(inputString);
        var hashBytes = md5.ComputeHash(inputBytes);
        var sb = new StringBuilder();
        for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("x2"));
        return sb.ToString();
    }
}