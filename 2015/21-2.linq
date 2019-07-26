<Query Kind="Program" />

void Main()
{
    List<Item> weapons = new List<UserQuery.Item>()
    {
        new Item { Cost = 8, Damage = 4 },
        new Item { Cost = 10, Damage = 5 },
        new Item { Cost = 25, Damage = 6 },
        new Item { Cost = 40, Damage = 7 },
        new Item { Cost = 74, Damage = 8 }
    };
    List<Item> armors = new List<UserQuery.Item>()
    {
        new Item { Cost = 0, Armor = 0 },
        new Item { Cost = 13, Armor = 1 },
        new Item { Cost = 31, Armor = 2 },
        new Item { Cost = 53, Armor = 3 },
        new Item { Cost = 75, Armor = 4 },
        new Item { Cost = 102, Armor = 5 }
    };
    List<Item> rings = new List<UserQuery.Item>()
    {
        new Item { Cost = 0 },
        new Item { Cost = 0 },
        new Item { Cost = 25, Damage = 1 },
        new Item { Cost = 50, Damage = 2 },
        new Item { Cost = 100, Damage = 3 },
        new Item { Cost = 20, Armor = 1 },
        new Item { Cost = 40, Armor = 2 },
        new Item { Cost = 80, Armor = 3 },
    };

    int playerHp = 100;
    int bossHp = 103;
    int bossDmg = 9;
    int bossArmor = 2;
    
    int highestCost = int.MinValue;

    foreach (var w in weapons)
    {
        foreach (var a in armors)
        {
            foreach (var r1 in rings)
            {
                foreach (var r2 in rings)
                {
                    if (r1 == r2) continue;
                    int cost = w.Cost + a.Cost + r1.Cost + r2.Cost;
                    int playerDmg = w.Damage + a.Damage + r1.Damage + r2.Damage;
                    int playerArmor = w.Armor + a.Armor + r1.Armor + r2.Armor;
                    
                    if (cost > highestCost && Sim(playerHp, playerDmg, playerArmor, bossHp, bossDmg, bossArmor) > 0)
                    {
                        highestCost = cost;
                    }
                }
            }
        }
    }
    
    highestCost.Dump();
}

private int Sim(int playerHp, int playerDmg, int playerArmor, int bossHp, int bossDmg, int bossArmor)
{
    while (playerHp > 0 && bossHp > 0)
    {
        bossHp -= Math.Max(1, playerDmg - bossArmor);
        if (bossHp <= 0) break;
        playerHp -= Math.Max(1, bossDmg - playerArmor);
    }
    return bossHp;
}

public class Item
{
    public int Cost { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
}