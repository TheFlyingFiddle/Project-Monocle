using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.EntityGUI
{
    abstract class ScrollBase : FSMControl<ScrollBase>
    {
        private const string VALUE_CHANGED_ID = "VALUE_CHANGED";
        private int DEFAULT_NUM_STEPS = 10;

        private float value, minValue, maxValue;
        public ScrollBase(Orientation orientation, int min, int max, int value_2)
        {
            this.Orientation = orientation;
            this.MinValue = min;
            this.MaxValue = max;
            this.Value = value;
            this.Step = (MaxValue - MinValue) / DEFAULT_NUM_STEPS;
        }

        public float Value
        {
            get { return this.value; }
            set
            {
                float tmp = this.value;

                if (value > MaxValue)
                    this.value = MaxValue;
                else if (value < MinValue)
                    this.value = MinValue;
                else
                    this.value = value;

                if (tmp != this.value)
                    this.OnValueChanged(new FloatChangedEventArgs(tmp, this.value));
            }
        }

        public Orientation Orientation
        {
            get;
            private set;
        }

        public float Step
        {
            get;
            set;
        }

        public float MinValue
        {
            get { return this.minValue; }
            set
            {
                this.minValue = value;
                if (this.minValue > this.value)
                    this.Value = this.minValue;
            }
        }

        public float MaxValue
        {
            get { return this.maxValue; }
            set
            {
                this.maxValue = value;
                if (this.value > this.maxValue)
                    this.Value = this.maxValue;
            }
        }

        public Color ButtonColor
        {
            get;
            set;
        }

        protected virtual void OnValueChanged(FloatChangedEventArgs args)
        {
            this.Invoke(VALUE_CHANGED_ID, args);
        }


        public event EventHandler<FloatChangedEventArgs> ValueChanged
        {
            add
            {
                this.AddEvent(VALUE_CHANGED_ID, value);
            }
            remove
            {
                this.RemoveEvent(VALUE_CHANGED_ID, value);
            }
        }

        protected override GUIFSM<ScrollBase> CreateFSM()
        {
            var idle = new SliderState();
            idle.AddTransition(GUIEventID.FocusGained, 1);
            var focused = new FocusedState();
            focused.AddTransition(GUIEventID.FocusLost, 0);
            focused.AddTransition(GUIEventID.LeftMouseDown, 2);
            var mouseMove = new MouseMoveState();
            mouseMove.AddTransition(GUIEventID.LeftMouseUp, 1);
            mouseMove.AddTransition(GUIEventID.FocusLost, 0);

            return new GUIFSM<ScrollBase>(this, new GUIState<ScrollBase>[] { idle, focused, mouseMove });
        }


        class SliderState : GUIState<ScrollBase>
        {
            protected internal override void Draw(ref Rect drawableArea, IGUIRenderer renderer)
            {  }
        }

        class FocusedState : SliderState
        {
            protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
            {
                base.OnMouseDownEvent(_event);
                float value;
                if (Control.Orientation == Orientation.Horizontal)
                    value = _event.X / (Control.Width - Control.Height / 2);
                else
                    value = _event.Y / (Control.Height - Control.Width / 2);

                Control.Value = Control.MinValue + (Control.MaxValue - Control.MinValue) * value;
            }

            protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
            {
                if (Control.Orientation == Orientation.Horizontal)
                {
                    if (_event.Key == OpenTK.Input.Key.Left)
                    {
                        Control.Value -= Control.Step;
                        return true;
                    }
                    else if (_event.Key == OpenTK.Input.Key.Right)
                    {
                        Control.Value += Control.Step;
                        return true;
                    }
                }
                else
                {
                    if (_event.Key == OpenTK.Input.Key.Up)
                    {
                        Control.Value -= Control.Step;
                        return true;
                    }
                    else if (_event.Key == OpenTK.Input.Key.Down)
                    {
                        Control.Value += Control.Step;
                        return true;
                    }
                }

                return base.OnKeyDownEvent(_event);
            }
        }

        class MouseMoveState : SliderState
        {
            protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
            {
                base.OnMouseMoveEvent(_event);
                float value;
                if (Control.Orientation == Orientation.Horizontal)
                    value = _event.X / (Control.Width - Control.Height / 2);
                else
                    value = _event.Y / (Control.Height - Control.Width / 2);
                Control.Value = Control.MinValue + (Control.MaxValue - Control.MinValue) * value;
            }
        }
    }
}
