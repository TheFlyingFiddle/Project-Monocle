using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using OpenTK;

namespace Monocle.EntityGUI
{
    class GUIEventSystem
    {
        private readonly GUIContainer root;
        private readonly KeyboardDevice keyboard;

        public GUIEventSystem(GUIContainer root, MouseDevice mouse, KeyboardDevice keyboard, INativeWindow window)
        {
            this.root = root;

            mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(mouse_Move);
            mouse.ButtonDown += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(mouse_ButtonDown);
            mouse.ButtonUp += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(mouse_ButtonUp);
            mouse.WheelChanged += new EventHandler<OpenTK.Input.MouseWheelEventArgs>(mouse_WheelChanged);

            this.keyboard = keyboard;
            keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(keyboard_KeyDown);
            keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(keyboard_KeyUp);
            window.KeyPress += new EventHandler<KeyPressEventArgs>(window_KeyPress);

        }

        #region Mouse



        void mouse_WheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            Vector2 position = new Vector2(e.X, e.Y);
            root.OnMouseWheelChanged(new MouseWheelEventArgs(e.DeltaPrecise, GetModifierKeys()));
        }

        void mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            MouseButton button = (MouseButton)e.Button;
            Vector2 position = new Vector2(e.X, e.Y); 

            root.OnMouseUpEvent(new MouseButtonEventArgs(button, position, GetModifierKeys()));
        }

        void mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            MouseButton button = (MouseButton)e.Button;
            Vector2 position = new Vector2(e.X, e.Y);
            if (root.Bounds.ContainsPoint(position))
            {
                root.OnMouseDownEvent(new MouseButtonEventArgs(button, position, GetModifierKeys()));
            }
        }

        void mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            Vector2 pos = new Vector2(e.X, e.Y);
            Vector2 delta = new Vector2(e.XDelta, e.YDelta);
            root.OnMouseMoveEvent(new MouseMoveEventArgs(pos, delta));
        }

        #endregion

        #region Keyboard

        void keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (root.Focused)
            {
                this.root.OnKeyDownEvent(new KeyEventArgs(e.Key, this.GetModifierKeys()));
                if(!root.Focused) 
                {
                    this.root.Focused = true;
                }
            }
        }

        void keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (root.Focused)
            {
                this.root.OnKeyUpEvent(new KeyEventArgs(e.Key, this.GetModifierKeys()));
            }
        }

        private ModifierKeys GetModifierKeys()
        {
            ModifierKeys modifiers = ModifierKeys.None;
            if (keyboard[Key.ShiftLeft] || keyboard[Key.ShiftRight])
                    modifiers |= ModifierKeys.Shift;

            if (keyboard[Key.ControlLeft] || keyboard[Key.ControlRight])
                    modifiers |= ModifierKeys.Ctrl;

            if (keyboard[Key.AltLeft] || keyboard[Key.AltRight])
                     modifiers |= ModifierKeys.Alt;


            return modifiers;
        }

        #endregion

        #region Window

        void window_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t' || e.KeyChar == '\r' || char.IsControl(e.KeyChar))
                return;

            if (root.Focused)
            {
                root.OnCharEvent(new CharEventArgs(e.KeyChar));
            }
        }

        #endregion
    }
}
