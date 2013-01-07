using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils
{
    /// <summary>
    /// Implementing this interface makes the type able to recive messages.
    /// </summary>
    public interface IReceiver
    {
        /// <summary>
        /// Send a message to the IReceiver.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="param">The parameter the message takes or null if the message lacks a parameter.</param>
        /// <param name="opt">See MessageOptions.</param>
        /// <exception cref="MessageException">Throw if something is wrong with the message and opt = MessageOptions.RequireReceiver</exception>
        void SendMessage(string messageName, object param, MessageOptions opt = MessageOptions.DontRequireReceiver);
    }
}
