using System;
using OpenTK;
using Monocle.Graphics;
namespace Monocle.EntityGUI
{
    interface IGUIControl
    {
        Rect Bounds { get; set; }
        void Draw(ref Vector2 offset, IGUIRenderer renderer);
        bool Focused { get; set;  }
        Vector2 Position { get; set; }


        bool OnKeyDownEvent(KeyEventArgs _event);
        void OnMouseMoveEvent(MouseMoveEventArgs _event);
        void OnMouseDownEvent(MouseButtonEventArgs _event);
        void OnMouseEnterEvent(MouseMoveEventArgs _event);
        void OnMouseExitEvent(MouseMoveEventArgs _event);
        void OnMouseUpEvent(MouseButtonEventArgs _event);
        void OnKeyUpEvent(KeyEventArgs _event);
        void OnCharEvent(CharEventArgs _event);
    }
}
