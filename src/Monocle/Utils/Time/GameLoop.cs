﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Utils
{
    public class GameLoop
    {
        public event Action<Time> Update;
        public event Action<Time> Render;

        private readonly Clock clock;
        private readonly long target;
        private readonly bool highResolutionWait;

        public GameLoop(int fps, bool highResolutionWait)
        {
            clock = new Clock();
            target = Clock.Frequency / fps;
            this.highResolutionWait = highResolutionWait;
        }


        public void StartLoop()
        {
            clock.Reset();
            long next_target = Clock.Now + target;
            while (true)
            {                
                if (Update != null)
                    Update(new Time(clock.TotalTime, clock.Elapsed));

                if (Render != null)
                    Render(new Time(clock.TotalTime, clock.Elapsed));

                if (next_target - Clock.Now > 0)
                    Sleeper.Sleep(Clock.TicksToNanoseconds((next_target - Clock.Now)), highResolutionWait);
                
                clock.Step();
                next_target = Clock.Now + target;
            }
        }


        private void UpdateReferenceTime()
        {
        }
    }
}
