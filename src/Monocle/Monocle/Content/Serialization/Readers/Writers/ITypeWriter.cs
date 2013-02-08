using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    public interface ITypeWriter
    {
        void WriteType(object toWrite, IWriter writer);
        Type GetInputType();
        Type GetOutputType();
    }

}
