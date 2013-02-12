using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.EntityGUI
{
    class ImageBox : GUIControl
    {
        public Frame Image
        {
            get;
            set;
        }

        public Color ImageColor
        {
            get;
            set;
        }

        public ImageBox(Frame image)
        {
            this.Image = image;
            this.ImageColor = Color.White;
        }

        protected internal override void Draw(ref Graphics.Rect drawableArea, IGUIRenderer renderer)
        {
            Color c = this.focused ? this.BackgroundColor : Color.AddContrast(this.BackgroundColor, 0.2f);

            drawableArea.W = this.Width;
            drawableArea.H = this.Height;

            renderer.DrawRect(ref drawableArea, c);
            if (Image != null)
                renderer.DrawFrame(Image, ref drawableArea, ImageColor);
        }
    }
}
