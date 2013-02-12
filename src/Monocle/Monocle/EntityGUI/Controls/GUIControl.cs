using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Core;
using Monocle.Graphics;
using OpenTK;
using OpenTK.Graphics;
using Monocle.Utils;
using OpenTK.Input;

namespace Monocle.EntityGUI
{
    abstract class GUIControl : ICloneable
    {
        private const string RESIZE_ID = "RESIZE";
        private const string FOCUS_LOST_ID = "FOCUS_LOST";
        private const string FOCUS_GAINED_ID = "FOCUS_GAINED";
        private const string CLICKED_ID = "CLICKED";

        private const string MOUSE_DOWN_ID = "MOUSE_DOWN";
        private const string MOUSE_UP_ID = "MOUSE_UP";
        private const string MOUSE_MOVE_ID = "MOUSE_MOVE";
        private const string MOUSE_ENTER_ID = "MOUSE_ENTER";
        private const string MOUSE_EXIT_ID = "MOUSE_EXIT";
        private const string MOUSE_WHEEL_ID = "MOUSE_WHEEL";

        private const string KEY_DOWN_ID = "KEY_DOWN";
        private const string KEY_UP_ID = "KEY_UP";
        private const string CHAR_ID = "CHAR";

        private readonly Dictionary<string, object> _eventMap;
        protected Origin origin;
        protected bool focused;
        protected Vector2 position, size;
        protected Rect padding, bounds;



        public float X { get { return this.bounds.X; } }
        public float Y { get { return this.bounds.Y; } }
        public float Width { get { return this.bounds.W; } }
        public float Height { get { return this.bounds.H; } }

        public string Name
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get { return this.position; }
            set 
            {
                this.position = value;
                this.CalcBounds(); 
            }
        }

        public virtual Vector2 Size
        {
            get { return this.size; }
            set 
            {
                var tmp = this.size;
                this.size = value;
                if (tmp != size)
                {
                    this.OnResized(new ResizeEventArgs(tmp, this.size));
                }
            } 
        }

        public Origin Origin
        {
            get { return this.origin; }
            set
            {
                this.origin = value;
                this.CalcBounds();
            }
        }

        public bool Visible
        {
            get;
            set;
        }

        public int FocusIndex
        {
            get;
            set;
        }

