using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public abstract class Variable<T> : ICloneable
    {
        public T Data;
        public readonly string Name;

        protected Variable(T data, string name)
        {
            this.Data = data;
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
    
    }

    class StructVariable<T> : Variable<T>
    {
        internal StructVariable(T data, string name)
            : base(data, name) { }

        public override object Clone()
        {
            return new StructVariable<T>(this.Data, this.Name);
        }
    }

    class ClonableVariable<T> : Variable<T>
    {
        internal ClonableVariable(T clonable, string name)
            : base(clonable, name) { }

        public override object Clone()
        {
            return new ClonableVariable<T>((T)((ICloneable)this.Data).Clone(), this.Name);
        }
    }
}