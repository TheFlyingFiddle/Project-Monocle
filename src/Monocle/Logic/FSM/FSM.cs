using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Logic
{
    /// <summary>
    /// A Finite State Machine. This state machine 
    /// uses events to transition between states. It provides
    /// access to a local collection of variables.
    /// </summary>
    public interface IFSM : ICloneable
    {
        /// <summary>
        /// Starts the fsm. Entering the first state. 
        /// </summary>
        void Start();
        
        /// <summary>
        /// Send an event to the IFSM. If the event 
        /// is pressent in the current state the machine will
        /// transition to a new state.
        /// </summary>
        /// <param name="_event">The string event to send.</param>
        void SendEvent(string _event);


        /// <summary>
        /// Send a message to the IFSM.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="param">The parameter the message takes or null if the message lacks a parameter.</param>
        /// <param name="opt">See MessageOptions.</param>
        /// <exception cref="MessageException">Throw if something is wrong with the message and opt = MessageOptions.RequireReceiver</exception>
        void SendMessage(string messageID, 
                                object param = null, 
                                MessageOptions opt = MessageOptions.DontRequireReceiver);


        /// <summary>
        /// Gets a variable out of the FSM.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <returns>A variable value.</returns>
        /// <exception cref="InvalidCastException">Thrown if the type T is not the correct type for this variable.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exist.</exception>
        T GetVariable<T>(string variableID);



        /// <summary>
        /// Sets a variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The new value of the variable.</param>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="InvalidCastException">Thrown if the type of the value is incorrect.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exsist.</exception>
        void SetVariable<T>(string variableID, T value);
    }

    /// <summary>
    /// Integer ID implementation of IFSM.
    /// </summary>
    public class FSM : IFSM
    {
        private readonly VariableCollection variables;
        private readonly IndexedState[] states;
        private int activeIndex;
        private readonly int startIndex;

        /// <summary>
        /// Creates a FSM.
        /// </summary>
        /// <remarks>The constructor does not clone states.</remarks>
        /// <param name="variables">The variables of the fsm.</param>
        /// <param name="states">The states in the fsm.</param>
        /// <param name="activeIndex">The startingID of the fsm.</param>
        public FSM(IVariableCollection variables, IndexedState[] states, int activeIndex)
        {
            this.variables = new VariableCollection(variables);
            this.states = states;
            this.startIndex = activeIndex;
            this.activeIndex = activeIndex;

            this.SetupActions();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="fSM"></param>
        public FSM(FSM fSM)
            : this(fSM.variables, fSM.states.Select((s) => (IndexedState)s.Clone()).ToArray(), fSM.startIndex)
        {
        }

        /// <summary>
        /// Hook in the actions. 
        /// </summary>
        private void SetupActions()
        {
            foreach (var state in this.states)
            {
                foreach (var action in state.Actions)
                {
                    action.FSM = this;
                }
            }
        }

        /// <summary>
        /// Start the state machine. Enter the start state.
        /// </summary>
        public void Start()
        {
            states[activeIndex].Enter();
        }

        /// <summary>
        /// Send an event to this FSM.
        /// </summary>
        /// <param name="_event">The event to send.</param>
        public void SendEvent(string _event)
        {
            var id = states[activeIndex].GetTransition(_event);
            if (id == -1)
                return;

            Transition(id);
        }


        /// <summary>
        /// Send a message to the FSM. That is propagated to its states.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="param">The parameter the message takes or null if the message lacks a parameter.</param>
        /// <param name="opt">See MessageOptions.</param>
        /// <exception cref="MessageException">Throw if something is wrong with the message and opt = MessageOptions.RequireReceiver</exception>
        public void SendMessage(string messageID, 
                                object param = null, 
                                MessageOptions opt = MessageOptions.DontRequireReceiver)
        {
            states[activeIndex].SendMessage(messageID, param, opt);
        }

        private void Transition(int stateID)
        {
            states[activeIndex].Exit();
            this.activeIndex = stateID;
            states[stateID].Enter();
        }

        /// <summary>
        /// Gets a variable out of the FSM.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <returns>A variable value.</returns>
        /// <exception cref="InvalidCastException">Thrown if the type T is not the correct type for this variable.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exist.</exception>
        public T GetVariable<T>(string variableID)
        {
            return this.variables.GetVariable<T>(variableID);
        }

        /// <summary>
        /// Sets a variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The new value of the variable.</param>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="InvalidCastException">Thrown if the type of the value is incorrect.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exsist.</exception>
        public void SetVariable<T>(string variableID, T value)
        {
            this.variables.SetVariable<T>(variableID, value);
        }

        /// <summary>
        /// Clones the FSM (DEEP)
        /// </summary>
        /// <returns>A FSM clone.</returns>
        public object Clone()
        {
            return new FSM(this);
        }
    }
}