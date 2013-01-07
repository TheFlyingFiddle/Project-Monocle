using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;

namespace Monocle.Utils
{
    [TypeReader]
    class VariableCollectionReader : TypeReader<VariableCollection>
    {
        public override VariableCollection Read(IReader reader)
        {
            int count = reader.ReadInt32();
            var collection = new IVariable[count];
            for (int i = 0; i < count; i++)
            {
                collection[i] = reader.Read<IVariable>();
            }

            return new VariableCollection(collection);
        }
    }
}
