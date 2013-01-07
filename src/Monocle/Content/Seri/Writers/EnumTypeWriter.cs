using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
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

        public Type GetWritableType()
        {
            return type;
        }
    }
}
