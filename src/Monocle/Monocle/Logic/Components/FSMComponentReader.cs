using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;
using Monocle.Utils;

namespace Monocle.Logic.Components
{
    [TypeReader]
    class FSMComponentReader : TypeReader<FSMComponent>
    {
        public override FSMComponent Read(IReader reader)
        {
            int stateCount = reader.ReadInt32();
            var states = new Dictionary<string, FSMComponent.State>();
            for (int i = 0; i < stateCount; i++)
            {
                var state = reader.Read<FSMComponent.State>();
                states.Add(state.Name, state);
            }

            var variables = reader.Read<IVariableCollection>();
            var startState = reader.ReadString();

            return new FSMComponent(states, variables, startState);
        }
    }

    [TypeReader]
    class FSMComponentStateReader : TypeReader<FSMComponent.State>
    {
        public override FSMComponent.State Read(IReader reader)
        {
            var name = reader.ReadString();
            var actions = reader.Read<List<IStateAction>>();
            var transitions = reader.Read<Dictionary<string, string>>();

            return new FSMComponent.State(name, actions, transitions);
        }
    }

}
