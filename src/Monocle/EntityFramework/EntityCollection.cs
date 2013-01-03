using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFramework
{
    public interface IEntityCollection
    {
        IEntity CreateEntity(string name);
        IEntity CreateEntity(Prefab prefab, string name = "");
        IEntity CloneEntity(IEntity entity);
        void DestroyEntity(IEntity entity);
        void ReamovedDestroyedEntities();

        event Action<IEntity> EntityCreated;
        event Action<IEntity> EntityDestroyed;
        event Action<Component> ComponentAdded;
        event Action<Component> ComponentRemoved;
    }

    public interface InternalEntityCollection : IEntityCollection
    {
        void AddedComponent(Component component);
        void RemovedComponent(Component component);
    }

    class EntityCollection : InternalEntityCollection
    {
        public event Action<IEntity> EntityCreated;
        public event Action<IEntity> EntityDestroyed;
        public event Action<Component> ComponentAdded;
        public event Action<Component> ComponentRemoved;

        private readonly IEntityCreator creator;
        private readonly List<IEntity> entities;
        private readonly List<IEntity> toRemove;

        public EntityCollection(IEntityCreator creator)
        {
            this.creator = creator;
            this.entities = new List<IEntity>();
            this.toRemove = new List<IEntity>();
        }

        public IEntity CreateEntity(string name)
        {
            var entity = creator.CreateEntity(name, this);
            this.AddEntity(entity);
            return entity;
        }

        public IEntity CreateEntity(Prefab prefab, string name = "")
        {
            var entity = creator.CreateEntity(prefab, this, name);
            this.AddEntity(entity);
            return entity;
        }

        public IEntity CloneEntity(IEntity entity)
        {
            var clone = creator.CopyEntity(entity, this);
            this.AddEntity(clone);
            return clone;
        }

        public void DestroyEntity(IEntity entity)
        {
            this.toRemove.Add(entity);
        }

        private void AddEntity(IEntity entity)
        {
            this.entities.Add(entity);
            if (this.EntityCreated != null)
                this.EntityCreated(entity);
        }

        private void RemoveEntity(IEntity entity)
        {
            this.entities.Remove(entity);
            if (this.EntityDestroyed != null)
                this.EntityDestroyed(entity);
        }

        public void AddedComponent(Component component)
        {
            if (this.ComponentAdded != null)
                this.ComponentAdded(component);
        }

        public void RemovedComponent(Component component)
        {
            if (this.ComponentRemoved != null)
                this.ComponentRemoved(component);
        }

        public void ReamovedDestroyedEntities()
        {
            foreach (var entity in this.toRemove)
            {
                this.RemoveEntity(entity);
            }

            this.toRemove.Clear();
        }
    }
}