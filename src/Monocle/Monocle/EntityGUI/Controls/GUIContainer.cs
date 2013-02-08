using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Graphics;
using OpenTK.Input;

namespace Monocle.EntityGUI
{
    class GUIContainer : GUIControl, IEnumerable<GUIControl>
    {
        private readonly List<GUIControl> controls;
        private int focusIndex = 0;

        private GUIControl FocusedComponent
        {
            get
            {
                return focusIndex < controls.Count ? controls[focusIndex] : null;
            }
        }

        private void SetFocus(int focusIndex)
        {
            if (focusIndex >= 0 && focusIndex < this.controls.Count)
            {
                this.RemoveFocus();

                this.focusIndex = focusIndex;
                this.controls[focusIndex].Focused = true;
            }
        }

        private void SetFocus(GUIControl item)
        {
            if (item.Focused)
                return;

            var index = this.controls.IndexOf(item);
            this.SetFocus(index);
        }


        private void RemoveFocus()
        {
            if (this.FocusedComponent != null && this.FocusedComponent.Focused)
                this.FocusedComponent.Focused = false;
        }

        public GUIContainer()
        {
            this.controls = new List<GUIControl>();
        }

        public void AddControl(GUIControl component)
        {
            this.controls.Add(component);
            this.OnControlAdded(component);
        }

        public void RemoveControl(GUIControl component)
        {
            var index = this.controls.IndexOf(component);
            if (index > focusIndex)
            {
                this.controls.RemoveAt(index);
                this.OnControlRemoved(component);
            }
            else if (index == focusIndex && index != 0)
            {
                this.SetFocus(focusIndex - 1);
                this.controls.RemoveAt(index);
                this.OnControlRemoved(component);
            }
            else if(index > 0)
            {
                this.focusIndex--;
                this.controls.RemoveAt(index);
                this.OnControlRemoved(component);
            }
        }

        protected internal override void Draw(ref Rect drawableArea, IGUIRenderer renderer)
        {
            Vector2 _offset = new Vector2(drawableArea.X, drawableArea.Y) + this.Bounds.TopLeft;
            foreach (var control in this.controls)
            {
                if (!control.Visible) continue;

                Rect bounds = control.Bounds;
                bounds.Displace(_offset);
                if (renderer.SetSubRectDrawableArea(ref drawableArea, ref bounds, out bounds))
                {
                    control.Draw(ref bounds, renderer);
                }
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

        protected virtual void OnControlAdded(GUIControl component)
        {

        }

        protected virtual void OnControlRemoved(GUIControl component)
        {

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
            Vector2 position = _event.Position - Bounds.TopLeft;
            Vector2 oldPos = position - _event.Delta;
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
                else if (item == this.FocusedComponent)
                {
                    item.OnMouseMoveEvent(innerEvent);
                }
            }
        }

        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);  
            Vector2 position = _event.Position - Bounds.TopLeft;
            foreach (var item in this)
            {
                if (!item.Visible) continue;

                if (item.Bounds.ContainsPoint(position))
                {
                    MouseButtonEventArgs innerEvent = new MouseButtonEventArgs(_event.Button, position - item.Bounds.TopLeft);
                    if (_event.Button == MouseButton.Left || _event.Button == MouseButton.Right)
                    {
                        this.SetFocus(item);
                    }

                    item.OnMouseDownEvent(innerEvent);
                }
            }
        }

        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);  
            Vector2 position = _event.Position - this.Bounds.TopLeft;
            if(this.FocusedComponent != null && this.FocusedComponent.Visible)
            {
                if (this.FocusedComponent.Bounds.ContainsPoint(position) && _event.Button == MouseButton.Left)
                {
                    this.FocusedComponent.OnClicked();
                }

                MouseButtonEventArgs innerEvent = new MouseButtonEventArgs(_event.Button, position - this.FocusedComponent.Bounds.TopLeft);
                this.FocusedComponent.OnMouseUpEvent(innerEvent);
            }
        }

        protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
        {
            base.OnKeyDownEvent(_event);  
            var focusedComp = this.FocusedComponent;
            if (focusedComp != null && this.FocusedComponent.Visible)
            {
                var consumed = focusedComp.OnKeyDownEvent(_event);
                if (consumed)
                    return true;
                else 
                {
                    return HandleFocusing(_event.Key, _event.Modifiers);
                }
            }
            
            return false;
        }

        protected internal override void OnKeyUpEvent(KeyEventArgs _event)
        {
            base.OnKeyUpEvent(_event);  
            var focusedComp = this.FocusedComponent;
            if (focusedComp != null && this.FocusedComponent.Visible)
            {
               focusedComp.OnKeyUpEvent(_event);
            }
        }

        protected internal override void OnCharEvent(CharEventArgs _event)
        {
            base.OnCharEvent(_event);  
            var focusedComp = this.FocusedComponent;
            if (focusedComp != null && this.FocusedComponent.Visible)
            {
                focusedComp.OnCharEvent(_event);
            }
        }

        protected internal override void OnMouseWheelChanged(MouseWheelEventArgs _event)
        {
            base.OnMouseWheelChanged(_event);  
            var focusedComp = this.FocusedComponent;
            if (focusedComp != null && this.FocusedComponent.Visible)
            {
                focusedComp.OnMouseWheelChanged(_event);
            }
        }

        #region Focus Management

        protected override void OnFocusGained(EventArgs eventArgs)
        {
            base.OnFocusGained(eventArgs);
            this.SetFocus(0);
        }

        protected override void OnFocusLost(EventArgs eventArgs)
        {
            base.OnFocusLost(eventArgs);
            this.RemoveFocus();
        }

        private bool HandleFocusing(Key key, ModifierKeys modifiers)
        {
            switch (key)
            {
                case OpenTK.Input.Key.Tab:
                    Console.WriteLine(modifiers);

                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                        return FocusPrev();
                    else
                        return FocusNext();

                case OpenTK.Input.Key.Left:
                case OpenTK.Input.Key.Down:
                    return FocusPrev();

                case OpenTK.Input.Key.Right:
                case OpenTK.Input.Key.Up:
                    return FocusNext();
            }

            return false;
        }

        private bool FocusPrev()
        {
            if (this.focusIndex > 0)
            {
                this.SetFocus(focusIndex - 1);
                return true;
            }

            this.Focused = false;
            return false;
        }

        private bool FocusNext()
        {
            if (this.focusIndex + 1 < this.controls.Count)
            {
                this.SetFocus(focusIndex + 1);
                return true;
            }

            this.Focused = false;
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
