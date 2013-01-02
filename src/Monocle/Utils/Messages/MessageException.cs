using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    [Serializable]
    public class MessageException : Exception
    {
        /// <summary>
        /// Creates a message exception.
        /// </summary>
        public MessageException(string p)
            : base (p)
        { }

        /// <summary>
        /// Creates a message exception.
        /// </summary>
        public MessageException(string formatedMessage, params object[] formatParams)
            : base(string.Format(formatedMessage, formatParams))
        {
        }
    }
}
