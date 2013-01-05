using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EntityFramework
{
    public interface IEntitySystem
    {
    }

    public class EntitySystem : IEntitySystem
    {     
        protected virtual void EntityCreated(IEntity entity) { }
        protected virtual void EntityDestroyed(IEntity entity) { }
        protected virtual void ComponentAdded(Component component) {  }
        protected virtual void ComponentRemoved(Component component) { }
    }


    public class UpdateSystem : EntitySystem
    {
        private ScriptMethod update;

        public UpdateSystem() 
        {
            update = new ScriptMethod("update");
        }

        public void Update()
        {
            if (update.Method != null)
                update.Method.Invoke();

        }

        protected override void ComponentAdded(Component component)
        {
            update.TrackInstance(component);
        }

        protected override void ComponentRemoved(Component component)
        {
            update.UnTrackInstance(component);
        }
    }
}