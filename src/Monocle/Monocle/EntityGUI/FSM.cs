using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Graphics;
using Monocle.Utils;

namespace Monocle.EntityGUI
{
    class GUIFSM<T>
    {
        public readonly T Control;
        private readonly GUIState<T>[] states;
        private int activeIndex;

        private GUIState<T> ActiveState
        {
            get { return this.states[activeIndex]; }
        }

        public GUIFSM(T control, GUIState<T>[] states)
        {
            this.Control = control;
            this.states = states;
            foreach (var state in states)
            {
                state.Control = control;
            }

            this.ActiveState.Enter();
        }

        private void Transition(GUIEventID _id)
        {
            var index = ActiveState[_id];
            if (index != -1)
            {
                this.activeIndex = index;
                this.ActiveState.Enter();
            }
        }

        internal void Draw(ref Rect drawableArea, IGUIRenderer batch)
        {
            ActiveState.Draw(ref drawableArea, batch);
        }

        internal void Update(Time time)
        {
            ActiveState.Update(time);
        }

        internal void OnFocusGained()
        {
            ActiveState.OnFocusGained();
            Transition(GUIEventID.FocusGained);
        }

        internal void OnFocusLost()
        {
            ActiveState.OnFocusLost();
            Transition(GUIEventID.FocusLost);
        }

        internal bool OnKeyDownEvent(KeyEventArgs _event)
        {
            var result = ActiveState.OnKeyDownEvent(_event);
            Transition(GUIEventID.KeyDown);
            return result;
        }

        internal void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            ActiveState.OnMouseMoveEvent(_event);
            Transition(GUIEventID.MouseMove);
        }

        internal void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            ActiveState.OnMouseDownEvent(_event);
            if (_event.Button == MouseButton.Left)
            {
                Transition(GUIEventID.LeftMouseDown);
            }
            else
            {
                Transition(GUIEventID.MouseDown);
            }
        }

        internal void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            ActiveState.OnMouseUpEvent(_event);
            if (_event.Button == MouseButton.Left)
            {
                Transition(GUIEventID.LeftMouseUp);
            }
            else
            {
                Transition(GUIEventID.MouseUp);
            }

        }

        internal void OnKeyUpEvent(KeyEventArgs _event)
        {
            ActiveState.OnKeyUpEvent(_event);
            Transition(GUIEventID.KeyUp);
        }

        internal void OnCharEvent(CharEventArgs _event)
        {
            ActiveState.OnCharEvent(_event);
            Transition(GUIEventID.Char);
        }

        internal void OnMouseEnter(MouseMoveEventArgs _event)
        {
            ActiveState.OnMouseEnter(_event);
            Transition(GUIEventID.MouseEnter);
        }

        internal void OnMouseExit(MouseMoveEventArgs _event)
        {
            ActiveState.OnMouseExit(_event);
            Transition(GUIEventID.MouseExit);
        }

        internal void OnMouseWheelChanged(MouseWheelEventArgs _event)
        {
            ActiveState.OnMouseWheelChanged(_event);
            Transition(GUIEventID.MouseExit);
        }

        internal void OnClicked()
        {
            ActiveState.OnClicked();
            Transition(GUIEventID.Clicked);
        }
    }

    abstract class GUIState<T>
    {
        private readonly Dictionary<GUIEventID, int> transitions;

        internal protected T Control
        {
            get;
            internal set;
        }

        public GUIState()
        {
            this.transitions = new Dictionary<GUIEventID, int>();
        }

        public void AddTransition(GUIEventID id, int _event)
        {
            this.transitions.Add(id, _event);
        }

        internal int this[GUIEventID id]
        {
            get
            {
                int _event;
                if (this.transitions.TryGetValue(id, out _event))
                    return _event;
                return -1;
            }
        }

        protected internal abstract void Draw(ref Rect area, IGUIRenderer batch);
        protected internal virtual void Update(Time time) { }
        protected internal virtual bool OnKeyDownEvent(KeyEventArgs _event) { return false; }
        protected internal virtual void OnMouseMoveEvent(MouseMoveEventArgs _event) { }
        protected internal virtual void OnMouseDownEvent(MouseButtonEventArgs _event) { }
        protected internal virtual void OnMouseUpEvent(MouseButtonEventArgs _event) { }
        protected internal virtual void OnMouseWheelChanged(MouseWheelEventArgs _event) { }
        protected internal virtual void OnKeyUpEvent(KeyEventArgs _event) { }
        protected internal virtual void OnCharEvent(CharEventArgs _event) { }
        protected internal virtual void OnMouseEnter(MouseMoveEventArgs _event) { }
        protected internal virtual void OnMouseExit(MouseMoveEventArgs _event) { }
        protected internal virtual void OnFocusGained() { }
        protected internal virtual void OnFocusLost() { }
        protected internal virtual void OnClicked() { }
        protected internal virtual void Enter() { }
    }
}
