using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public class DictionaryWriter<K, V> : TypeWriter<Dictionary<K, V>>
    {
        public override void WriteType(Dictionary<K, V> toWrite, IWriter writer)
        {
            writer.Write(toWrite.Count);
            foreach (var item in toWrite.Keys)
            {
                writer.Write(item);
                writer.Write(toWrite[item]);
            }
        }
    }
}
