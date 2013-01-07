using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;

namespace Monocle.Logic
{
    /// <summary>
    /// An action that performs logic in the active state.
    /// </summary>
    public interface IStateAction : ICloneable, IReceiver
    {
        /// <summary>
        /// Gets or sets the statemachine owner of this action.
        /// </summary>
        IFSM FSM { get; set; }

        /// <summary>
        /// Called when the owner state enters.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called when the owner state exits.
        /// </summary>
        void Exit();
    }

    /// <summary>
    /// Basic implementation of IStateAction.
    /// </summary>
    public abstract class StateAction : IStateAction
    {
        private IFSM fsm;

        /// <summary>
        /// Gets or sets the statemachine owner of this action.
        /// </summary>
        public IFSM FSM
        {
            get { return fsm; }
            set
            {
                if (fsm != null)
                    throw new ArgumentException("Cannot set readonly value FSM");
                this.fsm = value;
            }
        }

        /// <summary>
        /// Sends a message using the MessageSender class.
        /// </summary>
        /// <param name="name">Message name.</param>
        /// <param name="parameter">Message data.</param>
        public virtual void SendMessage(string name, object parameter = null, MessageOptions options = MessageOptions.DontRequireReceiver)
        {
            MessageSender.SendMessage(this, name, parameter, options);
        }

        public abstract void Enter();
        public virtual void Exit() { }
        public abstract object Clone();
    }
}
