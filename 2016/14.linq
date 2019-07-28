<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

static int _hashCount = 2017;
string _salt = "zpqevtbw";
int _targetCounter = 64;

void Main()
{
    Dictionary<int, Tuple<char, bool>> triplets = new Dictionary<int, Tuple<char, bool>>();
    int counter = 0;
    
    for (int i = 0; i < int.MaxValue; i++)
    {
        int check = i-1001;
        if (triplets.ContainsKey(check))
        {
            if (triplets[check].Item2)
            {
                counter++;
            }
            if (counter == _targetCounter)
            {
                check.Dump();
                return;
            }
            triplets.Remove(check);
        }
        
        string key = _salt + i.ToString();
        string hash = MD5Hash(key);

        for (int j = 0; j < hash.Length - 4; j++)
        {
           if (triplets.Any(t => t.Value.Item1 == hash[j]) && hash[j] == hash[j + 1] && hash[j] == hash[j + 2] && hash[j] == hash[j + 3] && hash[j] == hash[j + 4])
            {
                int[] indecies = triplets.Where(kvp => kvp.Value.Item1 == hash[j]).Select(kvp => kvp.Key).ToArray();
                foreach (int idx in indecies)
                {
                    triplets[idx] = new Tuple<char, bool>(hash[j], true);
                }
            }
        }

        for (int j = 0; j < hash.Length - 2; j++)
        {
            if (hash[j] == hash[j+1] && hash[j] == hash[j+2])
            {
                triplets.Add(i, new Tuple<char, bool>(hash[j], false));
                break;
            }
        }
    }
}

public static string MD5Hash(string inputString)
{
    string current = inputString;
    using (var md5 = MD5.Create())
    {
        for (int j = 1; j <= _hashCount; j++)
        {
            var inputBytes = Encoding.UTF8.GetBytes(current.Trim().ToLower());
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("x2"));
            current = sb.ToString().ToLower();
        }
    }
    return current;
}