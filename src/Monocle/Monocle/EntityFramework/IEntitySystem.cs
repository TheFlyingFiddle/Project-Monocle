using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Monocle.EntityFramework
{
    public interface IEntitySystem 
    {
        void TrackEntityCollection(IEntityCollection collection);
        void UnTrackEntityCollection(IEntityCollection collection);
        void SetUp();
        void TearDown();
        
        void Exceute();

        int Order { get; }
    }
    
    public abstract class EntitySystem : IEntitySystem
    {
        public int Order
        {
            get;
            set;
        }

        public EntitySystem(int order) : base() 
        {
            this.Order = order; 
        }


        public void TrackEntityCollection(IEntityCollection toTrack)
        {
            toTrack.EntityCreated += EntityCreated;
            toTrack.EntityDestroyed += EntityCreated;
            toTrack.ComponentCreated += ComponentCreated;
            toTrack.ComponentDestroyed += ComponentDestroyed;
        }

        public void UnTrackEntityCollection(IEntityCollection toTrack)
        {
            toTrack.EntityCreated -= EntityCreated;
            toTrack.EntityDestroyed -= EntityCreated;
            toTrack.ComponentCreated -= ComponentCreated;
            toTrack.ComponentDestroyed -= ComponentDestroyed;
        }

        public abstract void SetUp();
        public abstract void TearDown();
        public abstract void Exceute();

        protected virtual void EntityCreated(IEntity entity) { }
        protected virtual void EntityDestroyed(IEntity entity) { }
        protected virtual void ComponentCreated(Component component) {  }
        protected virtual void ComponentDestroyed(Component component) { }
    }
}