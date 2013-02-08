using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Graphics;
using OpenTK;
using Monocle.Utils;

namespace Monocle.EntityGUI
{
    interface IGUIRenderer
    {
        void DrawRect(ref Rect rect, Color color);
        void DrawFrame(Frame frame, ref Rect rect, Color color);
        void DrawString(Font textureFont, string text, ref Rect bounds, Color color, TextAlignment textAlignment);
        void DrawMultiLineString(Font font, string text, ref Rect bounds, Color color, ref Vector2 offset);
        void DrawMarkedString(Font textureFont, TextEditor textEditor, ref Rect rect, Color color, Color selectionColor, TextAlignment textAlignment);
        void DrawMarkedMultiLineString(Font font, TextEditor textEditor, ref Rect rect, ref Vector2 offset, Color color, Color selectionColor);

        void Draw(GUIContainer container, ref Matrix4 projection);
        bool SetSubRectDrawableArea(ref Rect currentDrawable, ref Rect innerDrawable, out Rect subRect);
    }
}
