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


    public class Font : ICloneable
    {
        public const int TabToSpaceCount = 4;

        public readonly string Face;
        public readonly float Size;
        public readonly float LineHeight;
        public readonly Texture2D Page;
        public readonly CharInfo UnkownChar;

        private readonly CharInfo[] chars;

        public Font(string face, int size, int lineHeight, Texture2D page, CharInfo[] chars)
        {
            this.Face = face;
            this.Size = size;
            this.LineHeight = lineHeight;
            this.Page = page;
            this.chars = chars;

            this.UnkownChar = chars['\u00A5'];
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

        public unsafe Vector2 MessureSubstring(string toMessure, int startIndex, int length)
        {
            if (toMessure.Length < startIndex + length)
                throw new IndexOutOfRangeException("length");

            Vector2 size = Vector2.Zero;
            float cursorX = 0;

            fixed (char* ptr = toMessure)
            {
                for (int i = startIndex; i < startIndex + length; i++)
                {
                    char c = ptr[i];
                    if (c == '\r')
                        continue;

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
                        cursorX += ci.Advance * TabToSpaceCount;
                        continue;
                    }

                    CharInfo info = this[c];
                    if (info == null)
                    {
                        info = this.UnkownChar;
                    }

                    if (i != toMessure.Length - 1)
                        cursorX += info.Advance;
                    else
                        cursorX += info.SrcRect.W;
                }
            }

            if (cursorX > size.X)
                size.X = cursorX;

            size.Y += this.Size;

            return size;
        }

        public Vector2 MessureString(string toMessure)
        {
            return MessureSubstring(toMessure, 0, toMessure.Length);
        }

        public int BestFit(string tofit, float maxWidth)
        {
            return this.BestSubstringFit(tofit, 0, tofit.Length, maxWidth);
        }

        public unsafe int BestSubstringFit(string tofit, int startIndex, int length, float maxWidth)
        {
            float pos = 0;
            fixed (char* ptr = tofit)
            {
                for (int i = startIndex; i < startIndex + length; i++)
                {
                    char c = ptr[i];
                    if (c == '\n' || c == '\r')
                    {
                        return i;
                    }
                    else if (c == '\t')
                    {
                        CharInfo ci = this[' '];
                        pos += ci.Advance * TabToSpaceCount;
                        continue;
                    }

                    CharInfo info = this[ptr[i]];
                    if (info == null)
                    {
                        info = this.UnkownChar;
                    }

                    pos += info.Advance;
                    if (pos > maxWidth)
                    {
                        return i;
                    }
                }
            }

            return tofit.Length;
        }

        public unsafe int BestFitBackWards(string tofit, int from, float maxWidth)
        {
            float pos = 0;
            fixed (char* ptr = tofit)
            {
                for (int i = from - 1; i >= 0; i--)
                {
                    char c = ptr[i];
                    if (c == '\n' || c == '\r')
                        return Math.Max(from - (i + 1), 0);
                    else if (c == '\t')
                    {
                        CharInfo ci = this[' '];
                        pos += ci.Advance * TabToSpaceCount;
                        continue;
                    }

                    CharInfo info = this[ptr[i]];
                    if (info == null)
                    {
                        info = this.UnkownChar;
                    }

                    pos += info.Advance;
                    if (pos > maxWidth)
                    {
                        return from - (i + 1);
                    }
                }
            }

            return tofit.Length;
        }

        public object Clone()
        {
            return this;
        }
    }
}
