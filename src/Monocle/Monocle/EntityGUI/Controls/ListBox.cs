using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.EntityGUI
{
    class ListBox<T> : FSMControl<ListBox<T>>
    {
        private const string SELECTED_CHANGED_ID = "SELECTED_CHANGED";

        private readonly ScrollBar scrollbar;
        private int selectedIndex = 3;
        
        public List<T> items;
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                var tmp = this.selectedIndex;
                this.selectedIndex = MathHelper.Clamp(0, items.Count - 1, value);
                if (tmp != this.selectedIndex)
                {
                    this.OnSelectedChanged(new SelectedChangedEventArgs<T>(this.items[this.selectedIndex]));
                }
            }
        }

        public Color Color
        {
            get;
            set;
        }

        public Color StripeColor
        {
            get;
            set;
        }

        public Color SelectedColor
        {
            get;
            set;
        }

        public Color TextColor
        {
            get;
            set;
        }

        internal Font Font
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
                this.scrollbar.Position = new Vector2(value.X - ScrollBar.DEFAULT_SCROLLBAR_SIZE, 0);
                this.scrollbar.Size = new Vector2(ScrollBar.DEFAULT_SCROLLBAR_SIZE, value.Y);
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
                this.scrollbar.Focused = value;
                base.Focused = value;
            }
        }

        public void AddItem(T item)
        {
            this.items.Add(item);
            this.scrollbar.MaxValue = this.items.Count * this.Font.Size - this.size.Y;
        }

        public void RemoveItem(T item)
        {
            this.items.Remove(item);
            this.scrollbar.MaxValue = this.items.Count * this.Font.Size - this.size.Y;
        }

        public T SelectedItem
        {
            get 
            {
                if (this.selectedIndex == -1)
                    return default(T);

                return this.items[this.selectedIndex]; 
            }
        }

        public ListBox(Font font)
        {
            this.Font = font;
            this.items = new List<T>();
            this.Color = Color.White;
            this.StripeColor = Color.LightGray;
            this.SelectedColor = Color.Gray;
            this.TextColor = Color.Black;

            this.scrollbar = new ScrollBar(Orientation.Vertical, 0, 100, 0);
            this.scrollbar.Step = this.Font.Size;
            this.scrollbar.ButtonColor = Color.Green;
            this.scrollbar.BackgroundColor = Color.DarkGray;
        }


        protected override GUIFSM<ListBox<T>> CreateFSM()
        {
            var idle = new Idle();
            idle.AddTransition(GUIEventID.FocusGained, 1);
            var focused = new FocusedState();
            focused.AddTransition(GUIEventID.FocusLost, 0);

            return new GUIFSM<ListBox<T>>(this, new GUIState<ListBox<T>>[] { idle, focused });
        }



        private void DecrementSelected()
        {
            if (this.selectedIndex == 0)
            {
                this.selectedIndex = this.items.Count - 1;
            }
            else
            {
                this.selectedIndex--;
            }

            this.MoveScrollbar();
            this.OnSelectedChanged(new SelectedChangedEventArgs<T>(this.items[selectedIndex]));
        }

        private void IncrementSelected()
        {
            this.selectedIndex = (selectedIndex + 1) % this.items.Count;
            this.MoveScrollbar();
            this.OnSelectedChanged(new SelectedChangedEventArgs<T>(this.items[selectedIndex]));
        }

        private void MoveScrollbar()
        {

            float selectedPos = this.selectedIndex * this.Font.Size;
            if (selectedPos > this.scrollbar.Value + this.size.Y || selectedPos  < this.scrollbar.Value)
            {
                this.scrollbar.Value = selectedPos;
            }
        }


        public event EventHandler<SelectedChangedEventArgs<T>> SelectedChanged
        {
            add
            {
                this.AddEvent(SELECTED_CHANGED_ID, value);
            }
            remove
            {
                this.RemoveEvent(SELECTED_CHANGED_ID, value);
            }
        }

        protected virtual void OnSelectedChanged(SelectedChangedEventArgs<T> selectedChangedEventArgs)
        {
            this.Invoke(SELECTED_CHANGED_ID, selectedChangedEventArgs);
        }

        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);
            Rect bounds = new Rect(this.scrollbar.X,0, this.size.X, this.size.Y);
            if (bounds.ContainsPoint(_event.Position))
            {
                MouseButtonEventArgs _internalEvent = new MouseButtonEventArgs(_event.Button, new OpenTK.Vector2(_event.X - scrollbar.X, _event.Y - scrollbar.Y));
                this.scrollbar.OnMouseDownEvent(_internalEvent);
            }
        }

        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);
            MouseButtonEventArgs _internalEvent = new MouseButtonEventArgs(_event.Button, new OpenTK.Vector2(_event.X - scrollbar.X, _event.Y - scrollbar.Y));
            this.scrollbar.OnMouseUpEvent(_internalEvent);
        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);
            MouseMoveEventArgs _internalEvent = new MouseMoveEventArgs(new Vector2(_event.X - scrollbar.X, _event.Y - scrollbar.Y), _event.Delta);
            this.scrollbar.OnMouseMoveEvent(_internalEvent);
        }


        class Idle : GUIState<ListBox<T>>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.Color);
                for (int i = 0; i < Control.items.Count; i++)
                {
                    Rect rect = new Rect(area.X, -Control.scrollbar.Value + area.Y + i * Control.Font.Size, area.W, Control.Font.Size);
                    Rect tmp;
                    if (renderer.SetSubRectDrawableArea(ref area, ref rect, out tmp))
                    {
                        Color c = (i == Control.selectedIndex ? Control.SelectedColor : (i % 2 == 0 ? Control.Color : Control.StripeColor));

                        renderer.DrawRect(ref rect, c);
                        renderer.DrawString(Control.Font, Control.items[i].ToString(), ref rect, Control.TextColor, TextAlignment.Left);
                    }
                }

                if (Control.items.Count * Control.Font.Size > Control.size.Y)
                {
                    Rect scrollRect = new Rect(area.X + Control.scrollbar.X, area.Y, Control.scrollbar.Width, area.H);
                    if (renderer.SetSubRectDrawableArea(ref area, ref scrollRect, out scrollRect))
                    {
                        Control.scrollbar.Draw(ref scrollRect, renderer);
                    }
                }
            }
        }

        class FocusedState : Idle
        {
            protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
            {
                switch (_event.Key)
                {
                    case OpenTK.Input.Key.Left:
                    case OpenTK.Input.Key.Up:
                        Control.DecrementSelected();               
                        return true;
                    case OpenTK.Input.Key.Right:
                    case OpenTK.Input.Key.Down:
                        Control.IncrementSelected();
                        return true;

                }

                return base.OnKeyDownEvent(_event);
            }

            protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
            {
                base.OnMouseDownEvent(_event);

                if (_event.Button == MouseButton.Left || _event.Button == MouseButton.Right)
                {
                    int selected = (int)((_event.Y + Control.scrollbar.Value) / Control.Font.Size);
                    Control.SelectedIndex = selected; 
                }
            }

            protected internal override void OnMouseWheelChanged(MouseWheelEventArgs _event)
            {
                base.OnMouseWheelChanged(_event);
                Control.scrollbar.Value = MathHelper.Clamp(0, Control.Font.Size * Control.items.Count - Control.size.Y, Control.scrollbar.Value - _event.Delta * Control.Font.Size);
            }
        }
    }
}
