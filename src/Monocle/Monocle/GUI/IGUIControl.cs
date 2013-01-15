using System;
namespace Monocle.GUI
{
    public interface IGUIControl
    {
        OpenTK.Graphics.Color4 BGColor { get; set; }
        Monocle.Graphics.Rect Bounds { get; set; }
        event EventHandler<EventArgs> Clicked;
        Monocle.Graphics.Rect Dimention { get; }
        OpenTK.Graphics.Color4 FGColor { get; set; }
        bool Focused { get; set; }
        event EventHandler<EventArgs> FocusGained;
        event EventHandler<EventArgs> FocusLost;
        event EventHandler<OpenTK.KeyPressEventArgs> KeyPress;
        event EventHandler<OpenTK.Input.MouseButtonEventArgs> MouseDown;
        event EventHandler<OpenTK.Input.MouseEventArgs> MouseEnter;
        event EventHandler<OpenTK.Input.MouseEventArgs> MouseExit;
        event EventHandler<OpenTK.Input.MouseEventArgs> MouseStay;
        event EventHandler<OpenTK.Input.MouseButtonEventArgs> MouseUp;
        OpenTK.Vector2 Position { get; set; }
        IGUIRenderer Renderer { get; set; }
        int RenderOrder { get; set; }
    }
}
