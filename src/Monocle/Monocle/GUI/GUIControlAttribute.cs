using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    [AttributeUsage(AttributeTargets.Class)]
    class GUIControlAttribute : Attribute
    {
        public Type Renderer { get; private set; }

        public GUIControlAttribute(Type renderer)
        {
            this.Renderer = renderer;
        }
    }
}
