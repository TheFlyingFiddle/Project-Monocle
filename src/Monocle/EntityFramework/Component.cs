using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFramework
{
    
    public abstract class Component 
    {
        public IEntity Owner
        {
            get;
            internal set;
        }

        public Component Copy(IEntity owner)
        {
            var clone = this.Clone();
            clone.Owner = owner;
            return clone;
        }

        protected abstract Component Clone();
    }

    public class UpdatableComponent : Component
    {
     
        public void Update()
        {
            Console.WriteLine("Updating...");
        }

        protected override Component Clone()
        {
            return new UpdatableComponent();
        }
    }

}
