using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public interface IVariableCollection : IEnumerable<KeyValuePair<string, object>>
    {
        T GetVariable<T>(string name);
        void SetVariable<T>(string name, T value);
    }

    /// <summary>
    /// A collection that stores variables.
    /// </summary>
    /// <remarks>A variable is a value that has an associated string.</remarks>
    public class VariableCollection : IVariableCollection
    {
        private readonly Dictionary<string, object> variables;

        public VariableCollection(Dictionary<string, object> variables)
        {
            this.variables = new Dictionary<string, object>(variables);
        }

        public VariableCollection(IVariableCollection collection)
        {
            this.variables = new Dictionary<string, object>();
            foreach (var keyvalue in collection)
            {
                this.variables.Add(keyvalue.Key, keyvalue.Value);
            }
        }

        /// <summary>
        /// Gets a variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <returns>A variable value.</returns>
        /// <exception cref="InvalidCastException">Thrown if the type T is not the correct type for this variable.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exist.</exception>
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
        
        /// <summary>
        /// Sets a variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The new value of the variable.</param>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="InvalidCastException">Thrown if the type of the value is incorrect.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exsist.</exception>
        public void SetVariable<T>(string name, T value)
        {
            object obj;
            if (this.variables.TryGetValue(name, out obj))
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (obj.GetType() == typeof(T))
                    this.variables[name] = value;
                else
                    throw new InvalidCastException(string.Format("Changing the type of variable {0} from {1} to {2} is not legal.", name, obj.GetType(), typeof(T)));
            }

            throw new ArgumentException("A variable named " + name + " does not exist.");
        }

        #region IEnumerator

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.variables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
