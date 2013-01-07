using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public interface ITypeWriter
    {
        void WriteType(object toWrite, IWriter writer);
        Type GetWritableType();
    }

    public interface ITypeWriter<T> : ITypeWriter
    {
        void WriteType(T toWrite, IWriter writer);
    }
}
