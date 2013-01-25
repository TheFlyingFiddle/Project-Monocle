using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Core;
using Monocle.Graphics;
using OpenTK;
using OpenTK.Graphics;

namespace Monocle.EntityGUI
{
    class GFSMComponent : Behaviour
    {
        private readonly GFSMState[] States;
        private int activeIndex;

        public GFSMComponent(GFSMState[] states)
        {
            this.activeIndex = 0;
            this.States = states;
            foreach (var item in states)
            {
                foreach (var action in item.Actions)
                {
                    action.FSM = this;
                }
            }
        }

        public override void Start()
        {
            var activeState = States[this.activeIndex];
            activeState.Enter();
        }

        public void SendEvent(int _event)
        {
            var activeState = States[this.activeIndex];
            var nextIndex = activeState[_event];
            if (nextIndex == -1)
                return;

            this.activeIndex = nextIndex;
            var nextActiveState = States[this.activeIndex];

            activeState.Exit();
            nextActiveState.Enter();
        }

        public void Draw(Vector2 offset, ISpriteBatch batch)
        {
            var activeState = States[activeIndex];
            activeState.Draw(offset, batch);
        }

        protected override Component Clone()
        {
            var copy = new GFSMState[this.States.Length];
            for (int i = 0; i < this.States.Length; i++)
            {
                copy[i] = this.States[i].Clone();
            }

            return new GFSMComponent(copy);
        }
    }

    class GFSMState
    {
        internal readonly GFSMAction[] Actions;
        private readonly int[] Transitions;

        public GFSMState(int[] transitions, GFSMAction[] actions)
        {
            this.Transitions = transitions;
            this.Actions = actions;        
        }

        internal int this[int _event]
        {
            get
            {
                if (_event < 0 || _event > this.Transitions.Length)
                {
                    return -1;
                }

                return this.Transitions[_event];
            }
        }

        internal void Enter()
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].Enter();
            }
        }

        internal void Exit()
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].Exit();
            }
        }

        internal void Draw(Vector2 offset, ISpriteBatch batch)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].Draw(offset, batch);
            }
        }

        internal GFSMState Clone()
        {
            var copy = new GFSMAction[this.Actions.Length];
            for (int i = 0; i < this.Actions.Length; i++)
            {
                copy[i] = this.Actions[i].Clone();
            }

            return new GFSMState(this.Transitions, copy);
        }
    }

    abstract class GFSMAction
    {
        protected internal GFSMComponent FSM;

        internal protected virtual void Enter() {  }
        internal protected virtual void Exit() {  }
        internal protected virtual void Draw(Vector2 offset, ISpriteBatch batch) {  }
        internal protected abstract GFSMAction Clone();
    }


    sealed class GFSMFrameAction : GFSMAction
    {
        private readonly string Frame_ID;
        private readonly string Bounds_ID;

        public GFSMFrameAction(string frame_ID, string Bounds_ID)
        {
            this.Frame_ID = frame_ID;
            this.Bounds_ID = Bounds_ID;
        }

        protected internal override void Draw(Vector2 offset, ISpriteBatch batch)
        {
            var bounds = this.FSM.Owner.GetVar<Rect>(Bounds_ID).Value; // Maby hash this?
            var frame = this.FSM.Owner.GetVar<Frame>(Frame_ID).Value;

            batch.AddFrame(frame,
                           new Rect(offset.X + bounds.X,
                                    offset.Y + bounds.Y,
                                    bounds.Width,
                                    bounds.Height),
                            Color4.White);                                         
        }

        protected internal override GFSMAction Clone()
        {
            return new GFSMFrameAction(Frame_ID, Bounds_ID);
        }
    }

    sealed class GFSMTextAction : GFSMAction
    {
        private readonly string Font_ID;
        private readonly string Bounds_ID;
        private readonly string Text_ID;

        public GFSMTextAction(string font_ID, string bounds_ID, string text_ID)
        {
            this.Font_ID = font_ID;
            this.Bounds_ID = bounds_ID;
            this.Text_ID = text_ID;
        }

        protected internal override void Draw(Vector2 offset, ISpriteBatch batch)
        {
            var bounds = this.FSM.Owner.GetVar<Rect>(Bounds_ID).Value;
            var text = this.FSM.Owner.GetVar<string>(Text_ID).Value;
            var font = this.FSM.Owner.GetVar<TextureFont>(Font_ID).Value;

            batch.AddString(font,
                            text,
                            new Vector2(offset.X + bounds.X, offset.Y + bounds.Y),
                            Color4.White,
                            new Vector2(bounds.Width / 2 - font.MessureString(text).X / 2, bounds.Height / 2 - font.MessureString(text).Y / 2));
        }

        protected internal override GFSMAction Clone()
        {
            return new GFSMTextAction(this.Font_ID, this.Bounds_ID, this.Text_ID);
        }
    }
}