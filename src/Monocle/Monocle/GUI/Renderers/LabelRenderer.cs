using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    public class LabelRenderer : ControlRenderer<Label>
    {
        public LabelRenderer(GUISkin skin) : base(skin, "Label") { }

        public override void Render(IGUIRenderingContext context, Utils.Time time, Label control)
        {
            GUIStyle style = control.SpecialStyle == null ? this.DefaultStyle : control.SpecialStyle;
            context.DrawText(style.Font, control.Text, control.TextAlignment, style.Normal.TextColor, control.Bounds);
        }
    }
}
