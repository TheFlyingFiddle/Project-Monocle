using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle
{
    class MathHelper
    {
        public static float Clamp(float min, float max, float value)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else
                return value;
        }

        public static int Clamp(int min, int max, int value)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else
                return value;
        }

        #region RANDOM -- Dunno if i should be doing this.

        private static Random Random = new Random();

        public static void SeedRandom(int seed)
        {
            Random = new Random(seed);
        }

        public static int RandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static float RandomFloat(float min, float max)
        {
            return (float)(min + Random.NextDouble() * (max - min));
        }

        public static Color RandomColor()
        {
            return Random.NextColor();
        }

        #endregion
    }
}
