using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    [GenericContentReader]
    public class ListReader<T> : TypeReader<List<T>>
    {
        public override List<T> Read(IReader reader)
        {
            int length = reader.ReadInt32();
            List<T> list = new List<T>(length);
            for (int i = 0; i < length; i++)
            {
                list.Add(reader.Read<T>());
            }

            return list;
        }
    }
}
