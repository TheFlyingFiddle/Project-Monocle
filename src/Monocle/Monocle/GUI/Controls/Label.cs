using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    public class Label : GUIControl
    {
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

        public Label()
            : base()
        {
            this.Text = string.Empty;
            this.Alignment = TextAlignment.Left;
        }

        public TextAlignment TextAlignment { get; set; }
    }
}
