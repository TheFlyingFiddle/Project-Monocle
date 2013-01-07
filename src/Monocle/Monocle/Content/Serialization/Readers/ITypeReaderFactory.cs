using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Monocle.Content.Serialization
{
    public interface ITypeReaderFactory
    {
        ITypeReader GetTypeReader<T>();
        ITypeReader GetTypeReader(Type type);

        void RegisterAssembly(Assembly assembly);
        void RegisterTypeReader(ITypeReader typeReader);
        void RegisterGenericTypeReader(Type genericTypeReaderType);
    }
}
