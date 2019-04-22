﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    // For now, includes 1D simplex perlin noise
    [Serializable]
    class Noise
    {
        // Permutation table of length 512
        private readonly byte[] myPermutations;
        private int mySeed;

        public Noise() : this(new Random().Next())
        { }

        public Noise(int aSeed)
        {
            mySeed = aSeed;

            Random tempRandom = new Random(aSeed);
            myPermutations = new byte[0x200];
            tempRandom.NextBytes(myPermutations);
        }

        public float Generate(float x)
        {
            int tempIndex = x.Floor();
            float tempX = x - tempIndex;

            float
                tempFirstValue = (float)Math.Pow(1.0f - tempX * tempX, 4) * Gradient(myPermutations[tempIndex & 0xff], tempX),
                tempIntermittent = 1 - (tempX * tempX - 2 * tempX + 1),
                tempSecondValue = (float)Math.Pow(tempIntermittent, 4) * Gradient(myPermutations[(tempIndex + 1) & 0xff], tempX - 1);

            //Factor 0.395 scales the range to precisely - 1-> 1
            return 0.395f * (tempFirstValue + tempSecondValue);
        }

        public float Generate(float x, float y)
        {
            const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            float n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            var s = (x + y) * F2; // Hairy factor for 2D
            var xs = x + s;
            var ys = y + s;
            var i = xs.Floor();
            var j = ys.Floor();

            var t = (i + j) * G2;
            var X0 = i - t; // Unskew the cell origin back to (x,y) space
            var Y0 = j - t;
            var x0 = x - X0; // The x,y distances from the cell origin
            var y0 = y - Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            var x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            var y1 = y0 - j1 + G2;
            var x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            var y2 = y0 - 1.0f + 2.0f * G2;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            var ii = Mod(i, 256);
            var jj = Mod(j, 256);

            // Calculate the contribution from the three corners
            var t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0.0f) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * Gradient(myPermutations[ii + myPermutations[jj]], x0, y0);
            }

            var t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0.0f) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * Gradient(myPermutations[ii + i1 + myPermutations[jj + j1]], x1, y1);
            }

            var t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0.0f) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * Gradient(myPermutations[ii + 1 + myPermutations[jj + 1]], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0f * (n0 + n1 + n2);
        }

        private float Gradient(int aHash, float x)
        {
            int tempHash = aHash & 15;
            float tempGradient = 1.0f + (tempHash & 7);

            if ((tempHash & 8) != 0)
                return -tempGradient * x;

            return tempGradient * x;
        }

        private float Gradient(int hash, float x, float y)
        {
            var h = hash & 7;      // Convert low 3 bits of hash code
            var u = h < 4 ? x : y;  // into 8 simple gradient directions,
            var v = h < 4 ? y : x;  // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }


        private static int Mod(int x, int m)
        {
            var a = x % m;
            return a < 0 ? a + m : a;
        }
    }
}
