using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Graphics;
using OpenTK;

namespace Monocle.EntityGUI
{
    class Button : FSMControl<Button>
    {
        public string Text
        {
            get;
            set;
        }

        public Color TextColor
        {
            get;
            set;
        }

        public Color IconColor
        {
            get;
            set;
        }

        public Font Font
        {
            get;
            set;
        }

        public Frame Icon
        {
            get;
            set;
        }

        public Button(Font font, string text)
            : this(font, null, text)
        {
        }

        public Button(Font font, Frame icon, string text)
        {
            this.Text = text;
            this.Font = font;
            this.Icon = icon;
            this.TextColor = Color.Black;
            this.IconColor = Color.White;
        }
                
        protected override GUIFSM<Button> CreateFSM()
        {
            var iState = new ButtonState();
            iState.AddTransition(GUIEventID.MouseEnter, 1);
            iState.AddTransition(GUIEventID.FocusGained, 2);

            var hState = new ButtonState();
            hState.AddTransition(GUIEventID.MouseExit, 0);
            hState.AddTransition(GUIEventID.FocusGained, 3);
            
            var fState = new FocusedState();
            fState.AddTransition(GUIEventID.FocusLost, 0);
            fState.AddTransition(GUIEventID.MouseEnter, 3);
            
            var fhState = new FocusedState();
            fhState.AddTransition(GUIEventID.FocusLost, 1);
            fhState.AddTransition(GUIEventID.MouseExit, 2);
            fhState.AddTransition(GUIEventID.LeftMouseDown, 4);

            var phState = new PressedState();
            phState.AddTransition(GUIEventID.LeftMouseUp, 3);
            phState.AddTransition(GUIEventID.MouseExit, 5);
            phState.AddTransition(GUIEventID.FocusLost, 1);

            var pState = new FocusedState();
            pState.AddTransition(GUIEventID.MouseEnter, 4);
            pState.AddTransition(GUIEventID.LeftMouseUp, 2);
            pState.AddTransition(GUIEventID.FocusLost, 0);

            return new GUIFSM<Button>(this, new GUIState<Button>[] { iState, hState, fState, fhState, phState, pState });
        }

        private void Draw(ref Rect area, IGUIRenderer renderer, Color c)
        {           

            renderer.DrawRect(ref area, c);

            Rect textRect = new Rect(area.X + this.padding.X, area.Y + area.H - this.Font.Size, area.W - this.padding.W - this.padding.X, this.Font.Size);
            renderer.DrawString(this.Font, this.Text, ref textRect, this.TextColor, TextAlignment.Left);

            if (this.Icon != null)
            {
                Rect rect = new Rect(area.X + (this.size.X / 4),
                                     area.Y + (this.size.Y / 4) ,
                                     this.size.X / 2,
                                     this.size.Y / 2);

                renderer.DrawFrame(this.Icon, ref rect, this.IconColor);
            }
        }

        #region States

        class ButtonState : GUIState<Button>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                this.Control.Draw(ref area, renderer, Control.BackgroundColor);
            }
        }

        class FocusedState : GUIState<Button>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                Color c = Color.AddContrast(Control.BackgroundColor, 0.2f);
                this.Control.Draw(ref area, renderer, c);
            }
        }

        class PressedState : GUIState<Button>
        {
            protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
            {
                Color c = new Color(Control.BackgroundColor, 1f);
                this.Control.Draw(ref area, renderer, c);
            }
        }


        #endregion
    }
}