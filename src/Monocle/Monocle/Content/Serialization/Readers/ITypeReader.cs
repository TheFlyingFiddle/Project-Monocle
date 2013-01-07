using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    public interface ITypeReader
    {
        object ReadType(IReader reader);
        Type GetRedableType();
    }
}
