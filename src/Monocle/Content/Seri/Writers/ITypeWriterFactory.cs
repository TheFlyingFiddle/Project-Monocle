using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public interface ITypeWriterFactory
    {
        ITypeWriter GetTypeWriter<T>();
        ITypeWriter GetTypeWriter(Type type);

        void RegisterTypeWriter(ITypeWriter writer);
        void RegisterGenericTypeWriter(Type genericTypeWriterType);
    }
}
