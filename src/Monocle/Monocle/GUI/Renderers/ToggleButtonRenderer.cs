using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Graphics;

namespace Monocle.GUI
{
    class ToggleButtonRenderer : GUIRenderer<ToggleButton>
    {
        private string backgroundKey;
        private string overKey;
        private string pressedKey;
        private string downKey;

        public ToggleButtonRenderer()
            : this(LookAndFeelAssets.Button_BG.ToString(),
                   LookAndFeelAssets.Button_Over.ToString(),
                   LookAndFeelAssets.Button_Pressed.ToString(), 
                   LookAndFeelAssets.Button_Down.ToString())
        { }

        public ToggleButtonRenderer(string background, string over,string pressed, string down)
        {
            this.backgroundKey = background;
            this.overKey = over;
            this.pressedKey = pressed;
            this.downKey = down;
        }

        public override void Render(IGUIRenderingContext context, Utils.Time time, ToggleButton control, LookAndFeel lookAndFeel)
        {
            if (control.Down)
            {
                VisibleElement down = lookAndFeel[downKey];
                context.DrawTexture(down.Texture,
                        control.Bounds,
                        control.BGColor,
                        down.SrcRect);
            }
            else if (control.Pressed && control.MouseHover)
            {
                VisibleElement pressed = lookAndFeel[pressedKey];
                context.DrawTexture(pressed.Texture,
                           control.Bounds,
                           control.BGColor,
                           pressed.SrcRect);
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

        private void DrawImage(ToggleButton control, IGUIRenderingContext context)
        {
            Vector2 position = control.Bounds.Center;
            context.DrawTexture(control.Image, position, control.FGColor, control.Image.Bounds);
        }

        private void DrawText(ToggleButton control, IGUIRenderingContext context)
        {
            context.DrawString(control.Text, control.Bounds, control.TextColor, TextAlignment.Center);
        }
    }
}
