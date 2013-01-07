using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils
{
    public interface IVariable : ICloneable
    {
        object Value { get; set; }
        string Name { get; }
    }

    public abstract class Variable<T> : IVariable
    {
        public T Value;
        public string Name { get; private set; }


        protected Variable(T data, string name)
        {
            this.Value = data;
            this.Name = name;
        }

        public abstract object Clone();
        
        internal static Variable<T> CreateVariable(T data, string name)
        {
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
                return new StructVariable<T>(data, name);
            else if (data is ICloneable)
                return new ClonableVariable<T>(data, name);
            else
                throw new InvalidTypeException(string.Format("The type {0} cannot be used as a variable!", typeof(T)));
        }

        object IVariable.Value
        {
            get { return Value; }
            set { this.Value = (T)value; }
        }
    }

    class StructVariable<T> : Variable<T>
    {
        internal StructVariable(T data, string name)
            : base(data, name) { }

        public override object Clone()
        {
            return new StructVariable<T>(this.Value, this.Name);
        }
    }

    class ClonableVariable<T> : Variable<T>
    {
        internal ClonableVariable(T clonable, string name)
            : base(clonable, name) { }

        public override object Clone()
        {
            return new ClonableVariable<T>((T)((ICloneable)this.Value).Clone(), this.Name);
        }
    }
}