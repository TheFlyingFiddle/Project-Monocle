using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Graphics;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK;

namespace Monocle.EntityGUI
{
    class TextBox : FSMControl<TextBox>
    {
        private const string TEXT_CHANGED_ID = "TEXT_CHANGED";
        protected readonly TextEditor editText;

        public string Text
        {
            get { return editText.ToString(); }
        }

        public string Hint
        {
            get;
            set;
        }

        public override OpenTK.Vector2 Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                Vector2 tmp = value;
                var properHeight = this.Padding.Y + this.Padding.H + this.Font.Size;
                if (tmp.Y != properHeight)
                {
                    tmp.Y = properHeight;
                }

                base.Size = tmp;
            }
        }

        public Color TextColor
        {
            get;
            set;
        }

        public Color SelectedColor
        {
            get;
            set;
        }

        public Color HintColor
        {
            get;
            set;
        }

        public int MaxLength
        {
            get { return this.editText.MaxSize; }
            set { this.editText.MaxSize = value; }
        }

        internal Font Font
        {
            get;
            private set;
        }

        public TextBox(Font font, string text, int maxLength = -1)
        {
            this.Font = font;
            this.editText = new TextEditor(CharacterFilter, text, maxLength);
            this.TextColor = Color.Black;
            this.HintColor = Color.LightGray;
            this.SelectedColor = Color.Blue;
            this.padding = new Rect(4, 4, 4, 4);
        }
                
        protected override GUIFSM<TextBox> CreateFSM()
        {
            var idle = new IdleTextBoxState();
            idle.AddTransition(GUIEventID.FocusGained, 1);

            var focused = new FocusedTextBoxState();
            focused.AddTransition(GUIEventID.FocusLost, 0);
            

            return new GUIFSM<TextBox>(this, new GUIState<TextBox>[] { idle, focused });
        }

        protected virtual bool CharacterFilter(char c) { return true; }

        internal protected virtual void OnTextChanged(TextChangedEventArgs args)
        {
            this.Invoke(TEXT_CHANGED_ID, args);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged
        {
            add
            {
                this.AddEvent(TEXT_CHANGED_ID, value);
            }
            remove
            {
                this.RemoveEvent(TEXT_CHANGED_ID, value);
            }
        }

        private bool selecting = false;
        protected internal override void OnMouseDownEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseDownEvent(_event);
            int index = this.Font.BestWidthFit(this.editText.ToString(), _event.Position.X - this.padding.X) + 1;
            this.editText.MarkerIndex = index;
            this.editText.SelectionIndex = index;

            this.selecting = true;
        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);
            if (this.selecting)
            {
                int index = this.Font.BestWidthFit(this.editText.ToString(), _event.Position.X - this.padding.X) + 1;
                this.editText.MarkerIndex = index;
            }
        }

        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);
            this.selecting = false;
        }

        #region States

        class IdleTextBoxState : GUIState<TextBox>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.BackgroundColor);

                Rect _contentRect = new Rect(area.X + Control.padding.X, area.Y + Control.padding.Y,
                                area.W - Control.padding.X - Control.padding.W,
                                area.H - Control.padding.H);

                if (renderer.SetSubRectDrawableArea(ref area, ref _contentRect, out _contentRect))
                {
                    if (Control.editText.Length > 0)
                    {
                        renderer.DrawString(Control.Font, Control.Text, ref _contentRect, Control.TextColor, TextAlignment.Left);
                    }
                    else
                    {
                        renderer.DrawString(Control.Font, Control.Hint, ref _contentRect, Control.HintColor, TextAlignment.Left);
                    }
                }
            }

            protected internal override void OnFocusGained()
            {
                base.OnFocusGained();
                // Control.editText.SelectAll();
            }
        }

        class FocusedTextBoxState : GUIState<TextBox>
        {
            protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
            {
                var oldText = Control.Text;
                if (Control.editText.ProcessKey(_event.Key, _event.Modifiers))
                {
                    Control.OnTextChanged(new TextChangedEventArgs(oldText, Control.Text));
                }

                switch (_event.Key)
                {
                    case Key.Left :
                    case Key.Right :
                        return true;
                }

                return base.OnKeyDownEvent(_event);
            }


            protected internal override void OnCharEvent(CharEventArgs _event)
            {
                base.OnCharEvent(_event);
                var oldText = Control.Text;
                if (Control.editText.ProcessChar(_event.Character))
                {
                    Control.OnTextChanged(new TextChangedEventArgs(oldText, Control.Text));
                }
            }

            protected internal override void Draw(ref Rect area, IGUIRenderer batch)
            {
                batch.DrawRect(ref area, Control.BackgroundColor);
                Rect _contentRect = new Rect(area.X + Control.padding.X, area.Y + Control.padding.Y,
                                area.W - Control.padding.X - Control.padding.W,
                                area.H - Control.padding.H - Control.padding.Y);

                if (batch.SetSubRectDrawableArea(ref area, ref _contentRect, out _contentRect))
                {
                    batch.DrawMarkedString(Control.Font, Control.editText, ref _contentRect, Control.TextColor, Control.SelectedColor, TextAlignment.Left);
                }
            }
        }

        #endregion
    }
}