using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [Serializable]
    public class Mana
    {
        public int Red { get; private set; }
        public int Blue { get; private set; }
        public int Green { get; private set; }
        
        public Mana(int red, int blue, int green)
        {
            Red = red;
            Blue = blue;
            Green = green;
        }

        public static Mana operator +(Mana a, Mana b) => new Mana(a.Red + b.Red, a.Blue + b.Blue, a.Green + b.Green);

        public static Mana operator -(Mana a, Mana b) => new Mana(a.Red - b.Red, a.Blue - b.Blue, a.Green - b.Green);

        public static Mana operator *(Mana a, Mana b) => new Mana(a.Red * b.Red, a.Blue * b.Blue, a.Green * b.Green);

        public static Mana operator +(Mana a, int b) => new Mana(a.Red + b, a.Blue + b, a.Green + b);
    }
}