﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Utils;

namespace EntityFramework
{
    public abstract class Component : Game.MonocleObject
    {
        private IEntity owner;
        public IEntity Owner
        {
            get { return this.owner; }
            internal set { this.owner = value; }
        }

        internal Component Copy(IEntity owner)
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

        protected override void DestroySelf()
        {
            if (this.owner == null)
                return;

            this.owner.RemoveComponent(this);
            this.owner = null;
        }
    }
}