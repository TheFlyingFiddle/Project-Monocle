using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public interface IMutableVariableCollection : IVariableCollection
    {
        void AddVariable(string id, object value);
        bool RemoveVariable(string id);
    }

    public sealed class MutableVariableCollection : IMutableVariableCollection
    {
        private readonly Dictionary<string, object> variables;

        public MutableVariableCollection()
        {
            this.variables = new Dictionary<string, object>();
        }

        public MutableVariableCollection(IVariableCollection variableCollection)
        {
            this.variables = new Dictionary<string, object>();
            foreach (var keyvalue in variableCollection)
                this.variables.Add(keyvalue.Key, keyvalue.Value);
        }

        public void AddVariable(string id, object value)
        {
            if (this.variables.ContainsKey(id))
                throw new ArgumentException(string.Format("There already exists a variable {0}.", id));
            
            this.variables.Add(id, value);
        }

        public bool RemoveVariable(string id)
        {
            return this.variables.Remove(id);
        }

        public T GetVariable<T>(string name)
        {
            object obj;
            if (this.variables.TryGetValue(name, out obj))
            {
                if (obj is T) return (T)obj;
                else throw new InvalidCastException(string.Format("Tried to get variable {0} as type {1} but {0} is of type {2}.", name, typeof(T), obj.GetType()));
            }

            throw new ArgumentException("A variable named " + name + " does not exist.");
        }

        public bool TryGetVariable<T>(string name, out T variable)
        {
            object obj;
            if (this.variables.TryGetValue(name, out obj))
            {
                if (obj is T)
                {
                    variable = (T)obj;
                    return true;
                }
            }

            variable = default(T);
            return false;
        }

        public void SetVariable<T>(string name, T value)
        {
            object obj;
            if (this.variables.TryGetValue(name, out obj))
            {
                if (obj.GetType() == typeof(T))
                {
                    this.variables[name] = value;
                    return;
                }
                else
                    throw new InvalidCastException(string.Format("Changing the type of variable {0} from {1} to {2} is not legal.", name, obj.GetType(), typeof(T)));
            }

            throw new ArgumentException("A variable named " + name + " does not exist.");
        }

        public int Count
        {
            get { return this.variables.Count; }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.variables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public object Clone()
        {
            return new MutableVariableCollection(this);
        }
    }
}
