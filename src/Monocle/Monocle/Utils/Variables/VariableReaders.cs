using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;

namespace Monocle.Utils.Variables
{
    [GenericTypeReader]
    class StructVariableReader<T> : TypeReader<StructVariable<T>>
    {
        public override StructVariable<T> Read(IReader reader)
        {
            return new StructVariable<T>(reader.Read<T>(), reader.ReadString());
        }
    }

    [GenericTypeReader]
    class ClonableVariableReader<T> : TypeReader<ClonableVariable<T>>
    {
        public override ClonableVariable<T> Read(IReader reader)
        {
            return new ClonableVariable<T>(reader.Read<T>(), reader.ReadString());
        }
    }
}