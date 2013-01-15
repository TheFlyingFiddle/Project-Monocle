using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.GUI
{
    public struct VisibleElement
    {
        public VisibleElement(Texture2D tex, Rect rect)
            : this()
        {
            // TODO: Complete member initialization
            this.Texture = tex;
            this.SrcRect = rect;
        }

        public Rect SrcRect { get; set; }
        public Texture2D Texture { get; set; }
    }
}
