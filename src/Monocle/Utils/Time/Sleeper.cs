using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Utils
{
    public class Sleeper
    {
        /// <summary>
        /// Sleep for ticks amount of time.
        /// </summary>
        /// <param name="ticks"></param>
        public static void Sleep(long nanoseconds, bool highRes)
        {
            if (nanoseconds < 0L)
                throw new ArgumentException("Time must be posetive or 0!");



            if (highRes)
            {
                long target = nanoseconds * Stopwatch.Frequency / 1000000000L + Stopwatch.GetTimestamp();
                while (Stopwatch.GetTimestamp() < target) { }
            }
            else
            {
                Thread.Sleep((int)(nanoseconds / 1000000));
            }
        }

    }
}
