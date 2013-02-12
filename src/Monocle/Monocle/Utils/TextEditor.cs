using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using Monocle.EntityGUI;

namespace Monocle.Utils
{
    class TextEditor
    {
        public const char BackSpace = '\u0008';
        public const char Delete = '\u007F';
        public const char Enter = '\n';

        private Func<char, bool> filter;

        private StringBuilder builder;
        private string _last;
        private int markerIndex, selectionIndex;

        public int MarkerIndex
        {
            get { return this.markerIndex; }
            set
            {
                if (value < 0)
                    this.markerIndex = 0;
                else if (value > this.Length)
                    this.markerIndex = this.Length;
                else
                    this.markerIndex = value;
            }
        }

        public int SelectionIndex
        {
            get { return this.selectionIndex; }
            set
            {
                if (value < 0)
                    this.selectionIndex = 0;
                else if (value > this.Length)
                    this.selectionIndex = this.Length;
                else
                    this.selectionIndex = value;
            }
        }

        public int MaxSize
        {
            get;
            set;
        }

        public int Length
        {
            get { return this.builder.Length; }
        }

        public bool Multiline
        {
            get;
            set;
        }

        public char this[int index]
        {
            get { return this.builder[index]; }
        }

        public TextEditor(Func<char, bool> filter = null, string text = "", int maxSize = -1, bool multiline = false)
        {
            this.filter = filter;
            this.MaxSize = maxSize;
            if(maxSize > 0 && text.Length > maxSize)
                throw new ArgumentException(string.Format("The length of the text {0} is larger then maxSize {1}", text, maxSize));

            this.builder = new StringBuilder(text);
            this._last = this.builder.ToString();
            this.markerIndex = 0;
            this.selectionIndex = -1;
            this.Multiline = multiline;
        }

        public void MoveLeft()
        {
            this.markerIndex = (this.markerIndex == 0) ? 0 : this.markerIndex - 1;
        }

        public void MoveRight()
        {
            this.markerIndex = (this.markerIndex == this.Length) ? this.Length : this.markerIndex + 1;
        }

        public void MoveUp()
        {
            for (int i = markerIndex - 1; i >= 0; i--)
            {
                char c = this.builder[i];
                if (c == '\n' || c == '\r')
                {
                    this.markerIndex = Math.Max(0, i);
                    return;
                }
            }
        }

        public void MoveDown()
        {
            for (int i = markerIndex; i < this.builder.Length; i++)
            {
                char c = this.builder[i];
                if (c == '\n' || c == '\r')
                {
                    this.markerIndex = Math.Min(i + 1, this.builder.Length);
                    return;
                }
            }
        }

        public bool ProcessKey(Key key, ModifierKeys modifiers)
        {
            switch (key)
            {
                case Key.Left :
                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (!this.Selected && this.markerIndex > 0)
                        {
                            this.Selected = true;
                        }
                    }
                    else
                    {
                        this.Selected = false;
                    }
                    this.MoveLeft();
                    return false;
                case Key.Right :
                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (!this.Selected && this.markerIndex < this.Length)
                        {
                            this.Selected = true;
                        }
                    }
                    else
                    {
                        this.Selected = false;
                    }

                    this.MoveRight();
                    return false;
                case Key.Up :
                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (!this.Selected && this.markerIndex > 0)
                        {
                            this.Selected = true;
                        }
                    }
                    else
                    {
                        this.Selected = false;
                    }

                    this.MoveUp();
                    return false;
                case Key.Down:
                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (!this.Selected && this.markerIndex < this.Length)
                        {
                            this.Selected = true;
                        }
                    }
                    else
                    {
                        this.Selected = false;
                    }

                    this.MoveDown();
                    return false;
                case Key.BackSpace :
                    return this.ProcessChar(BackSpace);
                case Key.Delete :
                    return this.ProcessChar(Delete);             
                case Key.Enter :
                    return this.ProcessChar(Enter);
                case Key.C :
                    if ((modifiers & ModifierKeys.Ctrl) == ModifierKeys.Ctrl)
                    {
                        System.Windows.Clipboard.SetText(this.SelectedText);
                    }
                    return false;
                case Key.V :
                    if((modifiers & ModifierKeys.Ctrl) == ModifierKeys.Ctrl)
                    {
                        foreach (var item in System.Windows.Clipboard.GetText())
                        {
                            this.ProcessChar(item);
                        }
                    }
                    return false;
                default :
                    return false;
            }
        }

        public bool ProcessChar(char c)
        {
            var result = false;

            if (this.filter != null)
            {
                if (!this.filter(c))
                    return false;
            }
           
            if (this.Selected)
            {
                this.RemoveSelection();
                result = true;
            }
                

            switch (c)
            {
                case BackSpace:
                    if (this.markerIndex == 0)
                        return false;
                    if (!result)
                    {
                        this.MoveLeft();
                        return this.RemoveChar();
                    }
                    return result;
                case Delete: 
                    if (!result)
                        return this.RemoveChar();
                    return result;
                default:
                    return this.InsertChar(c);
            }
        }

        private void RemoveSelection()
        {
            int min = Math.Min(this.selectionIndex, this.markerIndex);
            int max = Math.Max(this.selectionIndex, this.markerIndex);
            this.builder.Remove(min, max - min);
            this._last = this.builder.ToString();
            this.markerIndex = min;
            this.Selected = false;
        }


        public bool Selected 
        {
            get { return this.selectionIndex != -1; }
            set
            {
                if (value)
                    this.selectionIndex = this.markerIndex;
                else
                    this.selectionIndex = -1;
            }
        }

        public string SelectedText 
        {
            get
            {
                if (!this.Selected) return string.Empty;

                int min = Math.Min(this.selectionIndex, this.markerIndex);
                int max = Math.Max(this.selectionIndex, this.markerIndex);
                return this.builder.ToString().Substring(min, max - min);
            }
        }


        public void SelectAll()
        {
            if (this.Length > 0)
            {
                this.selectionIndex = 0;
            }

            this.markerIndex = this.Length;
        }

        private bool RemoveChar()
        {
            if (this.markerIndex < 0 || this.markerIndex == this.Length)
                return false;

            this.builder.Remove(this.markerIndex, 1);
            this._last = this.builder.ToString();
            return true;
        }

        private bool InsertChar(char c)
        {
            if ((c == '\n' || c == '\r') && !this.Multiline)
                return false;

            if (this.Length == this.MaxSize)
                return false;

            this.builder.Insert(this.markerIndex, c);
            this._last = this.builder.ToString();
            this.markerIndex++;
            return true;
        }

        public override string ToString()
        {
            return this._last;
        }

        public override bool Equals(object obj)
        {
            return this.builder.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.builder.GetHashCode();
        }

        public static bool operator ==(TextEditor first, TextEditor second)
        {
            if (object.ReferenceEquals(first, second))
                return true;

            if (((object)first) == null || ((object)second) == null)
                return false;

            return first.builder == second.builder;
        }

        public static bool operator !=(TextEditor first, TextEditor second)
        {
            return !(first == second);
        }

        public void SetText(string p)
        {
            this.Clear();
            if (this.MaxSize == -1 || p.Length <= this.MaxSize)
            {
                this.builder.Append(p);
            }
            else
            {
                this.builder.Append(p.Substring(0, this.MaxSize));
            }

            this._last = this.builder.ToString();
        }

        public void Clear()
        {
            this.builder.Clear();
            this._last = this.builder.ToString();
            this.markerIndex = 0;
            this.selectionIndex = -1;
        }
    }
}
