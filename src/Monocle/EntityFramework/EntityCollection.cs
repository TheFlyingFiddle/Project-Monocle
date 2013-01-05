using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace EntityFramework
{
    interface IEntityCollection : IEnumerable<IEntity> 
    {
        IEntity AddEntity();
        IEntity Find(Predicate<IEntity> match);

        event Action<IEntity> EntityCreated;
        event Action<IEntity> EntityDestroyed;
        event Action<Component> ComponentCreated;
        event Action<Component> ComponentDestroyed;
    }


    class EntityCollection : IEntityCollection
    {
        private readonly List<IEntity> entities;

        public event Action<IEntity> EntityCreated;
        public event Action<IEntity> EntityDestroyed;
        public event Action<Component> ComponentCreated;
        public event Action<Component> ComponentDestroyed;

        public EntityCollection()
        {
            this.entities = new List<IEntity>();
        }
      
        internal void OnComponentAdded(Component component)
        {
            if (this.ComponentCreated != null)
                this.ComponentCreated(component);
        }

        internal void OnComponentRemoved(Component component)
        {
            if (this.ComponentDestroyed != null)
                this.ComponentDestroyed(component);
        }

        internal void Remove(Entity entity)
        {
            if (this.EntityDestroyed != null)
                this.EntityDestroyed(entity);

            this.entities.Remove(entity);
        }

        public IEntity AddEntity()
        {
            var entity = new Entity(this, new VariableCollection());
            if (this.EntityCreated != null)
                this.EntityCreated(entity);

            this.entities.Add(entity);
            return entity;
        }

        public IEntity Find(Predicate<IEntity> match)
        {
            return this.entities.Find(match);
        }


        public IEnumerator<IEntity> GetEnumerator()
        {
            return this.entities.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
