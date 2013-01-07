using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    class EnumTypeReader : ITypeReader
    {
        private Type enumType;
        internal EnumTypeReader(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Must be enum type!");

            this.enumType = type;
        }

        public object ReadType(IReader reader)
        {
            var enumVal = reader.Read<int>();
            foreach (var e in Enum.GetValues(enumType))
            {
                if (enumVal == (int)e)
                    return e;
            }

            throw new ArgumentException("Enum value does not exist.");
        }

        public Type GetRedableType()
        {
            return enumType;
        }
    }
}
