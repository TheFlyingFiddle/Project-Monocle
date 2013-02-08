using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Monocle.Content.Serialization
{
    public interface ITypeWriterFactory
    {
        ITypeWriter GetTypeWriter<T>();
        ITypeWriter GetTypeWriter(Type type);

        void RegisterAssembly(Assembly toRegister);
        void RegisterTypeWriter(ITypeWriter writer);
        void RegisterGenericTypeWriter(Type genericTypeWriterType);
    }
}