        public bool Focusable
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }

        public Rect Bounds
        {
            get
            {
                return this.bounds;
            }
        }

        private void CalcBounds()
        {       
            switch (this.Origin)
            {
                case Origin.TopLeft:
                    this.bounds = new Rect(this.position.X, this.position.Y, this.size.X, this.size.Y);
                    break;
                case Origin.TopRight:
                    this.bounds = new Rect(this.position.X - this.size.X, this.position.Y, this.size.X, this.size.Y);
                    break;
                case Origin.Center:
                    this.bounds = new Rect(this.position.X - this.size.X / 2, this.position.Y - this.size.Y / 2, this.size.X, this.size.Y);
                    break;
                case Origin.BottomLeft:
                    this.bounds = new Rect(this.position.X, this.position.Y - this.size.Y, this.size.X, this.size.Y);
                    break;
                case Origin.BottomRight:
                    this.bounds = new Rect(this.position.X - this.size.X, this.position.Y - this.size.Y, this.size.X, this.size.Y);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public virtual Rect Padding
        {
            get { return this.padding; }
            internal protected set
            {
                this.padding = value;
            }
        }

        public virtual bool Focused
        {
            get { return this.focused; }
            internal set
            {
                this.focused = value;
                if (value)
                {
                    this.OnFocusGained(EventArgs.Empty);
                }
                else
                {
                    this.OnFocusLost(EventArgs.Empty);
                }
            }
        }

        public GUIControl()
        {
            this.BackgroundColor = Color.White;
            this.Visible = true;
            this.Focusable = true;
            this._eventMap = new Dictionary<string, object>();
        }

        protected virtual void OnResized(ResizeEventArgs resizeEventArgs)
        {
            this.CalcBounds();
            this.Invoke(RESIZE_ID, resizeEventArgs);
        }

        protected virtual void OnFocusLost(EventArgs eventArgs)
        {
            this.Invoke(FOCUS_LOST_ID, eventArgs);
        }

        protected virtual void OnFocusGained(EventArgs eventArgs)
        {
            this.Invoke(FOCUS_GAINED_ID, eventArgs);
        }

        #region Event Management

        protected void AddEvent<T>(string id, EventHandler<T> value) where T : EventArgs
        {
            object obj;
            if (_eventMap.TryGetValue(id, out obj))
            {
                EventHandler<T> handler = (EventHandler<T>)obj;
                handler += value;
                _eventMap.Remove(id);
                _eventMap[id] = handler;
            }
            else
            {
                _eventMap[id] = value;
            }
        }

        protected void RemoveEvent<T>(string id, EventHandler<T> value) where T : EventArgs
        {
            object obj;
            if (_eventMap.TryGetValue(id, out obj))
            {
                EventHandler<T> handler = (EventHandler<T>)obj;
                handler -= value;
                _eventMap.Remove(id);
                
                if(handler != null)
                    _eventMap[id] = handler;
            }
        }


        protected void Invoke<T>(string id, T value) where T : EventArgs
        {
            object obj;
            if (_eventMap.TryGetValue(id, out obj))
            {
                EventHandler<T> handler = (EventHandler<T>)obj;
                if (handler != null)
                {
                    handler.Invoke(this, value);
                }
            }
        }

        #endregion

        public event EventHandler<ResizeEventArgs> Resized
        {
            add
            {
                this.AddEvent<ResizeEventArgs>(RESIZE_ID, value);
            }
            remove
            {
                this.RemoveEvent<ResizeEventArgs>(RESIZE_ID, value);
            }
        }

        public event EventHandler<EventArgs> FocusLost
        {
            add
            {
                this.AddEvent<EventArgs>(FOCUS_LOST_ID, value);
            }
            remove
            {
                this.RemoveEvent<EventArgs>(FOCUS_LOST_ID, value);
            }
        }

        public event EventHandler<EventArgs> FocusGained
        {
            add
            {
                this.AddEvent<EventArgs>(FOCUS_GAINED_ID, value);
            }
            remove
            {
                this.RemoveEvent<EventArgs>(FOCUS_GAINED_ID, value);
            }
        }

        public event EventHandler<EventArgs> Clicked
        {
            add
            {
                this.AddEvent<EventArgs>(CLICKED_ID, value);
            }
            remove
            {
                this.RemoveEvent<EventArgs>(CLICKED_ID, value);
            }
        }

        public event EventHandler<KeyEventArgs> KeyDown
        {
            add
            {
                this.AddEvent(KEY_DOWN_ID, value);
            }
            remove
            {
                this.RemoveEvent(KEY_DOWN_ID, value);
            }
        }

        public event EventHandler<KeyEventArgs> KeyUp
        {
            add
            {
                this.AddEvent(KEY_UP_ID, value);
            }
            remove
            {
                this.RemoveEvent(KEY_UP_ID, value);
            }
        }

        public event EventHandler<CharEventArgs> Char
        {
            add
            {
                this.AddEvent(CHAR_ID, value);
            }
            remove
            {
                this.RemoveEvent(CHAR_ID, value);
            }
        }

        public event EventHandler<MouseMoveEventArgs> MouseMove
        {
            add
            {
                this.AddEvent(MOUSE_MOVE_ID, value);
            }
            remove
            {
                this.RemoveEvent(MOUSE_MOVE_ID, value);
            }
        }

        public event EventHandler<MouseButtonEventArgs> MouseDown
        {
            add
            {
                this.AddEvent(MOUSE_DOWN_ID, value);
            }
            remove
            {
                this.RemoveEvent(MOUSE_DOWN_ID, value);
            }
        }

        public event EventHandler<MouseButtonEventArgs> MouseUp
        {
            add
            {
                this.AddEvent(MOUSE_UP_ID, value);
            }
            remove
            {
                this.RemoveEvent(MOUSE_UP_ID, value);
            }
        }

        public event EventHandler<MouseMoveEventArgs> MouseEnter
        {
            add
            {
                this.AddEvent(MOUSE_ENTER_ID, value);
            }
            remove
            {
                this.RemoveEvent(MOUSE_ENTER_ID, value);
            }
        }

        public event EventHandler<MouseMoveEventArgs> MouseExit
        {
            add
            {
                this.AddEvent(MOUSE_EXIT_ID, value);
            }
            remove
            {
                this.RemoveEvent(MOUSE_EXIT_ID, value);
            }
        }

        public event EventHandler<MouseWheelEventArgs> WheelChanged
        {
            add
            {
                this.AddEvent(MOUSE_WHEEL_ID, value);
            }
            remove
            {
                this.RemoveEvent(MOUSE_WHEEL_ID, value);
            }
        }

        internal protected abstract void Draw(ref Rect drawableArea, IGUIRenderer renderer);
        internal protected virtual void Update(Time time) { }

        internal protected virtual void OnClicked()
        {
            this.Invoke(CLICKED_ID, EventArgs.Empty);
        }

        internal protected virtual bool OnKeyDownEvent(KeyEventArgs _event)
        {
            if (this.Focused && _event.Key == Key.Enter)
            {
                this.OnClicked();
            }

            this.Invoke(KEY_DOWN_ID, _event);
            return false;
        }

        internal protected virtual void OnKeyUpEvent(KeyEventArgs _event)
        {
            this.Invoke(KEY_DOWN_ID, _event);
        }

        internal protected virtual void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            this.Invoke(MOUSE_MOVE_ID, _event);
        }

        internal protected virtual void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            this.Invoke(MOUSE_DOWN_ID, _event);
        }

        internal protected virtual void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            this.Invoke(MOUSE_UP_ID, _event);
        }

        internal protected virtual void OnCharEvent(CharEventArgs _event)
        {
            this.Invoke(CHAR_ID, _event);
        }

        internal protected virtual void OnMouseEnter(MouseMoveEventArgs _event)
        {
            this.Invoke(MOUSE_ENTER_ID, _event);
        }

        internal protected virtual void OnMouseExit(MouseMoveEventArgs _event)
        {
            this.Invoke(MOUSE_EXIT_ID, _event);
        }

        internal protected virtual void OnMouseWheelChanged(MouseWheelEventArgs _event)
        {
            this.Invoke(MOUSE_WHEEL_ID, _event);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}