using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    public class Clock
    {
        private long startTime;
        private long lastTime;

        private long timeSuspended;
        private bool suspended;

        /// <summary>
        /// The elapsed time since the last step.
        /// </summary>
        public TimeSpan Elapsed;
        internal TimeSpan TotalTime;


        public Clock() 
        {
            this.Reset();
        }
                
        public void Reset()
        {
            this.startTime = Stopwatch.GetTimestamp();
            this.lastTime = startTime;
            this.timeSuspended = 0;
            this.suspended = false;
        }

        public void Step()
        {
            long count = Stopwatch.GetTimestamp();

            if (suspended)
            {
                timeSuspended += (count - timeSuspended);
                return;
            }

            Elapsed = Clock.CounterToTimeSpan(count - lastTime);
            TotalTime += Clock.CounterToTimeSpan(count - startTime - timeSuspended);
            lastTime = count;
        }

        public static long Now
        {
            get { return Stopwatch.GetTimestamp(); }
        }

        public static long Frequency
        {
            get { return Stopwatch.Frequency; }
        }

        public static long TicksToNanoseconds(long ticks)
        {
            return ticks * 1000000000L / Stopwatch.Frequency;
        }

        private static TimeSpan CounterToTimeSpan(long delta)
        {
            long value = checked(delta * 10000000L) / Stopwatch.Frequency;
            return TimeSpan.FromTicks(value);
        }
    }
}
