<Query Kind="Program" />

// Part 1: Boost 0
// Part 2: Boost 38
const int Boost = 38;

void Main()
{
    List<Group> groups = new List<Group>
    {
       new Group { Id = 1, Team = Team.Immune, Units = 2086, HP = 11953, Damage = 46, AttackType = DamageType.Cold, Initiative = 13 },
       new Group { Id = 2, Team = Team.Immune, Units = 329, HP = 3402, WeakTo = DamageType.Bludgeoning, Damage = 90, AttackType = DamageType.Slashing, Initiative = 1 },
       new Group { Id = 3, Team = Team.Immune, Units = 414, HP = 7103, WeakTo = DamageType.Bludgeoning, ImmuneTo = DamageType.Radiation, Damage = 170, AttackType = DamageType.Radiation, Initiative = 4 },
       new Group { Id = 4, Team = Team.Immune, Units = 2205, HP = 7118, WeakTo = DamageType.Fire, ImmuneTo = DamageType.Cold, Damage = 26, AttackType = DamageType.Radiation, Initiative = 18 },
       new Group { Id = 5, Team = Team.Immune, Units = 234, HP = 9284, WeakTo = DamageType.Slashing, ImmuneTo = DamageType.Cold | DamageType.Fire, Damage = 287, AttackType = DamageType.Radiation, Initiative = 12 },
       new Group { Id = 6, Team = Team.Immune, Units = 6460, HP = 10804, WeakTo = DamageType.Fire, Damage = 15, AttackType = DamageType.Slashing, Initiative = 11 },
       new Group { Id = 7, Team = Team.Immune, Units = 79, HP = 1935, Damage = 244, AttackType = DamageType.Radiation, Initiative = 8 },
       new Group { Id = 8, Team = Team.Immune, Units = 919, HP = 2403, WeakTo = DamageType.Fire, Damage = 22, AttackType = DamageType.Slashing, Initiative = 2 },
       new Group { Id = 9, Team = Team.Immune, Units = 172, HP = 1439, WeakTo = DamageType.Slashing, ImmuneTo = DamageType.Cold | DamageType.Fire, Damage = 69, AttackType = DamageType.Slashing, Initiative = 3 },
       new Group { Id = 10, Team = Team.Immune, Units = 1721, HP = 2792, WeakTo = DamageType.Radiation | DamageType.Fire, Damage = 13, AttackType = DamageType.Cold, Initiative = 16 },
       
       new Group { Id = 11, Team = Team.Infection, Units = 1721, HP = 29925, WeakTo = DamageType.Cold | DamageType.Radiation, ImmuneTo = DamageType.Slashing, Damage = 34, AttackType = DamageType.Radiation, Initiative = 5 },
       new Group { Id = 12, Team = Team.Infection, Units = 6351, HP = 21460, WeakTo = DamageType.Cold, Damage = 6, AttackType = DamageType.Slashing, Initiative = 15 },
       new Group { Id = 13, Team = Team.Infection, Units = 958, HP = 48155, WeakTo = DamageType.Bludgeoning, Damage = 93, AttackType = DamageType.Radiation, Initiative = 7 },
       new Group { Id = 14, Team = Team.Infection, Units = 288, HP = 41029, WeakTo = DamageType.Radiation, ImmuneTo = DamageType.Bludgeoning, Damage = 279, AttackType = DamageType.Cold, Initiative = 20 },
       new Group { Id = 15, Team = Team.Infection, Units = 3310, HP = 38913, Damage = 21, AttackType = DamageType.Radiation, Initiative = 19 },
       new Group { Id = 16, Team = Team.Infection, Units = 3886, HP = 16567, ImmuneTo = DamageType.Bludgeoning | DamageType.Cold, Damage = 7, AttackType = DamageType.Cold, Initiative = 9 },
       new Group { Id = 17, Team = Team.Infection, Units = 39, HP = 7078, Damage = 300, AttackType = DamageType.Bludgeoning, Initiative = 14 },
       new Group { Id = 18, Team = Team.Infection, Units = 241, HP = 40635, WeakTo = DamageType.Cold, Damage = 304, AttackType = DamageType.Fire, Initiative = 6 },
       new Group { Id = 19, Team = Team.Infection, Units = 7990, HP = 7747, ImmuneTo = DamageType.Fire, Damage = 1, AttackType = DamageType.Radiation, Initiative = 10 },
       new Group { Id = 20, Team = Team.Infection, Units = 80, HP = 30196, WeakTo = DamageType.Fire, Damage = 702, AttackType = DamageType.Bludgeoning, Initiative = 17 },
    };
    
    while (groups.Any(t => t.Team == Team.Infection) && groups.Any(t => t.Team == Team.Immune))
    {
        List<KeyValuePair<int, int>> attacks = new List<KeyValuePair<int, int>>();
        foreach (var g in groups.OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative))
        {
            var availableTargets = groups.Where(e => e.Team != g.Team).Where(e => !attacks.Any(a => a.Value == e.Id)).ToList();
            var target = availableTargets.OrderByDescending(e => g.DamageCalc(e)).ThenByDescending(e => e.EffectivePower).ThenByDescending(e => e.Initiative).FirstOrDefault();
            if (target != null && g.DamageCalc(target) > 0)
            {
                attacks.Add(new KeyValuePair<int, int>(g.Id, target.Id));
            }
        }
        foreach (var a in attacks.OrderByDescending(a => groups.Single(g => g.Id == a.Key).Initiative))
        {
            Group group = groups.SingleOrDefault(g => g.Id == a.Key);
            Group target = groups.SingleOrDefault(g => g.Id == a.Value);
            if (group != null && target != null)
            {
                int damage = group.DamageCalc(target);
                int killedUnits = Math.Min(target.Units, (int)Math.Floor((double)(damage / target.HP)));
                target.Units -= killedUnits;
                if (target.Units <= 0) groups.Remove(target);
            }
        }
    }
    
    groups.First().Team.Dump();
    groups.Sum(g => g.Units).Dump();
}

public class Group
{
    public int Id { get; set; }
    public Team Team { get; set; }
    public int Units { get; set; }
    public int HP { get; set; }
    public DamageType WeakTo { get; set; }
    public DamageType ImmuneTo { get; set; }
    public DamageType AttackType { get; set; }
    public int _damage;
    public int Damage
    {
        get
        {
            if (Team == Team.Immune) return Boost + _damage; else return _damage;
        }
        set { _damage = value; }
    }
    public int Initiative { get; set; }
    public int EffectivePower { get { return Units * Damage; } } 
    
    public int DamageCalc(Group enemy)
    {
        int multipler = 1;
        if (enemy.ImmuneTo.HasFlag(AttackType)) return 0;
        else if (enemy.WeakTo.HasFlag(AttackType)) multipler = 2;
        return EffectivePower * multipler;
    }
}

public enum Team
{
    Immune,
    Infection
}

[Flags]
public enum DamageType
{
    None = 0,
    Radiation = 1,
    Bludgeoning = 2,
    Fire = 4,
    Cold = 8,
    Slashing = 16
}