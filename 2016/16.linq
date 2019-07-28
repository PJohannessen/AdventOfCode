<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

static string _input = "11101000110010100";
static int _desiredLength = 35651584;

void Main()
{
    string data = _input;
    while (data.Length < _desiredLength)
    {
        data = PadData(data);
    }
    
    data = data.Substring(0, _desiredLength);
    string checksum = data;
    do {
        checksum = Checksum(checksum);
    } while (checksum.Length % 2 == 0);
    checksum.Dump();
}

static string PadData(string a)
{
    string b = (string)a.Clone();
    b = new string(b.Reverse().ToArray());
    b = b.Replace("0", "2");
    b = b.Replace("1", "0");
    b = b.Replace("2", "1");
    return a + "0" + b;
}

static string Checksum(string data)
{
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < data.Length; i = i + 2)
    {
        if (data[i] == data[i+1]) builder.Append("1");
        else builder.Append("0");
    }
    return builder.ToString();
}