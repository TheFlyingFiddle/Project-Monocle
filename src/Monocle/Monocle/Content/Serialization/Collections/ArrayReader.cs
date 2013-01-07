using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    /// <summary>
    /// Reads an array.
    /// </summary>
    /// <remarks>Arrays are a vary special type of object becouse of this we don't specify that the reader is a contentreader.</remarks>
    /// <typeparam name="T">The type to read.</typeparam>
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
