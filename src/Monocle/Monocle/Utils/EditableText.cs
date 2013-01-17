using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils
{
    public class EditableText
    {
        private const char BackSpace = '\u0008';
        private const char Delete = '\u007F';

        private StringBuilder builder;

        public int MaxSize
        {
            get;
            set;
        }

        public bool Multiline
        {
            get;
            set;
        }

        public int Length
        {
            get { return this.builder.Length; }
        }

        public EditableText(int maxSize = -1, bool multiline = false)
        {
            this.MaxSize = maxSize;
            if (maxSize <= 0)
                builder = new StringBuilder();
            else
                builder = new StringBuilder(maxSize);
            this.Multiline = multiline;
        }

        public int ProcessChar(char c, int cursorPos)
        {
            if (MaxSize != -1 && this.builder.Length == MaxSize)
                return cursorPos;

            switch (c)
            {
                case BackSpace :
                    if (this.RemoveChar(cursorPos - 1))
                        return cursorPos - 1;
                    return cursorPos;
                case Delete :
                    this.RemoveChar(cursorPos);
                    if (cursorPos != 0)
                        return cursorPos - 1;
                    return cursorPos;
                default:
                    if (this.InsertChar(c, cursorPos))
                        return cursorPos + 1;
                    return cursorPos;
            }
        }

        private bool InsertChar(char c, int cursorPos)
        {
            if (c == '\n' || c == '\r' && !this.Multiline)
                return false;

            if (c == '\r')
                c = '\n';

            this.builder.Insert(cursorPos, c);
            return true;
        }

        private bool RemoveChar(int cursorPos)
        {
            if (cursorPos >= this.builder.Length || cursorPos < 0)
                return false;

            this.builder.Remove(cursorPos, 1);
            return true;
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
