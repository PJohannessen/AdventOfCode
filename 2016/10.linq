<Query Kind="Program" />

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    string[] inputLines = File.ReadAllLines("10.txt");
    Dictionary<int, Bot> bots = new Dictionary<int, UserQuery.Bot>();
    Dictionary<int, int> outputs = new Dictionary<int, int>();
    
    int target1 = 17;
    int target2 = 61;

    foreach (var line in inputLines.Where(il => il.StartsWith("bot")))
    {
        var instructions = line.Split(' ');
        int bot = int.Parse(instructions[1]);
        bots.Add(bot, new Bot { Id = bot });
        bots[bot].LowRecipientType = (Recipient)Enum.Parse(typeof(Recipient), instructions[5]);
        bots[bot].LowRecipientId = int.Parse(instructions[6]);
        bots[bot].HighRecipientType = (Recipient)Enum.Parse(typeof(Recipient), instructions[10]);
        bots[bot].HighRecipientId = int.Parse(instructions[11]);
    }

    foreach (var line in inputLines.Where(il => il.StartsWith("value")))
    {
        var instructions = line.Split(' ');
        int bot = int.Parse(instructions[5]);
        int value = int.Parse(instructions[1]);
        bots[bot].Holding.Add(value);
    }
    
    while (bots.Values.Any(b => b.Holding.Count > 1))
    {
        var bot = bots.Values.First(b => b.Holding.Count > 1);
        
        if (bot.Holding.Contains(target1) && bot.Holding.Contains(target2))
        {
            bot.Id.Dump();
        }
        
        if (bot.LowRecipientType == Recipient.output)
        {
            if (!outputs.ContainsKey(bot.LowRecipientId)) outputs.Add(bot.LowRecipientId, 0);
            outputs[bot.LowRecipientId] = outputs[bot.LowRecipientId] + bot.Holding.Min();
        }
        else
        {
            bots[bot.LowRecipientId].Holding.Add(bot.Holding.Min());
        }

        if (bot.HighRecipientType == Recipient.output)
        {
            if (!outputs.ContainsKey(bot.HighRecipientId)) outputs.Add(bot.HighRecipientId, 0);
            outputs[bot.HighRecipientId] = outputs[bot.HighRecipientId] + bot.Holding.Max();
        }
        else
        {
            bots[bot.HighRecipientId].Holding.Add(bot.Holding.Max());
        }

        bot.Holding.Clear();
    }
    
    (outputs[0] * outputs[1] * outputs[2]).Dump();
}

public class Bot
{
    public int Id { get; set; }
    public List<int> Holding { get; } = new List<int>();
    public Recipient HighRecipientType { get; set; }
    public int HighRecipientId { get; set; }
    public Recipient LowRecipientType { get; set; }
    public int LowRecipientId { get; set; }
}

public enum Recipient
{
    bot,
    output
}