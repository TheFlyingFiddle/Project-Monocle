using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Graphics;

namespace Monocle.EntityGUI
{
    class CheckBox : FSMControl<CheckBox>
    {
        private const string CHECKED_ID = "CHECKED";
        private const string UNCHECKED_ID = "UNCHECKED";
        private bool _checked;

        public bool IsChecked
        {
            get { return this._checked; }
            set
            {
                this._checked = value;
                if (value)
                    this.OnChecked();
                else
                    this.OnUnChecked();
            }
        }

        public Color IconColor
        {
            get;
            set;
        }

        private readonly Frame checkedFrame;

        public CheckBox(Frame checkedFrame)
        {
            this.IconColor = Color.White;
            this.checkedFrame = checkedFrame;
        }


        protected override GUIFSM<CheckBox> CreateFSM()
        {
            var idle = new CheckBoxState();
            idle.AddTransition(GUIEventID.FocusGained, 1);
            var focused = new FocusedState();
            focused.AddTransition(GUIEventID.FocusLost, 0);

            return new GUIFSM<CheckBox>(this, new GUIState<CheckBox>[] { idle, focused });
        }

        protected internal void OnChecked()
        {
            this.Invoke(CHECKED_ID, EventArgs.Empty);
        }

        protected internal void OnUnChecked()
        {
            this.Invoke(UNCHECKED_ID, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Checked
        {
            add
            {
                this.AddEvent(CHECKED_ID, value);
            }
            remove
            {
                this.RemoveEvent(CHECKED_ID, value);
            }
        }

        public event EventHandler<EventArgs> UnChecked
        {
            add
            {
                this.AddEvent(UNCHECKED_ID, value);
            }
            remove
            {
                this.RemoveEvent(UNCHECKED_ID, value);
            }
        }

        class CheckBoxState : GUIState<CheckBox>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.BackgroundColor);

                if (Control.IsChecked)
                    renderer.DrawFrame(Control.checkedFrame, ref area, Control.BackgroundColor);
            }
        }

        class FocusedState : CheckBoxState
        {
            protected internal override void OnClicked()
            {
                Control.IsChecked = !Control.IsChecked;
                base.OnClicked();
            }

            protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
            {
                base.OnMouseDownEvent(_event);
            }
        }

    }
}
