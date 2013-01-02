using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    /// <summary>
    /// A state used by an IFSM.
    /// </summary>
    interface IState : ICloneable
    {
        /// <summary>
        /// Called when we enter the state.
        /// </summary>
        /// <remarks>Initialiation logic should go here.</remarks>
        void Enter();

        /// <summary>
        /// Called when we exit the state.
        /// </summary>
        /// <remarks>Cleanup logic should go here.</remarks>
        void Exit();

        /// <summary>
        /// Gets all the actions present in the state.
        /// </summary>
        IEnumerable<IStateAction> Actions { get; }


        /// <summary>
        /// Send a message to the IState.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="param">The parameter the message takes or null if the message lacks a parameter.</param>
        /// <param name="opt">See MessageOptions.</param>
        /// <exception cref="MessageException">Throw if something is wrong with the message and opt = MessageOptions.RequireReceiver</exception>
        void SendMessage(string messageID,
                           object param = null,
                           MessageOptions opt = MessageOptions.DontRequireReceiver);
    }


    /// <summary>
    /// Gets a transition for the event or -1 if no transition is avalible.
    /// </summary>
    interface IndexedState : IState
    {
        int GetTransition(string _event);
    }

    /// <summary>
    /// Basic implementation of IState. This class is used by FSM.
    /// </summary>
    class State : IndexedState
    {
        private readonly IStateAction[] actions;
        private readonly IDictionary<string, int> transitions;
     
        /// <summary>
        /// Name of the state.
        /// </summary>
        public readonly string Name;

        public IEnumerable<IStateAction> Actions
        {
            get { return this.actions.AsEnumerable<IStateAction>(); }
        }

        /// <summary>
        /// Creates a state.
        /// </summary>
        /// <remarks>The constructor does copy the parameters.</remarks>
        internal State(string name,
                      IEnumerable<IStateAction> actions,
                      IDictionary<string, int> transitions)
        {
            this.Name = name;
            this.actions = actions.Select((sa) => (IStateAction)sa.Clone()).ToArray();
            this.transitions = new Dictionary<string, int>(transitions);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        private State(State state)
            : this(state.Name, state.actions, state.transitions)
        { }

        /// <summary>
        /// Called when the fsm transitions to this state.
        /// </summary>
        public void Enter()
        {
            foreach (var action in this.actions)
            {
                action.Enter();
            }
        }

        /// <summary>
        /// Called when the fsm transitions from this state.
        /// </summary>
        public void Exit()
        {
            foreach (var action in this.actions)
            {
                action.Exit();
            }
        }


        /// <summary>
        /// Gets a transition for the event or -1 if no transition is avalible.
        /// </summary>
        public int GetTransition(string _event)
        {
            int result;
            if (this.transitions.TryGetValue(_event, out result))
                return result;
            return -1;
        }

        /// <summary>
        /// Send a message to all the actions present in the state.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="param">The parameter the message takes or null if the message lacks a parameter.</param>
        /// <param name="opt">See MessageOptions.</param>
        /// <exception cref="MessageException">Throw if something is wrong with the message and opt = MessageOptions.RequireReceiver</exception>
        public void SendMessage(string messageID,
                           object param = null,
                           MessageOptions opt = MessageOptions.DontRequireReceiver)
        {
            foreach (var action in this.actions)
            {
                action.SendMessage(messageID, param, opt);
            }
        }

        /// <summary>
        /// Clones the object. (deep copy)
        /// </summary>
        /// <returns>A State clone.</returns>
        public object Clone()
        {
            return new State(this);
        }
    }
}
