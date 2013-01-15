using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    public class LabelRenderer : GUIRenderer<Label>
    {
        public override void Render(IGUIRenderingContext context, Utils.Time time, Label control, LookAndFeel lookAndFeel)
        {
            context.DrawString(control.Text, control.Bounds, control.BGColor, TextAlignment.Left);
        }
    }
}
