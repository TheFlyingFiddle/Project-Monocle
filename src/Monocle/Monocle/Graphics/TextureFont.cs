using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Monocle.Graphics
{
    public class CharInfo
    {
        public readonly float Advance;
        private readonly Vector4 textureCoords;

        public Vector4 TextureCoords
        {
            get { return this.textureCoords; }
        }

        public Rect SrcRect
        {
            get;
            private set;
        }

        public Vector2 Offset
        {
            get;
            private set;
        }

        public CharInfo(Texture2D page, Rect rect, Vector2 offset, float advance)
        {
            this.SrcRect = rect;
            this.Offset = offset;
            this.Advance = advance;
         
            this.textureCoords = this.SrcRect.ToVector4();
            this.textureCoords.X /= (page.Width);
            this.textureCoords.Z /= (page.Width);
            this.textureCoords.Y /= (page.Height);
            this.textureCoords.W /= (page.Height);
        }
    }


    public class TextureFont : ICloneable
    {
        public readonly string Face;
        public readonly float Size;
        public readonly float LineHeight;
        public readonly Texture2D Page;

        private readonly CharInfo[] chars;

        public TextureFont(string face, int size, int lineHeight, Texture2D page, CharInfo[] chars)
        {
            // TODO: Complete member initialization
            this.Face = face;
            this.Size = size;
            this.LineHeight = lineHeight;
            this.Page = page;
            this.chars = chars;
        }

        public CharInfo this[char character]
        {
            get
            {
                if (character > this.chars.Length)
                    return null;

                return this.chars[character];
            }
        }

        public Vector2 MessureString(string toMessure)
        {
            Vector2 size = Vector2.Zero;
            float cursorX = 0;
            for (int i = 0; i < toMessure.Length; i++)
            {
                char c = toMessure[i];

                if (c == '\n')
                {
                    size.Y += this.LineHeight;
                    if (cursorX > size.X)
                        size.X = cursorX;

                    cursorX = 0;
                    continue;
                }
                else if (c == '\t')
                {
                    CharInfo ci = this[' '];
                    cursorX += ci.Advance * 4;
                    continue;
                }

                CharInfo info = this[c];

                cursorX += info.Advance;
            }

            if (cursorX > size.X)
                size.X = cursorX;

            size.Y += this.LineHeight;

            return size;
        }


        public object Clone()
        {
            return this;
        }
    }
}
