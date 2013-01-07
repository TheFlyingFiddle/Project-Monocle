using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    /// <summary>
    /// Writes an array.
    /// </summary>
    /// <remarks>Arrays are a very special type of object becouse of this we don't specify that the reader is a contentwriter.</remarks>
    /// <typeparam name="T">The type to write.</typeparam>
    public class ArrayWriter<T> : TypeWriter<T[]>
    {
        public override void WriteType(T[] toWrite, IWriter writer)
        {
            writer.Write(toWrite.Length);
            for (int i = 0; i < toWrite.Length; i++)
            {
                writer.Write(toWrite[i]);
            }
        }
    }
}
