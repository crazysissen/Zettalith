using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [Serializable]
    struct Mana
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public Mana(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static Mana operator +(Mana a, Mana b) => new Mana(a.Red + b.Red, a.Green + b.Green, a.Blue + b.Blue);

        public static Mana operator -(Mana a, Mana b) => new Mana(a.Red - b.Red, a.Green - b.Green, a.Blue - b.Blue);

        public static Mana operator *(Mana a, Mana b) => new Mana(a.Red * b.Red, a.Green * b.Green, a.Blue * b.Blue);

        public static Mana operator +(Mana a, int b) => new Mana(a.Red + b, a.Green + b, a.Blue + b);

        public static bool operator >(Mana a, Mana b) => a.Red > b.Red && a.Blue > b.Blue && a.Green > b.Green;

        public static bool operator <(Mana a, Mana b) => a.Red < b.Red || a.Blue < b.Blue || a.Green < b.Green;

        public static bool operator >=(Mana a, Mana b) => a.Red >= b.Red && a.Blue >= b.Blue && a.Green >= b.Green;

        public static bool operator <=(Mana a, Mana b) => a.Red <= b.Red || a.Blue <= b.Blue || a.Green <= b.Green;

        public static bool operator ==(Mana a, Mana b) => a.Red == b.Red && a.Blue == b.Blue && a.Green == b.Green;

        public static bool operator !=(Mana a, Mana b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj is Mana mana)
            {
                return this == mana;
            }

            return false;
        }

        public override string ToString() => "R: " + Red + ", G: " + Green + ", B: " + Blue;

        public override int GetHashCode() => base.GetHashCode();
    }
}