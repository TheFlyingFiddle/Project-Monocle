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

        public CharInfo(Rect rect, Vector2 offset, float advance)
        {
            this.SrcRect = rect;
            this.Offset = offset;
            this.Advance = advance;
        }
    }


    public class TextureFont
    {
        public readonly string Face;
        public readonly float Size;
        public readonly float Base;
        public readonly float LineHeight;

        public Texture2D Page
        {
            get;
            private set;
        }
        
        private Dictionary<char, CharInfo> charMap;

        public TextureFont(string face, int size, int _base, int lineHeight, Texture2D page, Dictionary<char, CharInfo> charMap)
        {
            // TODO: Complete member initialization
            this.Face = face;
            this.Size = size;
            this.Base = _base;
            this.LineHeight = lineHeight;
            this.Page = page;
            this.charMap = charMap;
        }

        public CharInfo this[char character]
        {
            get
            {
                CharInfo info;
                this.charMap.TryGetValue(character, out info);
                    
                return info;
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

    }
}
