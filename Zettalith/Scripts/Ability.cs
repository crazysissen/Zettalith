using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    abstract class Ability
    {
        public abstract string Name { get; protected set; }
        public abstract Mana ManaCost { get; protected set; }
        public abstract void Cast(object target);
    }
}
