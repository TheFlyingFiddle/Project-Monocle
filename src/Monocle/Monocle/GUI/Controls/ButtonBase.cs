using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using Monocle.Graphics;
using OpenTK.Graphics;

namespace Monocle.GUI
{
    public abstract class ButtonBase : GUIControl
    {
        public string Text
        {
            get;
            set;
        }

        public Color4 TextColor 
        {
            get; 
            set; 
        }

        public bool MouseHover
        {
            protected set;
            get;
        }

        public bool Pressed
        {
            protected set;
            get;
        }

        public Texture2D Image
        {
            get;
            set;
        }

        public ButtonBase(MouseDevice device) : base(device)
        {
            this.Text = string.Empty;
            this.Pressed = false;
            this.MouseHover = false;
        }
    }
}
