using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Input;

namespace Monocle.GUI
{
    class ImageBox : GUIControl
    {
        private static readonly ImageRenderer Default_Renderer = new ImageRenderer();

        public Texture2D Image
        {
            get;
            set;
        }

        public Rect SrcRect
        {
            get;
            set;
        }

        public ImageBox(MouseDevice device) : base(device) 
        {
            this.Renderer = Default_Renderer;
        }
    }
}
