using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    [GenericTypeReader]
    public class DictionaryReader<K,V> : TypeReader<Dictionary<K,V>>
    {
        public override Dictionary<K, V> Read(IReader reader)
        {
            Dictionary<K, V> dict = new Dictionary<K, V>();
            int length = reader.ReadInt32();
            for (int i = 0; i < length; i++)
            {
                K key = reader.Read<K>();
                V value = reader.Read<V>();
                dict.Add(key, value);
            }

            return dict;
        }
    }
}
