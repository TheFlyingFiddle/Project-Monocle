using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    [GenericContentReader]
    public class SetReader<T> : TypeReader<HashSet<T>>
    {
        public override HashSet<T> Read(IReader reader)
        {
            var set = new HashSet<T>();
            int length = reader.ReadInt32();
            for (int i = 0; i < length; i++)
            {
                set.Add(reader.Read<T>());
            }
            return set;
        }
    }
}
