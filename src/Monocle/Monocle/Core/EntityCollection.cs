using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Game;

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
      
        private void OnComponentAdded(MonocleObject component)
        {
            if (this.ObjectAdded != null)
                this.ObjectAdded(component);
        }

        private void OnComponentRemoved(MonocleObject component)
        {
            if (this.ObjectRemoved != null)
                this.ObjectRemoved(component);
        }

        public void Add(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.ObjectAdded += this.OnComponentAdded;
            entity.ObjectRemoved += this.OnComponentRemoved;
            entity.Destroyed += this.Remove;
            


            if (this.ObjectAdded != null)
                this.ObjectAdded(entity);

            this.entities.Add(entity);
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
                entity.ObjectAdded -= this.OnComponentAdded;
                entity.ObjectRemoved -= this.OnComponentRemoved;
                entity.Destroyed -= this.Remove;

                if (this.ObjectRemoved != null)
                    this.ObjectRemoved(entity);
            }

            return result;
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
    }
}
