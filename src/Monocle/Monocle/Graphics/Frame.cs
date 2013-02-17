using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.Graphics
{
    public class Frame
    {
        private readonly Vector4 textureCoordinates;
        private readonly Rect srcRect;
        private readonly Texture2D texture;

        public Vector4 TextureCoordinates
        {
            get { return this.textureCoordinates; }
        }

        public Rect SrcRect
        {
            get { return this.srcRect; }
        }

        public Texture2D Texture2D
        {
            get { return this.texture; }
        }

        public Frame(Rect srcRect, Texture2D texture)
        {
            this.srcRect = srcRect;
            this.texture = texture;

            this.textureCoordinates = srcRect.ToVector4();
            textureCoordinates.X /= texture.Width;
            textureCoordinates.Z /= texture.Width;
            textureCoordinates.Y /= texture.Height;
            textureCoordinates.W /= texture.Height;
        }
    }
}
