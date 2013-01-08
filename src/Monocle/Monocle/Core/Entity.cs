using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Game;
using Monocle.Utils;

namespace Monocle.Core
{
    public class Entity : MonocleObject, IEntity
    {
        public event Action<MonocleObject> ObjectAdded;
        public event Action<MonocleObject> ObjectRemoved;

        private string name;
        private Entity parent;
        private List<Component> components;
        private List<Entity> children;
        private HashSet<string> tags;
        private IVariableCollection variables;

        private Entity(Entity other)
        {
            this.name = other.name;
            this.tags = new HashSet<string>(other.tags);
            this.components = other.components.Select((x) => x.Copy(this)).ToList();
            this.variables = (IVariableCollection)other.variables.Clone();
            this.children = CloneChildren(other);
        }

        internal Entity(IVariableCollection variables)
        {
            this.name = string.Empty;
            this.tags = new HashSet<string>();
            this.components = new List<Component>();
            this.variables = variables;
            this.children = new List<Entity>();
        }

        private List<Entity> CloneChildren(Entity other)
        {
            var clones = new List<Entity>(other.children.Count);
            foreach (var child in other.children)
            {
                var clone = (Entity)child.Clone();
                clone.parent = this;
                clones.Add(clone);
            }
            return clones;
        }
                
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public IEntity Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.Reparent((Entity)value);
            }
        }

        #region Parenting Helpers

        private void Reparent(Entity value)
        {
            if (value == null)
            {
                if (this.parent != null)
                    this.parent.children.Remove(this);

            }
            else
            {
                if(this.ChildrenContainsValue(value))
                    throw new ArgumentException(string.Format("Parenting {0} to {1} would create a cyclic graph this is not allowed", this, value));

                if (this.parent != null)
                    this.parent.children.Remove(this);

                this.parent = value;
                this.parent.children.Add(this);
            }
        }

        private bool ChildrenContainsValue(Entity value)
        {
            foreach (var child in this.children)
            {
                if (object.ReferenceEquals(child, value))
                    return true;

                if (child.ChildrenContainsValue(value))
                    return true;
            }

            return false;
        }

        #endregion

        public IEnumerable<IEntity> Children
        {
            get { return this.children; }
        }

        public bool HasTag(string tag)
        {
            return this.tags.Contains(tag);
        }

        public void AddTag(string tag)
        {
            this.tags.Add(tag);
        }

        public bool RemoveTag(string tag)
        {
            return this.tags.Remove(tag);
        }

        public T GetComponent<T>()
        {
            var result = components.Find((x) => x is T);
            if (result == null)
                throw new ComponentMissingException();

            return (T)(object)result;
        }

        public IEnumerable<T> GetComponents<T>()
        {
            return components.FindAll((x) => x is T).Select<Component, T>((x) => (T)(object)x);
        }

        public bool TryGetComponent<T>(out T component)
        {
            object obj = components.Find((x) => x is T);
            if (obj != null)
            {
                component = (T)obj;
                return true;
            }
            component = default(T);
            return false;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T comp = new T();
            comp.Owner = this;
            this.components.Add(comp);
            if (this.ObjectAdded != null)
                this.ObjectAdded(comp);

            return comp;
        }

        public void RemoveComponent(Component component)
        {
            var result = this.components.Remove(component);
            if (result && this.ObjectRemoved != null)
                this.ObjectRemoved(component);
        }

        public Variable<T> GetVar<T>(string name)
        {
            return this.variables.GetVariable<T>(name);
        }

        public bool TryGetVar<T>(string name, out Variable<T> variable)
        {
            return this.variables.TryGetVariable<T>(name, out variable);
        }

        public bool HasVar(string name)
        {
            return this.variables.HasVariable(name);
        }

        public Variable<T> AddVar<T>(string name, T value)
        {
            return this.variables.AddVariable(name, value);
        }

        public bool RemoveVar(string name)
        {
            return this.variables.RemoveVariable(name);
        }

        public void SendMessage(string messageName, object param = null, Utils.MessageOptions options = MessageOptions.DontRequireReceiver)
        {
            this.components.ForEach((x) => x.SendMessage(messageName, param, options));
        }

        public void SendMessageUpwards(string messageName, object param = null, Utils.MessageOptions options = MessageOptions.DontRequireReceiver)
        {
            this.SendMessage(messageName, param, options);
            if (this.parent != null)
                this.parent.SendMessageUpwards(messageName, param, options);
        }

        public void SendMessageDownwards(string messageName, object param = null, Utils.MessageOptions options = MessageOptions.DontRequireReceiver)
        {
            this.SendMessage(messageName, param, options);
            this.children.ForEach((x) => x.SendMessageDownwards(messageName, param, options));
        }

        public object Clone()
        {
            return new Entity(this);
        }

        internal protected override void DestroySelf()
        {
            base.DestroySelf();

            DestroyComponents();
            DestroyChildren();

            this.name = null;
            this.tags = null;
            this.Parent = null;
            this.variables = null;
        }

        private void DestroyComponents()
        {
            for (int i = this.components.Count - 1; i >= 0; i--)
            {
                components[i].DestroyImediate();
            }
            this.components = null;
        }

        private void DestroyChildren()
        {
            for (int i = this.children.Count - 1; i >= 0; i--)
            {
                children[i].DestroyImediate();
            }
            this.children = null;
        }
    }
}