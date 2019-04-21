using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [Serializable]
    struct Stats
    {
        public int AttackDamage { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Armor { get; set; }
        public Mana Mana { get; set; }
        public Mana AbilityCost { get; set; }
        public Mana MoveCost { get; set; }

        public Stats(int attackDamage, int health, int armor, Mana mana, Mana abilityCost, Mana moveCost)
        {
            AttackDamage = attackDamage;
            MaxHealth = health;
            Armor = armor;
            Health = health;
            Mana = mana;
            AbilityCost = abilityCost;
            MoveCost = moveCost;
        }

        public Stats(int health)
        {
            AttackDamage = 0;
            MaxHealth = 0;
            Health = health;
            Armor = 0;
            Mana = new Mana();
            AbilityCost = new Mana();
            MoveCost = new Mana();
        }

        public Stats(int armor, bool isArmor)
        {
            AttackDamage = 0;
            MaxHealth = 0;
            Health = 0;
            Armor = armor;
            Mana = new Mana();
            AbilityCost = new Mana();
            MoveCost = new Mana();
        }

        public static Stats operator +(Stats a, Stats b) => new Stats
        {
            AttackDamage = a.AttackDamage + b.AttackDamage,
            MaxHealth = a.MaxHealth + b.MaxHealth,
            Health = a.Health + b.Health,
            Armor = a.Armor + b.Armor,
            Mana = a.Mana + b.Mana,
            AbilityCost = a.AbilityCost + b.AbilityCost,
            MoveCost = a.MoveCost + b.MoveCost
        };

        public static Stats operator *(Stats a, Stats b) => new Stats
        {
            AttackDamage = a.AttackDamage * b.AttackDamage,
            MaxHealth = a.MaxHealth + b.MaxHealth,
            Health = a.Health * b.Health,
            Armor = a.Armor * b.Armor,
            Mana = a.Mana * b.Mana,
            AbilityCost = a.AbilityCost * b.AbilityCost,
            MoveCost = a.MoveCost * b.MoveCost,
        };
    }
}
