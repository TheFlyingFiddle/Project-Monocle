using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;

namespace Monocle.Logic.Components
{
    [TypeWriter]
    class FSMComponentWriter : TypeWriter<FSMComponent>
    {
        public override void WriteType(FSMComponent toWrite, IWriter writer)
        {
            writer.Write(toWrite.StateCount);
            foreach (var state in toWrite.States)
            {
                writer.Write(state);
            }

            writer.Write(toWrite.Variables);
            writer.Write(toWrite.StartState);
        }
    }

    [TypeWriter]
    class FSMComponentStateWriter : TypeWriter<FSMComponent.State>
    {
        public override void WriteType(FSMComponent.State toWrite, IWriter writer)
        {
            writer.Write(toWrite.Name);
            writer.Write(toWrite.Actions);
            writer.Write(toWrite.Transitions);
        }
    }

}
