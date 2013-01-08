using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    class EnumTypeWriter : ITypeWriter
    {
        private Type type;

        public EnumTypeWriter(Type type)
        {
            this.type = type;
        }

        public void WriteType(object toWrite, IWriter writer)
        {
            writer.Write((int)toWrite);
        }

        public Type GetInputType()
        {
            return type;
        }

        public Type GetOutputType()
        {
            return type;
        }
    }
}
