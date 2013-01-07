using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{

    [GenericContentWriter]
    public class ListWriter<T> : TypeWriter<List<T>>
    {
        public override void WriteType(List<T> toWrite, IWriter writer)
        {
            writer.Write(toWrite.Count);
            for (int i = 0; i < toWrite.Count; i++)
            {
                writer.Write(toWrite[i]);
            }
        }
    }
}
