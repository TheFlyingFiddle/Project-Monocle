using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
