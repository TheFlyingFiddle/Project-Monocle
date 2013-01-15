using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    class ImageRenderer : GUIRenderer<ImageBox>
    {
        public override void Render(IGUIRenderingContext context, Utils.Time time, ImageBox control, LookAndFeel lookAndFeel)
        {
            if (control.Image != null)
            {
                context.DrawTexture(control.Image, control.Bounds, control.BGColor, control.SrcRect);
            }
        }
    }
}
