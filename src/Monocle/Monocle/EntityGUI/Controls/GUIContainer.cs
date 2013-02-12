using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Graphics;
using OpenTK.Input;
using Monocle.EntityGUI.Controls;

namespace Monocle.EntityGUI
{
    class GUIContainer : ScrollableControl, IEnumerable<GUIControl>
    {
        private readonly List<GUIControl> controls;
        private readonly IFocusManager focusManager;

        public GUIContainer()
            : this(new SimpleFocusManager())
        {
        }

        public GUIContainer(IFocusManager focusManager)
        {
            this.controls = new List<GUIControl>();
            this.focusManager = new SimpleFocusManager();
        }


        public void AddControl(GUIControl component)
        {
            this.controls.Add(component);
            this.OnControlAdded(component);
        }

        public void RemoveControl(GUIControl component)
        {
            var result = this.controls.Remove(component);
            this.OnControlRemoved(component);
        }

        protected internal override void DrawContent(ref Rect drawableArea, IGUIRenderer renderer)
        {
            Vector2 _offset = drawableArea.TopLeft + this.Bounds.TopLeft - this.ScrollOffset;
            foreach (var control in this.controls)
            {
                if (!control.Visible) continue;

                Rect bounds = control.Bounds;
                bounds.Displace(_offset);
                if (renderer.SetSubRectDrawableArea(ref drawableArea, ref bounds, out bounds))
                    control.Draw(ref bounds, renderer);
            }
        }

        protected internal override void Update(Utils.Time time)
        {
            base.Update(time);
            foreach (var item in this.controls)
            {
                if (!item.Visible) continue;
                item.Update(time);
            }
        }

        protected virtual void OnControlAdded(GUIControl control)
        {
            this.focusManager.ContainerItemAdded(control);
            FixContentArea();          
        }

        protected virtual void OnControlRemoved(GUIControl control)
        {
            this.focusManager.ContainerItemRemoved(control);
            FixContentArea();
        }

        private void FixContentArea()
        {
            float minHeight = this.controls.Min(x => x.Bounds.Top);
            float minWidth = this.controls.Min(x => x.Bounds.Left);

            float maxHeight = this.controls.Max((x) => x.Bounds.Bottom);
            float maxWidth = this.controls.Max(x => x.Bounds.Right);

            this.ContentArea = new Rect(minWidth, minHeight, maxWidth - minWidth, maxHeight - minHeight);         
        }
                
        protected internal override void OnMouseEnter(MouseMoveEventArgs _event)
        {
            base.OnMouseEnter(_event);  
            foreach (var control in this.controls)
            {
                if (!control.Visible) continue;

                control.OnMouseEnter(_event);
            }
        }

        protected internal override void OnMouseExit(MouseMoveEventArgs _event)
        {
            base.OnMouseExit(_event);  
            foreach (var control in this.controls)
            {
                if (!control.Visible) continue;

                control.OnMouseExit(_event);
            }

        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);  
            Vector2 position = _event.Position - Bounds.TopLeft + this.ScrollOffset;
            Vector2 oldPos = position - _event.Delta + this.ScrollOffset;
            foreach (var item in this)
            {
                if (!item.Visible) continue;

                MouseMoveEventArgs innerEvent = new MouseMoveEventArgs(position - item.Bounds.TopLeft, _event.Delta);
                var containsOldPoint = item.Bounds.ContainsPoint(oldPos);
                var containsNewPoint = item.Bounds.ContainsPoint(position);

                
                if (!containsNewPoint && containsOldPoint)
                {
                    item.OnMouseExit(innerEvent);
                }
                else if (containsNewPoint && !containsOldPoint)
                {
                    item.OnMouseEnter(innerEvent);
                }
                else if (containsNewPoint && containsOldPoint)
                {
                    item.OnMouseMoveEvent(innerEvent);
                }
                else if (item == focusManager.FocusedControl)
                {
                    item.OnMouseMoveEvent(innerEvent);
                }
            }
        }

        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);  
            Vector2 position = _event.Position - Bounds.TopLeft + this.ScrollOffset;
            foreach (var item in this)
            {
                if (!item.Visible) continue;

                if (item.Bounds.ContainsPoint(position))
                {
                    MouseButtonEventArgs innerEvent = new MouseButtonEventArgs(_event.Button, position - item.Bounds.TopLeft, _event.Modifiers);
                    if (_event.Button == MouseButton.Left || _event.Button == MouseButton.Right)
                    {
                        this.focusManager.GiveFocus(item);
                    }

                    item.OnMouseDownEvent(innerEvent);
                }
            }
        }

        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);  
            Vector2 position = _event.Position - this.Bounds.TopLeft + this.ScrollOffset;
            var focused = this.focusManager.FocusedControl;
            if (focused != null && focused.Visible)
            {
                if (focused.Bounds.ContainsPoint(position) && _event.Button == MouseButton.Left)
                {
                    focused.OnClicked();
                }

                MouseButtonEventArgs innerEvent = new MouseButtonEventArgs(_event.Button, position - focused.Bounds.TopLeft, _event.Modifiers);
                focused.OnMouseUpEvent(innerEvent);
            }
        }
        
        protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
        {
            base.OnKeyDownEvent(_event);
            var focused = this.focusManager.FocusedControl;
            if (focused != null && focused.Visible)
            {
                var consumed = focused.OnKeyDownEvent(_event);
                if (consumed)
                    return true;
            }

            return HandleFocusing(_event.Key, _event.Modifiers);
        }

        protected internal override void OnKeyUpEvent(KeyEventArgs _event)
        {
            base.OnKeyUpEvent(_event);
            var focused = this.focusManager.FocusedControl;
            if (focused != null && focused.Visible)
            {
                focused.OnKeyUpEvent(_event);
            }
        }

        protected internal override void OnCharEvent(CharEventArgs _event)
        {
            base.OnCharEvent(_event);
            var focused = this.focusManager.FocusedControl;
            if (focused != null && focused.Visible)
            {
                focused.OnCharEvent(_event);
            }
        }

        protected internal override void OnMouseWheelChanged(MouseWheelEventArgs _event)
        {
            base.OnMouseWheelChanged(_event);
            var focused = this.focusManager.FocusedControl;
            if (focused != null && focused.Visible)
            {
                focused.OnMouseWheelChanged(_event);
            }
        }

        #region Focus Management

        protected override void OnFocusGained(EventArgs eventArgs)
        {
            base.OnFocusGained(eventArgs);
            this.focusManager.ContainerFocusGained();
        }

        protected override void OnFocusLost(EventArgs eventArgs)
        {
            base.OnFocusLost(eventArgs);
            this.focusManager.ContainerFocusLost();
        }

        private bool HandleFocusing(Key key, ModifierKeys modifiers)
        {
            switch (key)
            {
                case OpenTK.Input.Key.Tab:
                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                        return this.focusManager.FocusPrev();
                    else
                        return this.focusManager.FocusNext();

                case OpenTK.Input.Key.Left:
                case OpenTK.Input.Key.Down:
                    return this.focusManager.FocusPrev();
                case OpenTK.Input.Key.Right:
                case OpenTK.Input.Key.Up:
                    return this.focusManager.FocusNext();
            }

            return false;
        }

        #endregion

        #region IEnumerable Region

        public IEnumerator<GUIControl> GetEnumerator()
        {
            return this.controls.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

    }
}
