<Query Kind="Program" />

static List<Spell> _availableSpells = new List<Spell>
{
    new Spell(
        "Recharge", 229,
        (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana - 229, p.TotalManaSpent + 229), new Boss(b.HP, b.Damage)),
        () => new Effect(
            "Recharge",
            5,
            (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana + 101, p.TotalManaSpent), new Boss(b.HP, b.Damage)),
            (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana, p.TotalManaSpent), new Boss(b.HP, b.Damage))
    )),
    new Spell(
        "Shield", 113,
        (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor + 7, p.Mana - 113, p.TotalManaSpent + 113), new Boss(b.HP, b.Damage)),
        () => new Effect(
            "Shield",
            6,
            (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana, p.TotalManaSpent), new Boss(b.HP, b.Damage)),
            (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor - 7, p.Mana, p.TotalManaSpent), new Boss(b.HP, b.Damage))
    )),
        new Spell(
        "Poison", 173,
        (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana - 173, p.TotalManaSpent + 173), new Boss(b.HP, b.Damage)),
        () => new Effect(
            "Poison",
            6,
            (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana, p.TotalManaSpent), new Boss(b.HP - 3, b.Damage)),
            (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana, p.TotalManaSpent), new Boss(b.HP, b.Damage))
    )),
    new Spell("Drain", 73, (p, b) => new Tuple<Player, Boss>(new Player(p.HP + 2, p.Armor, p.Mana - 73, p.TotalManaSpent + 73), new Boss(b.HP - 2, b.Damage)), null),
    new Spell("Magic Missle", 53, (p, b) => new Tuple<Player, Boss>(new Player(p.HP, p.Armor, p.Mana - 53, p.TotalManaSpent + 53), new Boss(b.HP - 4, b.Damage)), null)
};

static int _optimalMana = int.MaxValue;

void Main()
{
    var initialPlayer = new Player(50, 0, 500, 0);
    var initialBoss = new Boss(55, 8);
    PlayerTurn(initialPlayer, initialBoss, new List<Effect>());
    _optimalMana.Dump();
}

public void PlayerTurn(Player p, Boss b, List<Effect> effects)
{
    if (p.TotalManaSpent > _optimalMana) return;
    
    // Apply effects
    foreach (var e in effects)
    {
        var result = e.Tick(p, b);
        p = result.Item1;
        b = result.Item2;
        
        if (p.HP <= 0 || b.HP <= 0)
        {
            if (p.HP > 0 && b.HP <= 0 && p.TotalManaSpent <= _optimalMana)
                _optimalMana = p.TotalManaSpent;
            return;
        }
    }
    effects = effects.Where(e => e.RemainingTurns > 0).ToList();

    // Player casts
    List<Spell> castable = _availableSpells.Where(s => p.Mana >= s.RequiredMana && effects.All(e => e.Name != s.Name)).ToList();
    if (castable.Count == 0) return;
    foreach (var spell in castable)
    {
        var result = spell.Apply(p, b);
        if (result.Item1.HP <= 0 || result.Item2.HP <= 0)
        {
            if (result.Item1.HP > 0 && result.Item2.HP <= 0 && result.Item1.TotalManaSpent <= _optimalMana)
                _optimalMana = result.Item1.TotalManaSpent;
            return;
        }
        var resultingEffects = effects.Select(e => (Effect)e.Clone()).ToList();
        if (result.Item3 != null) resultingEffects.Add(result.Item3);
        BossTurn(result.Item1, result.Item2, resultingEffects);
    }
}

public void BossTurn(Player p, Boss b, List<Effect> effects)
{
    // Apply effects
    foreach (var e in effects)
    {
        var result = e.Tick(p, b);
        p = result.Item1;
        b = result.Item2;

        if (p.HP <= 0 || b.HP <= 0)
        {
            if (p.HP > 0 && b.HP <= 0 && p.TotalManaSpent <= _optimalMana)
                _optimalMana = p.TotalManaSpent;
            return;
        }
    }
    effects = effects.Where(e => e.RemainingTurns > 0).ToList();

    // Boss attacks
    int dmg = Math.Max(1, b.Damage - p.Armor);
    p = new Player(p.HP - dmg, p.Armor, p.Mana, p.TotalManaSpent);

    if (p.HP <= 0 || b.HP <= 0)
    {
        if (p.HP > 0 && b.HP <= 0 && p.TotalManaSpent <= _optimalMana)
            _optimalMana = p.TotalManaSpent;
        return;
    }

    PlayerTurn(p, b, effects);
}

public class Spell
{
    public Spell(string name, int requiredMana, Func<Player, Boss, Tuple<Player, Boss>> action, Func<Effect> createEffect)
    {
        Name = name;
        RequiredMana = requiredMana;
        Action = action;
        CreateEffect = createEffect;
    }
    public string Name { get; }
    public int RequiredMana { get; }
    private Func<Player, Boss, Tuple<Player, Boss>> Action { get; }
    private Func<Effect> CreateEffect { get; }

    public Tuple<Player, Boss, Effect> Apply(Player player, Boss boss)
    {
        var result = Action(player, boss);
        var effect = CreateEffect != null ? CreateEffect() : null;
        return new Tuple<Player, Boss, Effect>(result.Item1, result.Item2, effect);
    }
}

public class Effect : ICloneable
{
    public string Name { get; }
    public int RemainingTurns { get; private set; }
    private Func<Player, Boss, Tuple<Player, Boss>> TickFunc { get; }
    private Func<Player, Boss, Tuple<Player, Boss>> ExpireFunc { get; }
    
    public Effect(string name, int totalTurns, Func<Player, Boss, Tuple<Player, Boss>> tick, Func<Player, Boss, Tuple<Player, Boss>> expire)
    {
        Name = name;
        RemainingTurns = totalTurns;
        TickFunc = tick;
        ExpireFunc = expire;
    }
    
    public Tuple<Player, Boss> Tick(Player p, Boss b)
    {
        var result = TickFunc(p, b);
        RemainingTurns--;
        if (RemainingTurns == 0)
        {
            result = ExpireFunc(result.Item1, result.Item2);
        }
        return result;
    }

        public object Clone()
        {
            return new Effect(this.Name, this.RemainingTurns, this.TickFunc, this.ExpireFunc);
        }
    }


public struct Player
{
    public Player(int hp, int armor, int mana, int totalManaSpent)
    {
        HP = hp;
        Armor = armor;
        Mana = mana;
        TotalManaSpent = totalManaSpent;
    }
    public int HP { get; }
    public int Armor { get; }
    public int Mana { get; }
    public int TotalManaSpent { get; }
}

public struct Boss
{
    public Boss(int hp, int damage)
    {
        HP = hp;
        Damage = damage;
        
    }
    public int HP { get; }
    public int Damage { get; }
}