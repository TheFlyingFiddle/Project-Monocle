using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.Utils;

namespace Monocle.GUI
{
    public interface IGUIRenderer
    {
        void Render(IGUIRenderingContext context, Time time, IGUIControl control, LookAndFeel lookAndFeel); 
    }

    public interface IGUIRenderer<T> : IGUIRenderer  where T : IGUIControl
    {
        void Render(IGUIRenderingContext context, Time time, T control, LookAndFeel lookAndFeel);
    }

    public abstract class GUIRenderer<T> : IGUIRenderer<T> where T : IGUIControl
    {
        public void Render(IGUIRenderingContext context, Time time, IGUIControl control, LookAndFeel lookAndFeel)
        {
            this.Render(context, time, (T)control, lookAndFeel);
        }

        public abstract void Render(IGUIRenderingContext context, Time time, T control, LookAndFeel lookAndFeel);


    }
}
