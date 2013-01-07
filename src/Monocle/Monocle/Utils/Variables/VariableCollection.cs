using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils
{
    public interface IVariableCollection : IEnumerable<KeyValuePair<string, ICloneable>>, ICloneable
    {
        Variable<T> GetVariable<T>(string name);
        Variable<T> AddVariable<T>(string name, T value);
        bool TryGetVariable<T>(string name, out Variable<T> variable);
        int Count { get; }

        bool HasVariable(string name);
        bool RemoveVariable(string name);
    }

    /// <summary>
    /// A collection that stores variables.
    /// </summary>
    /// <remarks>A variable is a value that has an associated string.</remarks>
    public class VariableCollection : IVariableCollection
    {
        private readonly Dictionary<string, ICloneable> variables;

        public VariableCollection()
        {
            this.variables = new Dictionary<string, ICloneable>();
        }

        private VariableCollection(VariableCollection other)
        {
            variables = new Dictionary<string, ICloneable>();
            foreach (var variable in other.variables)
            {
                variables.Add(variable.Key, (ICloneable)variable.Value.Clone());
            }
        }


        public Variable<T> GetVariable<T>(string name)
        {
            ICloneable obj;
            if (this.variables.TryGetValue(name, out obj))
            {
                Variable<T> var = obj as Variable<T>;
                if (var == null)
                    throw CreateInalidTypeException(name, typeof(T), obj);

                return var;
            }

            throw new ArgumentException(string.Format("A variable named {0} does not exists.", name));
        }

        private InvalidCastException CreateInalidTypeException(string name, Type type, object variable)
        {
            Type variableType = variable.GetType();
            Type argumentType = variableType.GetGenericArguments()[0];

            return new InvalidCastException(string.Format("The variable {0} was is of type {1}, but was attempted to be retreived as {2}", name, type, argumentType));
        }

        public bool TryGetVariable<T>(string name, out Variable<T> variable)
        {
            ICloneable obj;
            if (this.variables.TryGetValue(name, out obj))
            {
                variable = obj as Variable<T>;
                return variable != null;
            }

            variable = null;
            return false;
        }

        public Variable<T> AddVariable<T>(string name, T value)
        {
            if (this.variables.ContainsKey(name))
                throw new ArgumentException(string.Format("A variable named {0} already exists.", name));
                  
            Variable<T> variable = Variable<T>.CreateVariable(value, name);
            this.variables.Add(name, variable);

            return variable;
        }

        public int Count
        {
            get { return this.variables.Count;  }
        }

        public bool HasVariable(string name)
        {
            return this.HasVariable(name);
        }

        public bool RemoveVariable(string name)
        {
            return this.variables.Remove(name);
        }

        public IEnumerator<KeyValuePair<string, ICloneable>> GetEnumerator()
        {
            return this.variables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.variables.GetEnumerator();
        }

        public object Clone()
        {
            return new VariableCollection(this);
        }
    }
}
