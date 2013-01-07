using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public abstract class TypeWriter<T> : ITypeWriter<T>
    {
        public abstract void WriteType(T toWrite, IWriter writer);

        public void WriteType(object toWrite, IWriter writer)
        {
            this.WriteType((T)toWrite, writer);
        }

        public Type GetWritableType()
        {
            return typeof(T);
        }
    }
}
