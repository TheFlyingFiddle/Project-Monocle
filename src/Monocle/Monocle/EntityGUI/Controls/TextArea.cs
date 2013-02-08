using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Graphics;
using OpenTK.Input;
using OpenTK;

namespace Monocle.EntityGUI.Controls
{
    class TextArea : FSMControl<TextArea>
    {
        private const string TEXT_CHANGED_ID = "TEXT_CHANGED";
        protected readonly TextEditor editText;

        private readonly ScrollBar vScrollbar;
        private readonly ScrollBar hScrollbar;

        
        public string Text
        {
            get { return this.editText.ToString(); }
            set { this.editText.SetText(value); }
        }

        public Color TextColor
        {
            get;
            set;
        }

        public Color SelectedColor
        {
            get;
            set;
        }

        public Font Font
        {
            get;
            set;
        }

        public override Vector2 Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                this.hScrollbar.Position = new Vector2(0, value.Y - ScrollBar.DEFAULT_SCROLLBAR_SIZE);
                this.vScrollbar.Position = new Vector2(value.X - ScrollBar.DEFAULT_SCROLLBAR_SIZE, 0);

                this.hScrollbar.Size = new Vector2(value.X, ScrollBar.DEFAULT_SCROLLBAR_SIZE);
                this.vScrollbar.Size = new Vector2(ScrollBar.DEFAULT_SCROLLBAR_SIZE, value.Y);

