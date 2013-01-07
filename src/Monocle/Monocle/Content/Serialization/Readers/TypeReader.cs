using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    public abstract class TypeReader<T> : ITypeReader
    {
        public abstract T Read(IReader reader);

        public object ReadType(IReader reader)
        {
            return Read(reader);
        }

        public Type GetRedableType()
        {
            return typeof(T);
        }
    }
}
