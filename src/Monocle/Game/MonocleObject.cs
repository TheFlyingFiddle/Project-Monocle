using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public abstract class MonocleObject
    {
        public static MonocleLifeTimeManager LifeTimeManager;

        static MonocleObject()
        {
            //Initialize with default lifeTimeManager.
            MonocleObject.LifeTimeManager = new DefaultLifeTimeManager();
        }

        private long instanceID;

        /// <summary>
        /// A identification that is guaranteed to be unique.
        /// </summary>
        public long InstanceID
        {
            get { return this.instanceID; }
            internal set { this.instanceID = value; }
        }

        public MonocleObject()
        {
            LifeTimeManager.InitializeObject(this);
        }

        public static bool operator ==(MonocleObject first, MonocleObject second) 
        {
            if (object.ReferenceEquals(first, second))
                return true;

            if ((object)second == null)
            {
                return first.InstanceID == -1;
            }
            else if ((object)first == null)
            {
                return second.InstanceID == -1;
            }

            return false;
        }

        public static bool operator !=(MonocleObject first, MonocleObject second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj is MonocleObject)
            {
                return ((MonocleObject)obj).instanceID == this.instanceID;
            }
            else if (obj == null)
                return this.instanceID == -1;
            return false;
        }

        public override int GetHashCode()
        {
            return ((int)this.instanceID ^ (int)(this.instanceID >> 32));
        }

        public void Destroy()
        {
            LifeTimeManager.FlagForDestruction(this);
        }

        internal protected void DestroyImediate()
        {
            LifeTimeManager.DestroyObject(this);
        }

        
        internal protected abstract void DestroySelf();
    }
}