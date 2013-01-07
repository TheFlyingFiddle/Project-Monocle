using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Game
{
    public abstract class MonocleLifeTimeManager 
    {
        internal abstract void InitializeObject(MonocleObject obj);
        internal abstract void DestroyObject(MonocleObject monocleObject);
        internal abstract void FlagForDestruction(MonocleObject obj);

        /// <summary>
        /// Destroys all MonocleObjects that have been flaged for deletion 
        /// using MonocleObject.Destroy();
        /// </summary>
        public abstract void DestroyObjectsFlaggedForDestruction();

    }
        
    public class DefaultLifeTimeManager : MonocleLifeTimeManager
    {
        //Have to lock everything in this class.
        private object _lock = new object();

        private long NextID;
        private readonly List<long> avalibleIDs;
        private readonly Queue<MonocleObject> flaggedForDestruction;
        
        public DefaultLifeTimeManager()
        {
            this.avalibleIDs = new List<long>();
            this.flaggedForDestruction = new Queue<MonocleObject>();
        }

        internal override void InitializeObject(MonocleObject toInitialize)
        {
            lock (_lock)
            {
                if (this.avalibleIDs.Count == 0)
                    toInitialize.InstanceID = NextID++;
                else
                {
                    toInitialize.InstanceID = avalibleIDs[avalibleIDs.Count - 1];
                    avalibleIDs.RemoveAt(avalibleIDs.Count - 1);
                }
            }
        }
        
        internal override void DestroyObject(MonocleObject toDestroy)
        {
            lock (_lock)
            {
                if (toDestroy.InstanceID == -1)
                    return;

                toDestroy.DestroySelf();
                long id = toDestroy.InstanceID;
                toDestroy.InstanceID = -1;

                if (id == NextID - 1)
                {
                    NextID = id;
                    return;
                }

                this.avalibleIDs.Add(id);
            }
        }

        internal override void FlagForDestruction(MonocleObject obj)
        {
            lock (_lock)
            {
                this.flaggedForDestruction.Enqueue(obj);
            }
        }
        
        public override void DestroyObjectsFlaggedForDestruction()
        {
            lock (_lock)
            {
                while (this.flaggedForDestruction.Count > 0)
                {
                    var obj = this.flaggedForDestruction.Dequeue();
                    this.DestroyObject(obj);
                }
            }
        } 
    }
}
