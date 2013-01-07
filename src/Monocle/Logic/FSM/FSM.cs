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
    public interface IFSM
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
        Variable<T> GetVariable<T>(string variableID);


        /// <summary>
        /// Adds a variable to the FSM.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="variableID">The id of the variable.</param>
        /// <param name="value">The value of the variable. (Cannot be null)</param>
        /// <returns>The newly created variable.</returns>
        Variable<T> AddVariable<T>(string variableID, T value);


        /// <summary>
        /// Removes a variable from the FSM.
        /// </summary>
        /// <param name="variableID">The variable to remove.</param>
        /// <returns>True if a variable was removed.</returns>
        bool RemoveVariable(string variableID);

        /// <summary>
        /// Checks if the FSM contains a specific variiable.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>True if the fsm contains the variable.</returns>
        bool HasVariable(string name);

    }
}