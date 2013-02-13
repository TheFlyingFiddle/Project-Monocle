using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Graphics;
using OpenTK.Input;
using OpenTK;

namespace Monocle.EntityGUI.Controls
{
    class TextArea : ScrollableFSMControl<TextArea>
    {
        private const string TEXT_CHANGED_ID = "TEXT_CHANGED";
        protected readonly TextEditor editText;
                
        public string Text
        {
            get { return this.editText.ToString(); }
            set 
            { 
                var old = this.editText.ToString();
                this.editText.SetText(value);
                this.OnTextChanged(new TextChangedEventArgs(old, this.editText.ToString()));
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

        public Font Font
        {
            get;
            set;
        }

        public TextArea(Font font, string text = "", int maxLenght = -1)
        {
            this.Font = font;
            this.editText = new TextEditor(null, "", -1, true);
            this.TextColor = Color.Black;
            this.SelectedColor = Color.Blue;
            this.padding = new Rect(4, 4, 4, 4);
            this.ScrollStep = Font.Size;
         }

        protected override GUIFSM<TextArea> CreateFSM()
        {
            var idle = new Idle();
            idle.AddTransition(GUIEventID.FocusGained, 1);

            var focused = new FocusedState();
            focused.AddTransition(GUIEventID.FocusLost, 0);

            return new GUIFSM<TextArea>(this, new GUIState<TextArea>[] { idle, focused });
        }


        internal protected virtual void OnTextChanged(TextChangedEventArgs args)
        {
            this.Invoke(TEXT_CHANGED_ID, args);

            Vector2 contentSize = this.Font.MessureString(this.editText.ToString());


            this.ContentArea = new Rect(0, 0, contentSize.X, contentSize.Y);            
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
            this.selecting = true;

            int lineIndex = (int)((_event.Y - this.padding.Y + this.ScrollOffset.Y) / this.Font.LineHeight);
            if (lineIndex == 0)
            {
                int index = this.Font.BestWidthFit(this.editText.ToString(), _event.Position.X + this.ScrollOffset.X);
                this.editText.MarkerIndex = index;
                this.editText.SelectionIndex = index;
                return;
            }

            int indexOfLine = LineIndex(lineIndex);

            if (indexOfLine == this.editText.Length - 1)
            {
                this.editText.MarkerIndex = this.editText.Length;
                this.editText.SelectionIndex = this.editText.Length;
            }
            else
            {
                int index = this.Font.BestSubstringWidthFit(this.editText.ToString(), indexOfLine, 
                            this.editText.Length - indexOfLine, _event.Position.X + this.ScrollOffset.X);
                this.editText.MarkerIndex = index;
                this.editText.SelectionIndex = index;
            }
        }


        protected internal override void OnMouseUpEvent(MouseButtonEventArgs _event)
        {
            base.OnMouseUpEvent(_event);
            this.selecting = false;
        }

        protected internal override void OnMouseMoveEvent(MouseMoveEventArgs _event)
        {
            base.OnMouseMoveEvent(_event);
            if (this.selecting)
            {
                int lineIndex = (int)((_event.Y + this.ScrollOffset.Y - this.padding.Y) / this.Font.LineHeight);
                if (lineIndex == 0)
                {
                    int index = this.Font.BestWidthFit(this.editText.ToString(), _event.Position.X + this.ScrollOffset.X);
                    this.editText.MarkerIndex = index;
                    return;
                }

                int indexOfLine = LineIndex(lineIndex);

                if (indexOfLine == this.editText.Length - 1)
                {
                    this.editText.MarkerIndex = this.editText.Length;
                }
                else
                {
                    int index = this.Font.BestSubstringWidthFit(this.editText.ToString(), indexOfLine, 
                                this.editText.Length - indexOfLine, _event.Position.X + this.ScrollOffset.X);
                    this.editText.MarkerIndex = index;
                }
            }
        }

        private int LineIndex(int lineIndex)
        {
            var txt = this.editText.ToString();
            int x = 0;
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '\n') 
                {
                    x++; if (lineIndex == x) return Math.Min(i + 1, this.editText.Length);
                }
            }

            return txt.Length - 1;
        }


        class Idle : GUIState<TextArea>
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
                        Vector2 offset = new Vector2(Control.HScrollBar.Value, Control.VScrollBar.Value);
                        renderer.DrawMultiLineString(Control.Font, Control.Text, ref _contentRect, Control.TextColor, ref offset);
                    }
                }
            }
        }

        class FocusedState : GUIState<TextArea>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                renderer.DrawRect(ref area, Control.BackgroundColor);

                Rect _contentRect = Control.ContentArea;
                _contentRect.Displace(area.TopLeft);
                if (renderer.SetSubRectDrawableArea(ref area, ref _contentRect, out _contentRect))
                {

                    Vector2 offset = new Vector2(Control.HScrollBar.Value, Control.VScrollBar.Value);
                    renderer.DrawMarkedMultiLineString(Control.Font, Control.editText,
                                           ref _contentRect, ref offset, Control.TextColor, 
                                           Control.SelectedColor);
                }
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

            protected internal override bool OnKeyDownEvent(KeyEventArgs _event)
            {
                var oldText = Control.Text;
                if (Control.editText.ProcessKey(_event.Key, _event.Modifiers))
                {
                    Control.OnTextChanged(new TextChangedEventArgs(oldText, Control.Text));
                }

                switch (_event.Key)
                {
                    case Key.Left:
                    case Key.Right:
                    case Key.Up:
                    case Key.Down:
                        return true;
                }

                return base.OnKeyDownEvent(_event);
            }
        }
    }
}