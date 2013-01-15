using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.GUI
{
    class ButtonRenderer : GUIRenderer<ButtonBase>
    {
        private string backgroundKey;
        private string overKey;
        private string downKey;

        public ButtonRenderer()
            : this(LookAndFeelAssets.Button_BG.ToString(),
                   LookAndFeelAssets.Button_Over.ToString(),
                   LookAndFeelAssets.Button_Pressed.ToString())
        { }

        public ButtonRenderer(string background, string over, string down)
        {
            this.backgroundKey = background;
            this.overKey = over;
            this.downKey = down;
        }
                   
        public override void Render(IGUIRenderingContext context, Utils.Time time, ButtonBase control, LookAndFeel lookAndFeel)
        {

            if (control.Pressed && control.MouseHover)
            {
                VisibleElement down = lookAndFeel[downKey];
                context.DrawTexture(down.Texture,
                           control.Bounds,
                           control.BGColor,
                           down.SrcRect);
            }
            else if (control.MouseHover)
            {
                VisibleElement over = lookAndFeel[overKey];
                context.DrawTexture(over.Texture,
                        control.Bounds,
                        control.BGColor,
                        over.SrcRect);
            }
            else
            {
                VisibleElement background = lookAndFeel[backgroundKey];
                context.DrawTexture(background.Texture,
                        control.Bounds,
                        control.BGColor,
                        background.SrcRect);
            }


            if (control.Image != null)
                DrawImage(control, context);

            DrawText(control, context);
        }

        private void DrawImage(ButtonBase control, IGUIRenderingContext context)
        {
            Vector2 position = control.Bounds.Center;
            context.DrawTexture(control.Image, position, control.FGColor, control.Image.Bounds);
        }

        private void DrawText(ButtonBase control, IGUIRenderingContext context)
        {
            context.DrawString(control.Text, control.Bounds, control.TextColor, TextAlignment.Center);               
        }
    }
}