using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.GUI
{
    public struct Frame
    {
        public Rect SrcRect
        {
            get;
            set;
        }

        public Texture2D Texture2D
        {
            get;
            set;
        }

        public Frame(Rect srcRect, Texture2D texture)
            : this()
        {
            SrcRect = srcRect;
            Texture2D = texture;
        }
    }
}
