using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace EntityFramework
{
    public interface IEntity 
    {
        string Name { get; }
        bool HasTag(string tag);

        T GetComponent<T>();
        IEnumerable<T> GetComponents<T>();
        bool TryGetComponent<T>(out T component) where T :  Component;

        T AddComponent<T>() where T : Component, new();
        void DestroyComponent(Component component);


        T GetVar<T>(string name);

        bool TryGetVar<T>(string name, out T variable);
        void SetVar<T>(string name, T value);

        void AddVar(string name, object value);
        bool RemoveVar(string name);

        IEnumerable<Component> Components { get; }
        IEnumerable<string> Tags { get; }
        IVariableCollection Variables { get; }

        void AddTag(string tag);
        bool RemoveTag(string tag);
    }

    public sealed class Entity : IEntity
    {
        private readonly InternalEntityCollection owner;
        private readonly List<Component> components;
        private readonly IMutableVariableCollection sharedVariables;
        private readonly HashSet<string> tags;

        public string Name { get; set; }

        public IEnumerable<Component> Components { get { return components; } }
        public IEnumerable<string> Tags { get { return tags; } }
        public IVariableCollection Variables { get { return sharedVariables; } }


        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }

        internal Entity(string name, InternalEntityCollection owner, Prefab prefab)
        {
            this.Name = name;
            this.owner = owner;
            this.components = prefab.CloneComponents().ToList();
            this.tags = new HashSet<string>(prefab.CloneTags());
            this.sharedVariables = prefab.CloneVariables();
        }


        internal Entity(string name, InternalEntityCollection owner)
        {
            this.Name = name;
            this.owner = owner;
            this.tags = new HashSet<string>();
            this.components = new List<Component>();
            this.sharedVariables = new MutableVariableCollection();
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

            //Wierd.
            return (T)(object)result;
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = components.Find((x) => x is T) as T;
            return component != null;
        }

        public IEnumerable<T> GetComponents<T>()
        {
            return components.FindAll((x) => x is T).Select<Component, T>((x) => (T)(object)x);
        }

        public T GetVar<T>(string name)
        {
            return this.sharedVariables.GetVariable<T>(name);
        }

        public bool TryGetVar<T>(string name, out T variable)
        {
            return this.sharedVariables.TryGetVariable<T>(name, out variable);
        }

        public void SetVar<T>(string name, T value)
        {
            this.sharedVariables.SetVariable<T>(name, value);
        }

        public void AddVar(string name, object value)
        {
            this.sharedVariables.AddVariable(name, value);
        }

        public bool RemoveVar(string name)
        {
            return this.sharedVariables.RemoveVariable(name);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            component.Owner = this;
            this.components.Add(component);
            this.owner.AddedComponent(component);
            return component;
        }

        internal void AddComponent(Component component)
        {
            component.Owner = this;
            this.components.Add(component);
            this.owner.AddedComponent(component);
        }


        public void Destroy()
        {
            owner.DestroyEntity(this);
        }


        public void DestroyComponent(Component component)
        {
            var result = this.components.Remove(component);
            if (result) this.owner.RemovedComponent(component);
        }
    }
}
