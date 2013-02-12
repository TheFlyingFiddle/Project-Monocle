using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Monocle.EntityGUI
{
    enum GUIEventID
    {
        FocusGained,
        FocusLost,
        LeftMouseDown,
        LeftMouseUp,
        MouseDown,
        MouseUp,
        MouseMove,
        MouseEnter,
        MouseExit,
        Char,
        KeyDown,
        KeyUp,
        Clicked,
        Update
    }

    enum MouseButton
    {
        Left = 0,
        Right,
        Middle,
        Button1,
        Button2,
        Button3,
        Button4,
        Button5,
        Button6,
        Button7,
        Button8,
        Button9,
        LastButton
    }

    [Flags]
    enum ModifierKeys
    {
        None = 0x00,
        Shift = 0x01,
        Ctrl = 0x02,
        Alt = 0x04
    }

    class MouseWheelEventArgs : EventArgs
    {
        public readonly float Delta;
        public readonly ModifierKeys Modifiers;

        public MouseWheelEventArgs(float delta, ModifierKeys modifiers)
        {
            this.Delta = delta;
            this.Modifiers = modifiers;
        }
    }


    class MouseMoveEventArgs : EventArgs
    {
        public readonly Vector2 Position;
        public readonly Vector2 Delta;

        public float X
        {
            get { return this.Position.X; }
        }

        public float Y
        {
            get { return this.Position.Y; }
        }

        public float DX
        {
            get { return this.Delta.X; }
        }

        public float DY
        {
            get { return this.Delta.Y; }
        }

        public MouseMoveEventArgs(Vector2 position, Vector2 delta)
        {
            this.Position = position;
            this.Delta = delta;
        }
    }

    class MouseButtonEventArgs : EventArgs
    {
        public readonly Vector2 Position;
        public readonly MouseButton Button;
        public readonly ModifierKeys Modifiers;

        public float X
        {
            get { return this.Position.X; }
        }

        public float Y
        {
            get { return this.Position.Y; }
        }

        public MouseButtonEventArgs(MouseButton button, Vector2 position, ModifierKeys modifiers)
        {
            this.Button = button;
            this.Position = position;
            this.Modifiers = modifiers;
        }
    }

    class KeyEventArgs : EventArgs
    {
        public readonly OpenTK.Input.Key Key;
        public readonly ModifierKeys Modifiers;
        public KeyEventArgs(OpenTK.Input.Key key, ModifierKeys modifiers)
        {
            this.Key = key;
            this.Modifiers = modifiers;
        }
    }

    class CharEventArgs : EventArgs
    {
        public readonly char Character;
        public CharEventArgs(char character)
        {
            this.Character = character;
        }
    }
}