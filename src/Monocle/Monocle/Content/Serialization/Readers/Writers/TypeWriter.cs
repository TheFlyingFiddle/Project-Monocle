using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    public abstract class TypeWriter<T> : ITypeWriter
    {
        public abstract void WriteType(T toWrite, IWriter writer);

        public void WriteType(object toWrite, IWriter writer)
        {
            this.WriteType((T)toWrite, writer);
        }

        public Type GetInputType()
        {
            return typeof(T);
        }

        public virtual Type GetOutputType()
        {
            return typeof(T);
        }
    }
}