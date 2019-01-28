using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    struct Stats
    {
        public int AttackDamage { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public Mana Mana { get; set; }
        public Mana AbilityCost { get; set; }
        public Mana MoveCost { get; set; }

        public static Stats operator +(Stats a, Stats b) => new Stats
        {
            MaxHealth = a.MaxHealth + b.MaxHealth,
            Health = a.Health + b.Health,
            Mana = a.Mana + b.Mana,
            AbilityCost = a.AbilityCost + b.AbilityCost,
            MoveCost = a.MoveCost + b.MoveCost
        };

        public static Stats operator *(Stats a, Stats b) => new Stats
        {
            MaxHealth = a.MaxHealth + b.MaxHealth,
            Health = a.Health * b.Health,
            Mana = a.Mana * b.Mana,
            AbilityCost = a.AbilityCost * b.AbilityCost,
            MoveCost = a.MoveCost * b.MoveCost,
        };
    }
}
