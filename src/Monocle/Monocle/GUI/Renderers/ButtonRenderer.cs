using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.GUI
{
    class ButtonRenderer : ControlRenderer<ButtonBase>
    {
        public ButtonRenderer(GUISkin skin)
            : base(skin, "Button")
        { }
                   
        public override void Render(IGUIRenderingContext context, Utils.Time time, ButtonBase control)
        {
            GUIStyle style = control.SpecialStyle == null ? this.DefaultStyle : control.SpecialStyle;
            if (control.Pressed && control.Hover)
            {
                context.DrawFrame(style.Pressed.Frame, control.Bounds, style.Pressed.FrameTint);
                //Draw icon here!
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