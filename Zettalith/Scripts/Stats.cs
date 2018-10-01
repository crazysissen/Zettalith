using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    struct Stats
    {
        int Health { get; set; }
        Mana Mana { get; set; }

        public Stats(int health, Mana mana)
        {
            Health = health;
            Mana = mana;
        }
    }
}
