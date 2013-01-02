using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    /// <summary>
    /// Transforms a method into a method able to recive messages.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MessageAttribute : Attribute
    { }
}
