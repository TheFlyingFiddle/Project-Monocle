using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Utils.Logging;

namespace Monocle.Core.Systems
{
    public class UpdateSystem : InterfaceSystem<IUpdatable>
    {
        public override void Exceute(Time time)
        {
            foreach (var item in this.Tracking)
            {
                try
                {
                    item.Update(time);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

    public class FixUpdateSystem : InterfaceSystem<IFixUpdatable>
    {
        private readonly TimeSpan updateInterval;
        private TimeSpan elapsed;

        public FixUpdateSystem(TimeSpan updateInterval)
        {
            this.updateInterval = updateInterval;
            this.elapsed = TimeSpan.Zero;
        }

        public override void Exceute(Time time)
        {
            elapsed += time.Elapsed;
            if (elapsed >= updateInterval)
            {
                elapsed -= updateInterval;
                foreach (var item in this.Tracking)
                {
                    try
                    {
                        item.FixedUpdate(time);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class CoroutineSystem : InterfaceSystem<ICoroutine>
    {
        public override void Exceute(Time time)
        {
            foreach (var coroutine in this.Tracking)
            {
                try
                {
                    coroutine.StepCoroutines();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
             }
        }
    }
}