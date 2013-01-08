using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Game;
using Monocle.Content.Serialization;

namespace Monocle.Core
{
    public abstract class Component : MonocleObject
    {
        [IgnoreSerialize]
        private Entity owner;
        public Entity Owner
        {
            get { return this.owner; }
            internal set { this.owner = value; }
        }

        internal Component Copy(Entity owner)
        {
            var clone = this.Clone();
            clone.Owner = owner;
            
            return clone;
        }

        protected abstract Component Clone();

        public virtual void SendMessage(string messageName, object param, MessageOptions options)
        {
            MessageSender.SendMessage(this, messageName, param, options);
        }

        internal new void DestroyImediate()
        {
            base.DestroyImediate();
        }

        internal protected override void DestroySelf()
        {
            if (this.owner == null)
                return;

            this.owner.RemoveComponent(this);
            this.owner = null;
        }
    }
}
