using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    static class MathZ
    {
        public const float
           DEGTORAD = (2 * (float)Math.PI) / 360,
           RADTODEG = 360 / (2 * (float)Math.PI);

        /// <summary>Accelerating sine. Equation that from 0-1 accelerates according to a sine wave</summary>
        public static float SineA(float value)
            => (float)Math.Sin(value * Math.PI * 0.5);

        /// <summary>Decellerating sine. Equation that from 0-1 decellerates according to a sine wave</summary>
        public static float SineD(float value)
            => (float)Math.Sin((value - 1) * Math.PI * 0.5) + 1;

        public static float Lerp(this float value, float min, float max)
            => min + (max - min) * value;

        public static Vector2 Rotate(this Vector2 vector, float radian)
        {
            float sin = (float)Math.Sin(radian);
            float cos = (float)Math.Cos(radian);

            vector = new Vector2()
            {
                X = (cos * vector.X) - (sin * vector.Y),
                Y = (sin * vector.X) + (cos * vector.Y)
            };

            return vector;
        }

        public static Point RoundToPoint(this Vector2 vector)
            => new Point((int)Math.Round(vector.X), (int)Math.Round(vector.Y));

        public static int HighestPowerLessThanOrEqual(this int number, out int power)
        {
            if (number < 2)
            {
                power = 0;

                return 0;
            }

            power = 1;

            int current = 2;

            while (current * 2 <= number)
            {
                ++power;

                current *= 2;
            }

            return current;
        }

        public static int HighestPowerLessThanOrEqual(this int number)
            => number.HighestPowerLessThanOrEqual(out int voidInt);

        public static int LowestPowerMoreThanOrEqual(this int number, out int power)
        {
            if (number <= 0)
            {
                power = 0;

                return 0;
            }

            power = 1;

            int current = 2;

            while (current < number)
            {
                ++power;

                current *= 2;
            }

            return current;
        }

        public static int LowestPowerMoreThanOrEqual(this int number)
            => number.LowestPowerMoreThanOrEqual(out int voidInt);

        public static float RotationTowards(this Vector2 origin, Vector2 target)
            => (float)Math.Atan2(target.Y - origin.Y, target.X - origin.X);

        public static float Min(this float value, float minimum) => value < minimum ? minimum : value;

        public static float Max(this float value, float maximum) => value > maximum ? maximum : value;

        public static float Clamp(this float value, float min, float max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        public static Vector2 Normalized(this Vector2 vector)
        {
            Vector2 returnVector = vector;
            returnVector.Normalize();
            return returnVector;
        }

        public static Vector2 Lerp(this Vector2 vector, Vector2 target, float value) => vector + (target - vector) * value;

        public static SPoint[] ToSPointArray(this Point[] array)
        {
            SPoint[] newArray = new SPoint[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    newArray[i] = array[i];
                }
                catch
                {
                    newArray[i] = new SPoint();
                }
            }

            return newArray;
        }

        public static Point[] ToPointArray(this SPoint[] array)
        {
            Point[] newArray = new Point[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    newArray[i] = array[i];
                }
                catch
                {
                    newArray[i] = new Point();
                }
            }

            return newArray;
        }

        public static Point ToRender(this Point point)
        {
            int multiplier = InGameController.IsHost ? 1 : -1;
            return new Point(point.X * multiplier, point.Y * multiplier);
        }

        public static Vector2 ToRender(this Vector2 vector)
        {
            int multiplier = InGameController.IsHost ? 1 : -1;
            return new Vector2(vector.X * multiplier, vector.Y * multiplier);
        }
    }
}
