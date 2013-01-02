using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    class ConsoleAction : StateAction
    {
        private readonly string command;
        private readonly string _event;

        public override void Enter()
        { }

        [Message]
        public void HandleCommand(string command)
        {
            if (this.command == command)
            {
                this.FSM.SendEvent(_event);
            }
        }

        public ConsoleAction(string command, string _event)
        {
            this.command = command;
            this._event = _event;
        }

        private ConsoleAction(ConsoleAction action)
        {
            this.command = action.command;
            this._event = action._event;
        }

        public override object Clone()
        {
            return new ConsoleAction(this);
        }
    }
}
