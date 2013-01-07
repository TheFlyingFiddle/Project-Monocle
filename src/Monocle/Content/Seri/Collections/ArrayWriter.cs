using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
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
