<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    string input = "cxdnnyjw";
    string password = string.Empty;
    int index = 0;
    using (var md5 = MD5.Create())
    {
        while (password.Length < 8)
        {
            char? next = GetNext(md5, string.Concat(input, index.ToString()));
            if (next.HasValue) password += next;
            index++;
        }
    }

    password.Dump();
}

char? GetNext(MD5 md5, string inputString)
{
    var inputBytes = Encoding.UTF8.GetBytes(inputString);
    var hashBytes = md5.ComputeHash(inputBytes);
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < hashBytes.Length; i++)
    {
        sb.Append(hashBytes[i].ToString("x2"));
    }
    string hash = sb.ToString();
    if (hash[0] == '0' && hash[1] == '0' && hash[2] == '0' && hash[3] == '0' && hash[4] == '0') return hash[5];
    return null;
}