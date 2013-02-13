using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.EntityGUI.Controls
{
    class ComboBox<T> : FSMControl<ComboBox<T>>
    {
        private readonly List<T> items;


        protected override GUIFSM<ComboBox<T>> CreateFSM()
        {
            var idle = new Idle();
            idle.AddTransition(GUIEventID.FocusGained, 1);
           
            var focused = new Focus();
            focused.AddTransition(GUIEventID.FocusLost, 0);
            
            return new GUIFSM<ComboBox<T>>(this, new GUIState<ComboBox<T>>[] { idle, focused });
        }


        class Idle : GUIState<ComboBox<T>>
        {
            protected internal override void Draw(ref Graphics.Rect area, IGUIRenderer batch)
            {
                throw new NotImplementedException();
            }
        }

        class Focus : GUIState<ComboBox<T>>
        {
            protected internal override void Draw(ref Graphics.Rect area, IGUIRenderer batch)
            {
                throw new NotImplementedException();
            }
        }

        class Pressed : GUIState<ComboBox<T>>
        {
            protected internal override void Draw(ref Graphics.Rect area, IGUIRenderer batch)
            {
                throw new NotImplementedException();
            }
        }

    }
}
