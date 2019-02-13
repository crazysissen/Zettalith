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

        static public float EaseOutElastic(float p) => (float)Math.Sin(-13 * HALFPI * (p + 1)) * (float)Math.Pow(2, -10 * p) + 1;

        public static float EaseInElastic(float p) => (float)Math.Sin(13 * HALFPI * p) * (float)Math.Pow(2, 10 * (p - 1));

        static public float EaseOutBack(float p) => 1 - EaseInBack(1 - p);

        public static float EaseInBack(float p) => p * p * p - p * (float)Math.Sin(p * PI);

        static public float EaseOutCubic(float p) => 1 + EaseInCubic(p - 1);

        static public float EaseInCubic(float p) => p * p * p;
    }
}
