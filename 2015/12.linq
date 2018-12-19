<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
    Directory.SetCurrentDirectory (Path.GetDirectoryName (Util.CurrentQueryPath));
    var input = File.ReadAllText("12.txt");
    Part1(input).Dump();
    Part2(input).Dump();
}

int Part1(string input)
{
    var numbers = Regex.Matches(input, @"[+-]?\d+(\.\d+)?");
    int sum = 0;
    foreach (Match n in numbers)
    {
        sum += int.Parse(n.Value);
    }
    return sum;
}

int Part2(string input)
{
    JObject json = JObject.Parse(input);
    return JsonObject(json);
}

int JsonObject(JObject json)
{
    int sum = 0;
    foreach (var a in json)
    {
        if (a.Value is JArray)
        {
            sum += JsonArray(a.Value as JArray);
        }
        else if (a.Value is JObject)
        {
            sum += JsonObject(a.Value as JObject);
        }
        else
        {
            string value = a.Value.ToObject<string>();
            if (value == "red") return 0;
            int n = 0;
            int.TryParse(value, out n);
            sum += n;
        }
    }
    return sum;
}

int JsonArray(JArray json)
{
    int sum = 0;
    foreach (var a in json)
    {
        if (a is JArray)
        {
            sum += JsonArray(a as JArray);
        }
        else if (a is JObject)
        {
            sum += JsonObject(a as JObject);
        }
        else
        {
            string value = a.ToObject<string>();
            int n = 0;
            int.TryParse(value, out n);
            sum += n;
        }
    }
    return sum;
}