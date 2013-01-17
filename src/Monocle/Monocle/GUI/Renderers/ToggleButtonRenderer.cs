using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Graphics;

namespace Monocle.GUI
{
    class ToggleButtonRenderer : ControlRenderer<ToggleButton>
    {

        public ToggleButtonRenderer(GUISkin skin) : base(skin, "ToggleButton") { }

        public override void Render(IGUIRenderingContext context, Utils.Time time, ToggleButton control)
        {
            GUIStyle style = control.SpecialStyle == null ? this.DefaultStyle : control.SpecialStyle;

            if (control.Active)
            {
                context.DrawFrame(style.Active.Frame, control.Bounds, style.Active.FrameTint);
                context.DrawText(style.Font, control.Text, TextAlignment.Center, style.Active.TextColor, control.Bounds);
            }
            else if (control.Pressed && control.Hover)
            {
                context.DrawFrame(style.Pressed.Frame, control.Bounds, style.Pressed.FrameTint);
                context.DrawText(style.Font, control.Text, TextAlignment.Center, style.Pressed.TextColor, control.Bounds);
            }
            else if (control.Hover)
            {
                context.DrawFrame(style.Hover.Frame, control.Bounds, style.Hover.FrameTint);
                context.DrawText(style.Font, control.Text, TextAlignment.Center, style.Hover.TextColor, control.Bounds);
            }
            else
            {
                context.DrawFrame(style.Normal.Frame, control.Bounds, style.Normal.FrameTint);
                context.DrawText(style.Font, control.Text, TextAlignment.Center, style.Normal.TextColor, control.Bounds);
            }
        }
    }
}
