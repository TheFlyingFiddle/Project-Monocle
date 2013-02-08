using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Graphics
{
    class Animation : ICloneable
    {
        private Frame[] frames;
        private int activeIndex;
        private TimeSpan interval;
        private TimeSpan elapsed;
        private bool running = false;

        public Frame CurrentFrame
        {
            get { return this.frames[activeIndex]; }
        }

        public Animation(Frame[] frames, int fps)
        {
            this.frames = frames;
            this.activeIndex = 0;

            this.interval = TimeSpan.FromSeconds(1.0d / fps);
            this.elapsed = TimeSpan.Zero;
        }

        public void Start()
        {
            running = true;
        }

        public void Stop()
        {
            running = false;
        }

        public void Reset()
        {
            this.activeIndex = 0;
            this.elapsed = TimeSpan.Zero;
        }

        public void Update(Utils.Time time)
        {
            if (running)
            {
                this.elapsed += time.Elapsed;
                if (this.interval <= this.elapsed)
                {
                    this.elapsed -= this.interval;
                    this.activeIndex = (this.activeIndex + 1) % this.frames.Length;
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
