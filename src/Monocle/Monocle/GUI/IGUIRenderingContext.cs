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
        private readonly FontBatch fontBatch;
        private readonly Batch frameBatch;

        public GUIRenderingContext(Batch frameBatch, FontBatch fontBatch)
        {
            this.frameBatch = frameBatch;
            this.fontBatch = fontBatch;
        }

        public void DrawMarkedText(TextureFont font, string text, TextAlignment alignment, Color4 tint, Rect bounds, int startPos, int endPos, Vector2 offset = default(Vector2))
        {
            throw new NotImplementedException();
        }

        public void DrawText(TextureFont font, string text, TextAlignment alignment, Color4 tint, Rect bounds, Vector2 offset = default(Vector2))
        {
            fontBatch.Draw(font, text, alignment, tint, bounds, offset);
        }

        public void DrawFrame(Frame frame, Rect bounds, Color4 tint, Vector2 offset = default(Vector2))
        {
            this.frameBatch.Draw(frame.Texture2D, bounds, tint, frame.SrcRect);
        }

        public void DrawBorder(Frame borderFrame, Rect bounds, Color4 tint, int width)
        {
            bounds.X -= width;
            bounds.Y -= width;
            bounds.Width += width;
            bounds.Height += width;

            this.frameBatch.Draw(borderFrame.Texture2D, bounds, tint, borderFrame.SrcRect);
        }


        public void Begin(ref Matrix4 projection)
        {
            this.fontBatch.Begin(ref projection);
            this.frameBatch.Begin(ref projection);
        }

        public void End()
        {
            this.fontBatch.End();
            this.frameBatch.End();
        }
    }
}