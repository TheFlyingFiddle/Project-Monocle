using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public class ArrayReader<T> : TypeReader<T[]>
    {
        public override T[] Read(IReader reader)
        {
            int length = reader.ReadInt32();
            T[] array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = reader.Read<T>();
            }

            return array;
        }
    }
}
