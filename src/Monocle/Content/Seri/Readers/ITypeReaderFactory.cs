using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public interface ITypeReaderFactory
    {
        ITypeReader GetTypeReader<T>();
        ITypeReader GetTypeReader(Type type);
        void RegisterTypeReader(ITypeReader typeReader);
        void RegisterGenericTypeReader(Type genericTypeReaderType);
    }
}
