using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.Graphics
{
    public struct Frame
    {
        private readonly Rect srcRect;
        private readonly Texture2D texture;

        public Rect SrcRect
        {
            get { return this.srcRect; }
        }

        public Texture2D Texture2D
        {
            get { return this.texture; }
        }

        public Frame(Rect srcRect, Texture2D texture)
            : this()
        {
            this.srcRect = srcRect;
            this.texture = texture;
        }
    }
}
