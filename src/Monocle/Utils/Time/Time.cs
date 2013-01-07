using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public struct Time
    {
        public static Time Zero { get; set; }
            
        public readonly TimeSpan Total;
        public readonly TimeSpan Elapsed;

        public Time(long total_ticks, long elapsed_ticks)
        {
            Elapsed = TimeSpan.FromTicks(elapsed_ticks);
            Total = TimeSpan.FromTicks(total_ticks);
        }

        public Time(TimeSpan total, TimeSpan elapsed)
        {
            this.Total = total;
            this.Elapsed= elapsed;
        }

        public float ElapsedSeconds
        {
            get { return (float)this.Elapsed.TotalSeconds; }
        }
    }
}