                base.Size = value;
            }
        }

        public override bool Focused
        {
            get
            {
                return base.Focused;
            }
            internal set
            {
                this.hScrollbar.Focused = true;
                this.vScrollbar.Focused = true;

                base.Focused = value;
            }
        }

        public TextArea(Font font, string text = "", int maxLenght = -1)
        {
            this.Font = font;
            this.editText = new TextEditor(null, "", -1, true);
            this.TextColor = Color.Black;
            this.SelectedColor = Color.Blue;
            this.padding = new Rect(4, 4, 4, 4);

            this.hScrollbar = new ScrollBar(Orientation.Horizontal, 0, 0, 0);
            this.hScrollbar.ButtonColor = Color.Green;
            this.hScrollbar.BackgroundColor = Color.DarkGray;

            this.vScrollbar = new ScrollBar(Orientation.Vertical, 0, 0, 0);
            this.vScrollbar.ButtonColor = Color.Green;
            this.vScrollbar.BackgroundColor = Color.DarkGray;
         }

        protected override GUIFSM<TextArea> CreateFSM()
        {
            var idle = new Idle();
            idle.AddTransition(GUIEventID.FocusGained, 1);

            var focused = new FocusedState();
            focused.AddTransition(GUIEventID.FocusLost, 0);

            return new GUIFSM<TextArea>(this, new GUIState<TextArea>[] { idle, focused });
        }


        internal protected virtual void OnTextChanged(TextChangedEventArgs args)
        {
            this.Invoke(TEXT_CHANGED_ID, args);

            Vector2 contentSize = this.Font.MessureString(this.editText.ToString());

            if (contentSize.X > this.Width - this.padding.W - this.padding.X)
            {
                this.hScrollbar.MaxValue = contentSize.X - this.Width + this.padding.W + this.padding.X + this.vScrollbar.Size.X;
            }
            else
            {
                this.hScrollbar.MaxValue = 0;
            }

            if (contentSize.Y > this.Height - this.padding.H - this.padding.Y)
            {
                this.vScrollbar.MaxValue = contentSize.Y - this.Height + this.padding.H + this.padding.Y + this.hScrollbar.Size.Y;
            }
            else
            {
                this.vScrollbar.MaxValue = 0;
            }
        }

        public event EventHandler<TextChangedEventArgs> TextChanged
        {
            add
            {
                this.AddEvent(TEXT_CHANGED_ID, value);
            }
            remove
            {
                this.RemoveEvent(TEXT_CHANGED_ID, value);
            }
        }

        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);
            if (vScrollbar.Bounds.ContainsPoint(_event.Position))
            {
                MouseButtonEventArgs _internalEvent = new MouseButtonEventArgs(_event.Button, new Vector2(_event.X - vScrollbar.X, _event.Y - vScrollbar.Y));
                this.vScrollbar.OnMouseDownEvent(_internalEvent);
            }
            
            if (hScrollbar.Bounds.ContainsPoint(_event.Position))
            {
                MouseButtonEventArgs _internalEvent = new MouseButtonEventArgs(_event.Button, new Vector2(_event.X - hScrollbar.X, _event.Y - hScrollbar.Y));
                this.hScrollbar.OnMouseDownEvent(_internalEvent);
            }
        }

        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);
            MouseButtonEventArgs _internalEvent0 = new MouseButtonEventArgs(_event.Button, new OpenTK.Vector2(_event.X - vScrollbar.X, _event.Y - vScrollbar.Y));
            this.vScrollbar.OnMouseUpEvent(_internalEvent0);


            MouseButtonEventArgs _internalEvent1 = new MouseButtonEventArgs(_event.Button, new OpenTK.Vector2(_event.X - hScrollbar.X, _event.Y - hScrollbar.Y));
            this.hScrollbar.OnMouseUpEvent(_internalEvent1);
        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);

            MouseMoveEventArgs _internalEvent0 = new MouseMoveEventArgs(new Vector2(_event.X - vScrollbar.X, _event.Y - vScrollbar.Y), _event.Delta);
            this.vScrollbar.OnMouseMoveEvent(_internalEvent0);


            MouseMoveEventArgs _internalEvent1 = new MouseMoveEventArgs(new Vector2(_event.X - hScrollbar.X, _event.Y - hScrollbar.Y), _event.Delta);
            this.hScrollbar.OnMouseMoveEvent(_internalEvent1);
        }



        private void DrawScrollbars(ref Rect area, IGUIRenderer renderer)
        {
            if (this.vScrollbar.MaxValue + this.Width > this.Width)
            {
                Rect scrollArea = new Rect(area.X + this.vScrollbar.X, area.Y, this.vScrollbar.Width, area.H);
                if (renderer.SetSubRectDrawableArea(ref area, ref scrollArea, out scrollArea))
                {
                    this.vScrollbar.Draw(ref scrollArea, renderer);
                }
            }

            if (this.hScrollbar.MaxValue + this.Height > this.Height)
            {
                Rect scrollArea = new Rect(area.X, area.Y + this.hScrollbar.Y, area.W, this.hScrollbar.Height);
                if (renderer.SetSubRectDrawableArea(ref area, ref scrollArea, out scrollArea))
                {
                    this.hScrollbar.Draw(ref scrollArea, renderer);
                }
            }
        }

        class Idle : GUIState<TextArea>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.BackgroundColor);

                Rect _contentRect = new Rect(area.X + Control.padding.X, area.Y + Control.padding.Y,
                                area.W - Control.padding.X - Control.padding.W,
                                area.H - Control.padding.H);

                if (renderer.SetSubRectDrawableArea(ref area, ref _contentRect, out _contentRect))
                {
                    if (Control.editText.Length > 0)
                    {
                        Vector2 offset = new Vector2(Control.hScrollbar.Value, Control.vScrollbar.Value);
                        renderer.DrawMultiLineString(Control.Font, Control.Text, ref _contentRect, Control.TextColor, ref offset);
                    }
                }

                Control.DrawScrollbars(ref area, renderer);
            }
        }

        class FocusedState : GUIState<TextArea>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.BackgroundColor);
                Rect _contentRect = new Rect(area.X + Control.padding.X, area.Y + Control.padding.Y,
                                area.W - Control.padding.X - Control.padding.W,
                                area.H - Control.padding.H - Control.padding.Y);

                if (renderer.SetSubRectDrawableArea(ref area, ref _contentRect, out _contentRect))
                {

                    Vector2 offset = new Vector2(Control.hScrollbar.Value, Control.vScrollbar.Value);
                    renderer.DrawMarkedMultiLineString(Control.Font, Control.editText,
                                           ref _contentRect, ref offset, Control.TextColor, 
                                           Control.SelectedColor);
                }

                Control.DrawScrollbars(ref area, renderer);
            }

            protected internal override void OnCharEvent(CharEventArgs _event)
            {
                base.OnCharEvent(_event);
                var oldText = Control.Text;
                if (Control.editText.ProcessChar(_event.Character))
                {
                    Control.OnTextChanged(new TextChangedEventArgs(oldText, Control.Text));
                }
            }

            protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
            {
                var oldText = Control.Text;
                if (Control.editText.ProcessKey(_event.Key, _event.Modifiers))
                {
                    Control.OnTextChanged(new TextChangedEventArgs(oldText, Control.Text));
                }

                switch (_event.Key)
                {
                    case Key.Left:
                    case Key.Right:
                    case Key.Up:
                    case Key.Down:
                        return true;
                }

                return base.OnKeyDownEvent(_event);
            }

            protected internal override void OnMouseWheelChanged(MouseWheelEventArgs _event)
            {
                base.OnMouseWheelChanged(_event);
                Control.vScrollbar.Value -= _event.Delta * Control.Font.Size;
            }
        }
    }
}
