using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;

namespace Monocle.Utils.Variables
{
    [ContentWriter]
    class VariableCollectionWriter : TypeWriter<VariableCollection>
    {
        public override void WriteType(VariableCollection toWrite, IWriter writer)
        {
            writer.Write(toWrite.Count);
            foreach (var variable in toWrite)
            {
                writer.Write(variable);
            }
        }
    }
}
