using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.EntityGUI
{
    class Panel : GUIContainer
    {
        public Frame BackgroundImage
        {
            get;
            set;
        }


        public Panel(Frame frame = null)
        {
            this.BackgroundImage = frame;
        }

        protected internal override void DrawContent(ref Rect drawableArea, IGUIRenderer renderer)
        {
            if (this.BackgroundImage != null)
            {
                renderer.DrawFrame(this.BackgroundImage, ref drawableArea, BackgroundColor);
            }
            else
            {
                renderer.DrawRect(ref drawableArea, BackgroundColor);
            }


            base.DrawContent(ref drawableArea, renderer);
        }


    }
}
