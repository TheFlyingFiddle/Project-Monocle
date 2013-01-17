using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.Utils;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;

namespace Monocle.GUI
{


    public abstract class GUIControl : Monocle.GUI.IGUIControl 
    {
        private Rect bounds;

        private bool focused;
        private bool enabled;

        /// <summary>
        /// Gets or sets the bounds of the GUIControl.
        /// </summary>
        public Rect Bounds
        {
            get { return this.bounds; }
            set { this.bounds = value; }
        }

        public Rect Dimention
        {
            get { return new Rect(0, 0, this.bounds.Width, this.bounds.Height); }
        }

        public GUIStyle SpecialStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the focused state of this GUIControl.
        /// </summary>
        public bool Focused
        {
            get { return this.focused; }
            set 
            {
                if (this.focused)
                {
                    if (!value)
                        this.OnFocusLost();
                }
                else
                {
                    if (value)
                        this.OnFocusGained();
                }


                this.focused = value; 
            }
        }

        public bool Active
        {
            get;
            set;
        }

        public bool Hover
        {
            get
            {
                return this.CurrentState is MouseEventStateOver ||
                       this.CurrentState is MouseEventStateDragOver;
            } 
                         
        }

        /// <summary>
        /// Gets or sets the order that this GUIControl will be rendered. Larger is later.
        /// </summary>
        public int RenderOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bottom left position of the GUIControl.
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(this.bounds.X, this.bounds.Y); }
            set { this.bounds.X = value.X; this.bounds.Y = value.Y; }
        }

        /// <summary>
        /// Creates a gui control.
        /// </summary>
        public GUIControl()
        {
            this.bounds = Rect.Zero;
            this.enabled = true;
            this.focused = false;
            this.Active = false;

            this.CurrentState = new MouseEventStateNone(this);
        }
        

        internal protected virtual void RegisterMouseEvents(GUIInputDevice inputDevice)
        {
            throw new NotImplementedException();
        }

        internal protected virtual void UnRegisterMouseEvents(GUIInputDevice inputDevice)
        {
            throw new NotImplementedException();
        }

        internal protected virtual void RegisterKeyboardEvents(GUIInputDevice inputDevice)
        {
            throw new NotImplementedException();
        }

        internal protected virtual void UnRegisterKeyboardEvents(GUIInputDevice inputDevice)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invoked when a user clicks the GUIControl.
        /// </summary>
        public event EventHandler<EventArgs> Clicked;

        /// <summary>
        /// Invoked when the GUIControl loses focus.
        /// </summary>
        public event EventHandler<EventArgs> FocusLost;

        /// <summary>
        /// Invoked when the GUIControl gains focus.
        /// </summary>
        public event EventHandler<EventArgs> FocusGained;

        /// <summary>
        /// Invoked when the GUIControl has focus and a user presses a key.
        /// </summary>
        public event EventHandler<KeyPressEventArgs> KeyPress;
        
        /// <summary>
        /// Invoked when a mouse button is pressed over the GUIControl.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown;

        /// <summary>
        /// Invoked when a mouse button is released over the GUIControl.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp;

        /// <summary>
        /// Invoked every frame that the mouse is over the GUIControl.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseStay;

        /// <summary>
        /// Invoked when the mouse enters the GUIControl.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseEnter;

        /// <summary>
        /// INvoked when the mouse exits the GUIControl.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseExit;

        protected virtual void OnFocusGained()
        {
            if (this.FocusGained != null)
                this.FocusGained(this, EventArgs.Empty);
        }

        protected virtual void OnFocusLost()
        {
            if (this.FocusLost != null)
                this.FocusLost(this, EventArgs.Empty);
        }

        protected virtual void OnMouseEnter(MouseEventArgs device)
        {
            if (MouseEnter != null)
                MouseEnter.Invoke(this, new MouseEventArgs(device.X, device.Y));
        }

        protected virtual void OnMouseExit(MouseEventArgs device)
        {
            if (MouseExit != null)
                MouseExit.Invoke(this, new MouseEventArgs(device.X, device.Y));
        }

        protected virtual void OnMouseDown(MouseButtonEventArgs device)
        {
            if (MouseDown != null)
                MouseDown.Invoke(this, device);
        }

        protected virtual void OnMouseUp(MouseButtonEventArgs device)
        {
            if (MouseUp != null)
                MouseUp.Invoke(this, device);
        }

        protected virtual void OnMouseStay(MouseDevice device)
        {
            if (MouseStay != null)
                MouseStay.Invoke(this, new MouseEventArgs(device.X, device.Y));
        }

        protected virtual void OnClicked()
        {
            if (Clicked != null)
                Clicked.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnKeyDown(KeyPressEventArgs args)
        {
            if (this.KeyPress != null)
                this.KeyPress.Invoke(this, args);
        }

        private void MouseMove(object sender, MouseEventArgs args)
        {
            this.CurrentState.update(this.bounds.ContainsPoint(new Vector2(args.X, args.Y)), args);
        }

        private void MouseButtonDown(object sender,  MouseButtonEventArgs args)
        {
            this.CurrentState.MouseButtonDown(args);
        }

        private void MouseButtonUp(object sender, MouseButtonEventArgs args)
        {
            this.CurrentState.MouseButtonUp(args);
        }

        private MouseEventState CurrentState;

        #region Mouse Event Management

        private abstract class MouseEventState
        {
            protected GUIControl control;
            public MouseEventState(GUIControl control) { this.control = control; }

            public abstract void update(bool collision, MouseEventArgs mouse);
            public virtual void MouseButtonDown(MouseButtonEventArgs args) { }
            public virtual void MouseButtonUp(MouseButtonEventArgs args) { }
        }

        private class MouseEventStateNone : MouseEventState
        {
            public MouseEventStateNone(GUIControl control) : base(control) { }
            public override void update(bool collision, MouseEventArgs mouse)
            {
                if (collision)
                {
                    control.OnMouseEnter(mouse);
                    control.CurrentState = new MouseEventStateOver(control);
                }
            }
        }

        private class MouseEventStateOver : MouseEventState
        {

            public MouseEventStateOver(GUIControl control) : base(control) { }

            public override void update(bool collision, MouseEventArgs mouse)
            {
                if (!collision)
                {
                    control.OnMouseExit(mouse);
                    control.CurrentState = new MouseEventStateNone(control);
                }
            }

            public override void MouseButtonDown(MouseButtonEventArgs args)
            {
                if (args.Button == MouseButton.Left)
                {
                    control.CurrentState = new MouseEventStateDragOver(control);
                }

                control.OnMouseDown(args);
            }

            public override void MouseButtonUp(MouseButtonEventArgs args)
            {
                if (args.Button == MouseButton.Left)
                {
                    return;
                }

                control.OnMouseUp(args);
            }
        }

        private class MouseEventStateDragOver : MouseEventState
        {
            public MouseEventStateDragOver(GUIControl control) : base(control) { }

            public override void update(bool collision, MouseEventArgs mouse)
            {
                if (!collision)
                {
                    control.OnMouseExit(mouse);
                    control.CurrentState = new MouseEventStateDrag(control);
                }
            }

            public override void MouseButtonDown(MouseButtonEventArgs args)
            {
                control.OnMouseDown(args);
            }

            public override void MouseButtonUp(MouseButtonEventArgs args)
            {
                if (args.Button == MouseButton.Left)
                {
                    control.OnClicked();
                    control.CurrentState = new MouseEventStateOver(control);
                }

                control.OnMouseUp(args);
            }
        }

        private class MouseEventStateDrag : MouseEventState
        {
            public MouseEventStateDrag(GUIControl control) : base(control) { }

            public override void update(bool collision, MouseEventArgs mouse)
            {
                if (collision)
                {
                    control.OnMouseEnter(mouse);
                    control.CurrentState = new MouseEventStateDragOver(control);
                }
            }

            public override void MouseButtonUp(MouseButtonEventArgs args)
            {
                if (args.Button == MouseButton.Left)
                {
                    control.OnMouseUp(args);
                    control.CurrentState = new MouseEventStateNone(control);
                }
            }
        }


        #endregion
    }
}