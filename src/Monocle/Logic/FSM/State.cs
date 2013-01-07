using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Logic
{
    /// <summary>
    /// A state used by an IFSM.
    /// </summary>
    public interface IState
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
}
