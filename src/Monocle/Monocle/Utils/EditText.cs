using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils
{
    public class EditText
    {
        public const char BackSpace = '\u0008';
        public const char Delete = '\u007F';
        private const char NewLine = '\n';
        private const char Return = '\r';

        private StringBuilder builder;
        private int marker;


        public int Marker
        {
            get { return this.marker; }
            set
            {
                if (value < 0)
                    this.marker = 0;
                else if (value > this.Length)
                    this.MaxSize = this.Length;
                else
                    this.marker = value;
            }
        }

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

        public EditText(string text = "", int maxSize = -1, bool multiLine = false)
        {
            this.MaxSize = maxSize;
            if(maxSize > 0 && text.Length > maxSize)
                throw new ArgumentException(string.Format("The length of the text {0} is larger then maxSize {1}", text, maxSize));

            this.builder = new StringBuilder(text);
            this.Multiline = multiLine;
            this.marker = 0;
        }

        public void MoveLeft()
        {
            this.marker = (this.marker == 0) ? 0 : this.marker - 1;
        }

        public void MoveRight()
        {
            this.marker = (this.marker == this.Length) ? this.Length : this.marker + 1;
        }

        private bool AtEnd()
        {
            return this.Length == this.MaxSize;
        }   

        public bool ProcessChar(char c)
        {
            switch (c)
            {
                case BackSpace :
                    if (this.marker == 0)
                        return false;
                    this.MoveLeft();
                    return this.RemoveChar();
                case Delete :
                    return this.RemoveChar();
                default:
                    return this.InsertChar(c);
            }
        }

        private bool RemoveChar()
        {
            if (this.marker < 0 || this.marker == this.Length)
                return false;

            this.builder.Remove(this.marker, 1);
            return true;
        }

        private bool InsertChar(char c)
        {
            if (c == NewLine || c == Return && !this.Multiline)
                return false;
            if (this.Length == this.MaxSize)
                return false;

            this.builder.Insert(this.marker, c);
            this.marker++;
            return true;
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.builder.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.builder.GetHashCode();
        }

        public static bool operator ==(EditText first, EditText second) 
        {
            if (object.ReferenceEquals(first, second))
                return true;

            if (((object)first) == null || ((object)second) == null)
                return false;

            return first.builder == second.builder;
        }

        public static bool operator !=(EditText first, EditText second)
        {
            return !(first == second);
        }

        public void Clear()
        {
            this.builder.Clear();
            this.marker = 0;
        }

        internal void Remove(int min, int max)
        {
            this.builder.Remove(min, max - min);
            this.marker = min;
        }
    }
}