<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

Dictionary<int, char> _password = new Dictionary<int, char>();

void Main()
{
    string input = "cxdnnyjw";
    int index = 0;
    using (var md5 = MD5.Create())
    {
        while (_password.Count < 8)
        {
            UpdatePassword(md5, string.Concat(input, index.ToString()));
            index++;
        }
    }

    new string (_password.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray()).Dump();
}

void UpdatePassword(MD5 md5, string inputString)
{
    var inputBytes = Encoding.UTF8.GetBytes(inputString);
    var hashBytes = md5.ComputeHash(inputBytes);
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < hashBytes.Length; i++)
    {
        sb.Append(hashBytes[i].ToString("x2"));
    }
    string hash = sb.ToString();
    if (hash[0] == '0' && hash[1] == '0' && hash[2] == '0' && hash[3] == '0' && hash[4] == '0')
    {
        if ("01234567".Contains((hash[5].ToString())) && !_password.ContainsKey(int.Parse(hash[5].ToString())))
        {
            _password.Add(int.Parse(hash[5].ToString()), hash[6]);
        }
    }
}