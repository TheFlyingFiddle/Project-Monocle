using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Game;
using Monocle.Content.Serialization;

namespace Monocle.Core
{
    public interface IEntityCollection : IMonocleCollection , IEnumerable<Entity> 
    {
        Entity Find(Predicate<Entity> match);

        void Add(Entity e);
        bool Remove(Entity e);
    }

    public class EntityCollection : IEntityCollection
    {
        private readonly List<Entity> entities;

        public event Action<MonocleObject> ObjectAdded;
        public event Action<MonocleObject> ObjectRemoved;

        public EntityCollection()
        {
            this.entities = new List<Entity>();
        }

        private EntityCollection(List<Entity> entities)
           : this()
        {
            foreach (var entity in entities)
            {
                this.Add(entity);
            }
        }
      
        private void OnObjectAdded(MonocleObject component)
        {
            if (this.ObjectAdded != null)
                this.ObjectAdded(component);
        }

        private void OnObjectRemoved(MonocleObject component)
        {
            if (this.ObjectRemoved != null)
                this.ObjectRemoved(component);
        }


        private void ChildAdded(Entity entity)
        {
            if (this.entities.Contains(entity))
                return;

            AddRecurse(entity);
        }

        public void Add(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
                        
            if (entity.Parent != null)
                entity.Parent = null;

            AddRecurse(entity);

        }

        private void AddRecurse(Entity entity)
        {
            if (this.entities.Contains(entity))
                return;

            entity.ComponentAdded += this.OnObjectAdded;
            entity.ComponentRemoved += this.OnObjectRemoved;
            entity.ChildAdded += this.ChildAdded;
            entity.Destroyed += this.Remove;
            

            if (this.ObjectAdded != null)
                this.ObjectAdded(entity);

            this.entities.Add(entity);

            foreach (var child in entity.Children)
            {
                AddRecurse(child);
            }
        }

        private void Remove(MonocleObject entity)
        {
            this.Remove((Entity)entity);
        }

        public bool Remove(Entity entity)
        {
            var result = this.entities.Remove(entity);
            if (result)
            {
                RemoveRecurse(entity);
            }

            return result;
        }

        private void RemoveRecurse(Entity entity)
        {

            entity.ComponentAdded -= this.OnObjectAdded;
            entity.ComponentRemoved -= this.OnObjectRemoved;
            entity.ChildAdded -= this.OnObjectAdded;
            entity.ChildRemoved -= this.OnObjectRemoved;
            entity.Destroyed -= this.Remove;

            if (this.ObjectRemoved != null)
                this.ObjectRemoved(entity);

            foreach (var component in entity.GetComponents<Component>())
            {
                this.OnObjectRemoved(component);
            }

            foreach (var child in entity.Children)
            {
                RemoveRecurse(child);
            }

            this.entities.Remove(entity);
        }

        public Entity Find(Predicate<Entity> match)
        {
            return this.entities.Find(match);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return this.entities.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public class EntityCollectionReader : TypeReader<EntityCollection>
        {
            public override EntityCollection Read(IReader reader)
            {
                int count = reader.ReadInt32();
                var list = new List<Entity>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(reader.Read<Entity>());
                }

                return new EntityCollection(list);
            }
        }

        public class EntityCollectionWriter : TypeWriter<EntityCollection>
        {
            public override void WriteType(EntityCollection toWrite, IWriter writer)
            {
                int count = toWrite.entities.Count((x) => x.Parent == null);
                writer.Write(count);
                foreach (var entity in toWrite.entities)
                {
                    if(entity.Parent == null)
                        writer.Write(entity);
                }
            }
        }
    }
}