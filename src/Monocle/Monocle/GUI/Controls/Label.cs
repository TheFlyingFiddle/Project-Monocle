using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    public class Label : GUIControl
    {
        private static readonly LabelRenderer Default_Renderer = new LabelRenderer();

        public string Text
        {
            get;
            set;
        }

        public TextAlignment Alignment
        {
            get;
            set;
        }

        public Label(MouseDevice device)
            : base(device)
        {
            this.Renderer = Default_Renderer;
            this.Text = string.Empty;
            this.Alignment = TextAlignment.Left;
        }
    }
}
