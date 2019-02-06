using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    static class Easing
    {
        const float
            PI = (float)Math.PI,
            HALFPI = (float)Math.PI * 0.5f;

        static public float EaseOutElastic(float p)
            => (float)Math.Sin(-13 * HALFPI * (p + 1)) * (float)Math.Pow(2, -10 * p) + 1;

        static public float EaseOutBack(float p)
        {
            float f = 1 - p;
            return 1 - (f * f * f - f * (float)Math.Sin(f * PI));
        }

        static public float EaseOutQuartic(float p)
        {
            float f = p - 1;
            return f * f * f * (1 - p) + 1;
        }
    }
}
