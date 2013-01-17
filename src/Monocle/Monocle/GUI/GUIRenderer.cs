using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Utils;

namespace Monocle.GUI
{
    class GUIRenderer
    {
        private readonly Dictionary<Type, IControlRenderer> renderers;
        private readonly IGUIRenderingContext renderingContext;
        private readonly GUISkin skin;

        public GUIRenderer()
        {
            renderers = new Dictionary<Type, IControlRenderer>();
        }


        public void RenderControl(IGUIControl control, Time time, ref Matrix4 projection)
        {
            IControlRenderer renderer;
            if (!this.renderers.TryGetValue(control.GetType(), out renderer))
            {
                renderer = CreateRenderer(control.GetType());
            }
            
            renderingContext.Begin(ref projection);
            renderer.Render(this.renderingContext, time, control);
            renderingContext.End();
        }

        private IControlRenderer CreateRenderer(Type type)
        {
            var atrib = (GUIControlAttribute)type.GetCustomAttributes(typeof(GUIControlAttribute), false)[0];
            var renderer = (IControlRenderer)Activator.CreateInstance(atrib.Renderer, skin);

            this.renderers.Add(type, renderer);
            return renderer;
        }
    }
}