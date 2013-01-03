using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFramework
{
    interface IEntityCreator
    {
        IEntity CreateEntity(Prefab prefab, InternalEntityCollection entityCollection, string name);
        IEntity CreateEntity(string name, InternalEntityCollection entityCollection);
        IEntity CopyEntity(IEntity entity, InternalEntityCollection entityCollection);
    }

    class EntityCreator : IEntityCreator
    {
        public IEntity CreateEntity(Prefab prefab, InternalEntityCollection entityCollection, string name)
        {
            return new Entity(name, entityCollection, prefab);
        }

        public IEntity CreateEntity(string name, InternalEntityCollection entityCollection)
        {
            return new Entity(name, entityCollection);
        }

        public IEntity CopyEntity(IEntity entity, InternalEntityCollection entityCollection)
        {
            var copy = new Entity(entity.Name, entityCollection);
            foreach (var component in entity.Components)
            {
                copy.AddComponent(component.Copy(copy));
            }

            foreach (var tag in entity.Tags)
            {
                copy.AddTag(tag);
            }

            foreach (var variable in entity.Variables)
            {
                copy.AddVar(variable.Key, variable.Value);
            }

            return copy;
        }
    }

}
