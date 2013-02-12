using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.EntityGUI.Controls
{
    abstract class ScrollableControl : GUIControl
    {
        private const float Max_Step_Value = 100;


        private readonly ScrollBar vScrollbar;
        private readonly ScrollBar hScrollbar;
        private Rect contentArea;

        protected ScrollBar VScrollBar
        {
            get { return this.vScrollbar; }
        }

        protected ScrollBar HScrollBar
        {
            get { return this.hScrollbar; }
        }

        protected Vector2 ScrollOffset
        {
            get { return new Vector2(this.hScrollbar.Value, this.vScrollbar.Value); }
        }

        protected float ScrollStep
        {
            get { return this.vScrollbar.Step; }
            set
            {
                this.vScrollbar.Step = this.hScrollbar.Step = Math.Min(value, Max_Step_Value);
            }
        }

        protected Rect ContentArea
        {
            get { return this.contentArea; }
            set
            {
                var tmp = this.contentArea;
                this.contentArea = new Rect(value.X + this.padding.X, value.Y + this.padding.Y, 
                                            Math.Max(value.W + ScrollBar.DEFAULT_SCROLLBAR_SIZE, this.Width - this.padding.W - this.padding.X),
                                            Math.Max(value.H + ScrollBar.DEFAULT_SCROLLBAR_SIZE, this.Height - this.padding.H - this.padding.Y));
                if (tmp != value)
                    this.OnContentAreaChanged(new ValueChangedEventArgs<Rect>(tmp, value));
            }
        }

        public ScrollableControl()
        {
            this.vScrollbar = new ScrollBar(Orientation.Vertical, 0, 0, 0);
            this.hScrollbar = new ScrollBar(Orientation.Horizontal, 0, 0, 0);

            this.SetupScrollbars();
        }

        private void SetupScrollbars()
        {
            this.vScrollbar.BackgroundColor = Color.Transparent;
            this.vScrollbar.ButtonColor = Color.Gray;
            this.vScrollbar.Focused = true;

            this.hScrollbar.BackgroundColor = Color.Transparent;
            this.hScrollbar.ButtonColor = Color.Gray;
            this.hScrollbar.Focused = true;
        }

        protected virtual void OnContentAreaChanged(ValueChangedEventArgs<Rect> valueChangedEventArgs)
        {
            FixScrollbarMinMax();
        }

        private void FixScrollbarMinMax()
        {
            this.hScrollbar.MinValue = this.contentArea.Left < 0 ? this.contentArea.Left : 0;
            this.hScrollbar.MaxValue = (this.contentArea.Right - this.Width) > 0 ? (this.contentArea.Right - this.Width) : 0;

            this.vScrollbar.MinValue = this.contentArea.Top < 0 ? this.contentArea.Top : 0;
            this.vScrollbar.MaxValue = (this.contentArea.Bottom - this.Height) > 0 ? (this.contentArea.Bottom - this.Height) : 0; ;

            if (this.hScrollbar.MinValue < 0 || this.hScrollbar.MaxValue > 0)
            {
                this.hScrollbar.Visible = true;
                this.hScrollbar.Step = (hScrollbar.MaxValue - hScrollbar.MinValue) / 5;
            }
            else
                this.hScrollbar.Visible = false;

            if (this.vScrollbar.MinValue < 0 || this.vScrollbar.MaxValue > 0)
            {
                this.vScrollbar.Visible = true;
                this.vScrollbar.Step = (vScrollbar.MaxValue - vScrollbar.MinValue) / 5;
            }
            else
                this.vScrollbar.Visible = false;
        }

        protected override void OnResized(ResizeEventArgs resizeEventArgs)
        {
            base.OnResized(resizeEventArgs);

            this.hScrollbar.Position = new Vector2(0, this.Height - ScrollBar.DEFAULT_SCROLLBAR_SIZE);
            this.hScrollbar.Size = new Vector2(this.Width - ScrollBar.DEFAULT_SCROLLBAR_SIZE, ScrollBar.DEFAULT_SCROLLBAR_SIZE);

            this.vScrollbar.Position = new Vector2(this.Width - ScrollBar.DEFAULT_SCROLLBAR_SIZE, 0);
            this.vScrollbar.Size = new Vector2(ScrollBar.DEFAULT_SCROLLBAR_SIZE, this.Height - ScrollBar.DEFAULT_SCROLLBAR_SIZE);

            this.FixScrollbarMinMax();
        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);
            MouseMoveEventArgs _internalEvent0 = new MouseMoveEventArgs(new Vector2(_event.X - vScrollbar.X, _event.Y - vScrollbar.Y), _event.Delta);
            this.vScrollbar.OnMouseMoveEvent(_internalEvent0);


            MouseMoveEventArgs _internalEvent1 = new MouseMoveEventArgs(new Vector2(_event.X - hScrollbar.X, _event.Y - hScrollbar.Y), _event.Delta);
            this.hScrollbar.OnMouseMoveEvent(_internalEvent1);
        }


        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);
            MouseButtonEventArgs _internalEvent0 = new MouseButtonEventArgs(_event.Button, new OpenTK.Vector2(_event.X - vScrollbar.X, _event.Y - vScrollbar.Y), _event.Modifiers);
            this.vScrollbar.OnMouseUpEvent(_internalEvent0);


            MouseButtonEventArgs _internalEvent1 = new MouseButtonEventArgs(_event.Button, new OpenTK.Vector2(_event.X - hScrollbar.X, _event.Y - hScrollbar.Y), _event.Modifiers);
            this.hScrollbar.OnMouseUpEvent(_internalEvent1);
        }

        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);
            if (vScrollbar.Bounds.ContainsPoint(_event.Position))
            {
                MouseButtonEventArgs _internalEvent = new MouseButtonEventArgs(_event.Button, new Vector2(_event.X - vScrollbar.X, _event.Y - vScrollbar.Y), _event.Modifiers);
                this.vScrollbar.OnMouseDownEvent(_internalEvent);
            }

            if (hScrollbar.Bounds.ContainsPoint(_event.Position))
            {
                MouseButtonEventArgs _internalEvent = new MouseButtonEventArgs(_event.Button, new Vector2(_event.X - hScrollbar.X, _event.Y - hScrollbar.Y), _event.Modifiers);
                this.hScrollbar.OnMouseDownEvent(_internalEvent);
            }
        }

        protected internal override void OnMouseWheelChanged(MouseWheelEventArgs _event)
        {
            base.OnMouseWheelChanged(_event);
            if ((_event.Modifiers & ModifierKeys.Ctrl) == ModifierKeys.Ctrl)
                this.hScrollbar.Value -= _event.Delta * this.ScrollStep;
            else
                this.vScrollbar.Value -= _event.Delta * this.ScrollStep;
        }


        protected internal override sealed void Draw(ref Rect area, IGUIRenderer renderer)
        {
            this.DrawContent(ref area, renderer);

            if (this.vScrollbar.Visible)
            {
                Rect scrollArea = new Rect(area.X + this.vScrollbar.X, area.Y, this.vScrollbar.Width, this.vScrollbar.Height);
                if(renderer.SetSubRectDrawableArea(ref area, ref scrollArea, out scrollArea)) 
                    this.vScrollbar.Draw(ref scrollArea, renderer);
            }

            if (this.hScrollbar.Visible)
            {
                Rect scrollArea = new Rect(area.X, area.Y + this.hScrollbar.Y, this.hScrollbar.Width, this.hScrollbar.Height);

                if (renderer.SetSubRectDrawableArea(ref area, ref scrollArea, out scrollArea))
                    this.hScrollbar.Draw(ref scrollArea, renderer);

            }
        }

        protected internal abstract void DrawContent(ref Rect area, IGUIRenderer renderer);
    }
}