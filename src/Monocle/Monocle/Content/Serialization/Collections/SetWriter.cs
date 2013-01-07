using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{

    [GenericContentWriter]
    public class SetWriter<T> : TypeWriter<HashSet<T>>
    {
        public override void WriteType(HashSet<T> toWrite, IWriter writer)
        {
            writer.Write(toWrite.Count);
            foreach (var item in toWrite)
            {
                writer.Write(item);
            }
        }
    }
}
