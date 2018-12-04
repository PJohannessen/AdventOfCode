<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	string baseKey = "bgvyzdsv";
	int i = 1;
	while (true)
	{
		string key = baseKey + i.ToString();
		if (MD5Hash(key).Substring(0, 5) == "00000")
		{
			i.Dump();
			return;
		}
		i++;
	}
}

public string MD5Hash(string inputString)
{
	using (var md5 = MD5.Create())
	{
		var inputBytes = Encoding.UTF8.GetBytes(inputString.Trim().ToLower());
		var hashBytes = md5.ComputeHash(inputBytes);
		var sb = new StringBuilder();
		for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("x2"));
		return sb.ToString();
	}
}