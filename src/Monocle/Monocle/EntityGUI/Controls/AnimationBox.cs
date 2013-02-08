using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.EntityGUI
{
    class AnimationBox : FSMControl<AnimationBox>
    {
        private const string FRAME_CHANGED_ID = "FRAME_CHANGED";

        private Animation animation;

        public Animation Animation
        {
            get { return this.animation; }
            set
            {
                this.animation = (Animation)value.Clone();
            }
        }

        public Color AnimationColor
        {
            get;
            set;
        }

        public AnimationBox(Animation animation)
        {
            this.Animation = animation;
            this.Animation.Stop();
            this.AnimationColor = Color.White;
        }

        protected override GUIFSM<AnimationBox> CreateFSM()
        {
            var idle = new Idle();
            idle.AddTransition(GUIEventID.MouseEnter, 1);
            idle.AddTransition(GUIEventID.FocusGained, 2);

            var hover = new AnimUpdate();
            hover.AddTransition(GUIEventID.MouseExit, 0);
            hover.AddTransition(GUIEventID.FocusGained, 3);

            var focus = new AnimUpdate();
            focus.AddTransition(GUIEventID.FocusLost, 0);
            focus.AddTransition(GUIEventID.MouseEnter, 3);

            var focusH = new AnimUpdate();
            focusH.AddTransition(GUIEventID.FocusLost, 1);
            focusH.AddTransition(GUIEventID.MouseExit, 2);       

            return new GUIFSM<AnimationBox>(this, new GUIState<AnimationBox>[] { idle, hover, focus, focusH });
        }

        public event EventHandler<FrameChangedEventArgs> FrameChanged
        {
            add
            {
                this.AddEvent(FRAME_CHANGED_ID, value);
            }
            remove
            {
                this.RemoveEvent(FRAME_CHANGED_ID, value);
            }
        }

        protected virtual void OnFrameChanged(FrameChangedEventArgs args)
        {
            this.Invoke(FRAME_CHANGED_ID, args);
        }


        protected internal override void Update(Utils.Time time)
        {
            if (this.Animation != null)
            {
                var old = this.Animation.CurrentFrame;
                this.Animation.Update(time);

                if (old != this.Animation.CurrentFrame)
                {
                    this.OnFrameChanged(new FrameChangedEventArgs(old, this.Animation.CurrentFrame));
                }
            }
        }


        class Idle : GUIState<AnimationBox>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.BackgroundColor);
                if (Control.Animation != null)
                {
                    renderer.DrawFrame(Control.Animation.CurrentFrame, ref area, Control.AnimationColor);
                }
            }
            protected internal override void Enter()
            {
                if (Control.Animation != null)
                {
                    Control.Animation.Stop();
                    Control.Animation.Reset();
                }
            }
        }

        class AnimUpdate : Idle
        {
            protected internal override void Enter()
            {
                if (Control.Animation != null)
                {
                    Control.Animation.Start();
                }
            }
        }
    }
}