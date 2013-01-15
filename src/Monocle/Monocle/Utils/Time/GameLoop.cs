using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Monocle.Utils
{
    public class GameLoop
    {
        public event Action<Time> Update;
        public event Action<Time> Render;

        private readonly Clock clock;
        private readonly long target;
        private readonly bool highResolutionWait;
        private volatile bool isRunning;

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
            while (isRunning)
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

        public void Stop()
        {
            this.isRunning = false;
        }

        public void Pause()
        {
            clock.Pause();

        }

        public void UnPause()
        {
            clock.UnPause();
        }
    }
}