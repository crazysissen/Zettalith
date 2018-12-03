using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    struct Stats
    {
        public int Health { get; set; }
        public Mana Mana { get; set; }
        public Mana AbilityCost { get; set; }
        public Mana MoveCost { get; set; }

        public Stats(int health)
        {
            Health = health;
            Mana = new Mana(0, 0, 0);
            AbilityCost = new Mana(0, 0, 0);
            MoveCost = new Mana(0, 0, 0);
        }

        public static Stats operator +(Stats a, Stats b) => new Stats
        {
            Health = a.Health + b.Health,
            Mana = a.Mana + b.Mana,
            AbilityCost = a.AbilityCost + b.AbilityCost,
            MoveCost = a.MoveCost + b.MoveCost
        };

        public static Stats operator *(Stats a, Stats b) => new Stats
        {
            Health = a.Health * b.Health,
            Mana = a.Mana * b.Mana,
            AbilityCost = a.AbilityCost * b.AbilityCost,
            MoveCost = a.MoveCost * b.MoveCost,
        };
    }
}
