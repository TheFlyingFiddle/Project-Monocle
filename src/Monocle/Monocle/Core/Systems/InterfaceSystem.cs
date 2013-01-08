using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Game;
using Monocle.Utils;

namespace Monocle.Core.Systems
{
    public abstract class InterfaceSystem<T> : ISystem
    {
        protected readonly List<T> Tracking;

        public InterfaceSystem()
        {
            Tracking = new List<T>();
        }

        public bool Enabled
        {
            get;
            set;
        }

        public abstract void Exceute(Time time);


        public void OnAdded(MonocleObject obj)
        {
            if (obj is T)
            {
                Tracking.Add((T)(object)obj);
            }
        }

        public void OnRemoved(MonocleObject obj)
        {
            if (obj is T)
            {
                Tracking.Remove((T)(object)obj);
            }
        }

        public void TrackCollection(IMonocleCollection collection)
        {
            collection.ObjectAdded += OnAdded;
            collection.ObjectRemoved += OnRemoved;
        }
    }
}