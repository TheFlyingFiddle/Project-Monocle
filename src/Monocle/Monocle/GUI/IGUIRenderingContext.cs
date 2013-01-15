using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.Utils;
using OpenTK;
using Monocle.Utils.Logging;

namespace Monocle.GUI
{
    public interface IGUIRenderingContext
    {
        void Render(IGUIControl gUIControl, Utils.Time time);
        void DrawTexture(Texture2D texture2D, Rect rect, OpenTK.Graphics.Color4 color, Rect srcRect);
        void DrawTexture(Texture2D texture2D, Vector2 position, OpenTK.Graphics.Color4 color, Rect srcRect);
        void DrawString(string text, Rect rect, OpenTK.Graphics.Color4 color, TextAlignment textAlignment);
    }

    public class GUIRenderingContext : IGUIRenderingContext
    {
        private Batch batch;
        private LookAndFeel lookAndFeel;
        private Matrix4 projection;

        public Matrix4 Projection
        {
            get { return this.projection; }
            set { this.projection = value; }
        }

        public GUIRenderingContext(Batch batch, LookAndFeel lookAndFeel, Matrix4 projection)
        {
            this.batch = batch;
            this.lookAndFeel = lookAndFeel;
            this.projection = projection;
        }

        public void Render(IGUIControl guiControl, Time time)
        {
            this.batch.Begin(ref projection);

            var renderer = guiControl.Renderer;
            renderer.Render(this, time, guiControl, lookAndFeel);
            this.batch.End();
        }


        public void DrawTexture(Texture2D texture2D, Rect dest, OpenTK.Graphics.Color4 tint, Rect srcRect)
        {
            batch.Draw(texture2D, dest, tint, srcRect);
        }

        public void DrawTexture(Texture2D texture2D, Vector2 position, OpenTK.Graphics.Color4 tint, Rect rect)
        {
            batch.Draw(texture2D, position, tint, rect);
        }

        public void DrawString(string text, Rect rect, OpenTK.Graphics.Color4 color, TextAlignment textAlignment)
        {
            Debug.LogInfo("The text: " + text + " was supposed to be written.!");
        }
    }
}