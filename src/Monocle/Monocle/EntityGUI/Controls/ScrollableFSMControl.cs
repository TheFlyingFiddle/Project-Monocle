using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.EntityGUI.Controls
{
    abstract class ScrollableFSMControl<T> : ScrollableControl
    {
        private readonly GUIFSM<T> fsm;

        public ScrollableFSMControl()
        {
            fsm = this.CreateFSM();
        }

        protected override void OnFocusGained(EventArgs eventArgs)
        {
            base.OnFocusGained(eventArgs);
            fsm.OnFocusGained();
        }

        protected override void OnFocusLost(EventArgs eventArgs)
        {
            base.OnFocusLost(eventArgs);
            fsm.OnFocusLost();
        }

        protected abstract GUIFSM<T> CreateFSM();

        protected internal override void DrawContent(ref Rect drawableArea, IGUIRenderer renderer)
        {
            fsm.Draw(ref drawableArea, renderer);
        }

        protected internal override void Update(Utils.Time time)
        {
            fsm.Update(time);
        }

        protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
        {
            base.OnKeyDownEvent(_event);
            return fsm.OnKeyDownEvent(_event);
        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);
            fsm.OnMouseMoveEvent(_event);
        }

        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);
            fsm.OnMouseDownEvent(_event);
        }

        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);
            fsm.OnMouseUpEvent(_event);
        }

        protected internal override void OnKeyUpEvent(KeyEventArgs _event)
        {
            base.OnKeyUpEvent(_event);
            fsm.OnKeyUpEvent(_event);
        }

        protected internal override void OnCharEvent(CharEventArgs _event)
        {
            base.OnCharEvent(_event);
            fsm.OnCharEvent(_event);
        }

        protected internal override void OnMouseEnter(MouseMoveEventArgs _event)
        {
            base.OnMouseEnter(_event);
            fsm.OnMouseEnter(_event);
        }

        protected internal override void OnMouseExit(MouseMoveEventArgs _event)
        {
            base.OnMouseExit(_event);
            fsm.OnMouseExit(_event);
        }

        protected internal override void OnMouseWheelChanged(MouseWheelEventArgs _event)
        {
            base.OnMouseWheelChanged(_event);
            fsm.OnMouseWheelChanged(_event);
        }

        protected internal override void OnClicked()
        {
            base.OnClicked();
            fsm.OnClicked();    
        }
    }
}