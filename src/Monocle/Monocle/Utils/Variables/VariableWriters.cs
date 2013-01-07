using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;

namespace Monocle.Utils.Variables
{
    [GenericTypeWriter]
    class StructVariableWriter<T> : TypeWriter<StructVariable<T>>
    {
        public override void WriteType(StructVariable<T> toWrite, IWriter writer)
        {
            writer.Write(toWrite.Value);
            writer.Write(toWrite.Name);
        }
    }

    [GenericTypeWriter]
    class ClonableVariableWriter<T> : TypeWriter<ClonableVariable<T>>
    {
        public override void WriteType(ClonableVariable<T> toWrite, IWriter writer)
        {
            writer.Write(toWrite.Value);
            writer.Write(toWrite.Name);
        }
    }
}
