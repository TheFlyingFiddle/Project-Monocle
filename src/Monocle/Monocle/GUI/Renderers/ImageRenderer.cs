using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    class ImageBoxRenderer : ControlRenderer<ImageBox>
    {
        public ImageBoxRenderer(GUISkin skin) : base(skin, "Box") { }

        public override void Render(IGUIRenderingContext context, Utils.Time time, ImageBox control)
        {
            context.DrawFrame(control.Frame, control.Bounds, control.Tint);
        }
    }
}
