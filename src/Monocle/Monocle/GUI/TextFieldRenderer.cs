using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    class TextFieldRenderer : GUIRenderer<TextField>
    {
        public TextFieldRenderer(GUISkin skin) : base(skin, "TextFied") { }

        public override void Render(IGUIRenderingContext context, Utils.Time time, TextField control, LookAndFeel lookAndFeel)
        {
            throw new NotImplementedException();
        }
    }
}
