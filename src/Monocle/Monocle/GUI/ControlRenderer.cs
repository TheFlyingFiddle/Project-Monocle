using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.Utils;

namespace Monocle.GUI
{
    public interface IControlRenderer
    {
        void Render(IGUIRenderingContext context, Time time, IGUIControl control); 
    }

    public interface IControlRenderer<T> : IControlRenderer  where T : IGUIControl
    {
        void Render(IGUIRenderingContext context, Time time, T control);
    }

    public abstract class ControlRenderer<T> : IControlRenderer<T> where T : IGUIControl
    {
        protected readonly GUIStyle DefaultStyle;

        public ControlRenderer(GUISkin skin, string styleID)
        {
            this.DefaultStyle = skin[styleID];
        }

        public void Render(IGUIRenderingContext context, Time time, IGUIControl control)
        {
            this.Render(context, time, (T)control);
        }

        public abstract void Render(IGUIRenderingContext context, Time time, T control);


    }
}
