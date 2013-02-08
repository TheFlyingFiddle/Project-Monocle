using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.EntityGUI
{
    public class ResizeEventArgs : EventArgs
    {
        public readonly Vector2 OldSize;
        public readonly Vector2 NewSize;

        public ResizeEventArgs(Vector2 oldSize, Vector2 newSize)
        {
            this.OldSize = oldSize;
            this.NewSize = newSize;
        }
    }

    public class ValueChangedEventArgs<T> : EventArgs
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public ValueChangedEventArgs(T oldValue, T newValue) 
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    public class TextChangedEventArgs : ValueChangedEventArgs<string>
    {
        public TextChangedEventArgs(string oldText, string newText)
         : base(oldText, newText) { }
    }

    public class FloatChangedEventArgs : ValueChangedEventArgs<float>
    {
        public FloatChangedEventArgs(float o, float n)
            : base(o, n) { }
    }

    public class SelectedChangedEventArgs<T> : EventArgs
    {
        public readonly T Selected;
        public SelectedChangedEventArgs(T selected)
        {
            this.Selected = selected;
        }
    }

    public class FrameChangedEventArgs : EventArgs
    {
        public readonly Frame Old;
        public readonly Frame New;

        public FrameChangedEventArgs(Frame old, Frame _new)
        {
            this.Old = old;
            this.New = _new;
        }
    }
}
