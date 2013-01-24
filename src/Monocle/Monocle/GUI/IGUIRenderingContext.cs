using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.Utils;
using OpenTK;
using Monocle.Utils.Logging;
using OpenTK.Graphics;

namespace Monocle.GUI
{
    public interface IGUIRenderingContext
    {
        void DrawMarkedText(TextureFont font, string text, TextAlignment alignment, Color4 tint, Rect bounds, int startPos, int endPos, Vector2 offset = default(Vector2));
        void DrawText(TextureFont font, string text, TextAlignment alignment, Color4 tint, Rect bounds, Vector2 offset = default(Vector2));
        void DrawFrame(Frame frame, Rect bounds, Color4 tint, Vector2 offset = default(Vector2));
        void DrawBorder(Frame borderFrame, Rect bounds, Color4 tint, int width);

        void Begin(ref Matrix4 projection);
        void End();
    }

    public class GUIRenderingContext : IGUIRenderingContext
    {
        private readonly SpriteBatch fontBatch;

        public GUIRenderingContext(SpriteBatch fontBatch)
        {
            this.fontBatch = fontBatch;
        }

        public void DrawMarkedText(TextureFont font, string text, TextAlignment alignment, Color4 tint, Rect bounds, int startPos, int endPos, Vector2 offset = default(Vector2))
        {
            throw new NotImplementedException();
        }

        public void DrawText(TextureFont font, string text, TextAlignment alignment, Color4 tint, Rect bounds, Vector2 offset = default(Vector2))
        {
        }

        public void DrawFrame(Frame frame, Rect bounds, Color4 tint, Vector2 offset = default(Vector2))
        {
        }

        public void DrawBorder(Frame borderFrame, Rect bounds, Color4 tint, int width)
        {
            bounds.X -= width;
            bounds.Y -= width;
            bounds.Width += width;
            bounds.Height += width;

        }


        public void Begin(ref Matrix4 projection)
        {
        }

        public void End()
        {
        }
    }
}