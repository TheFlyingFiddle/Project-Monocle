using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Input;

namespace Monocle.GUI
{
    [GUIControl(typeof(ImageBoxRenderer))]
    class ImageBox : GUIControl
    {
        public Frame Frame
        {
            get;
            set;
        }

        public ImageBox() : base() 
        {
        }

        public OpenTK.Graphics.Color4 Tint { get; set; }
    }
}
