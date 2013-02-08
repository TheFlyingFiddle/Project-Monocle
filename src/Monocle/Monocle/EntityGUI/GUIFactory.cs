using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.EntityGUI
{
    class GUIFactory
    {
        private readonly Dictionary<Type, GUIControl> standardObjects;

        public GUIFactory()
        {
            standardObjects = new Dictionary<Type, GUIControl>();
        }

        public T Create<T>(float x, float y, float width, float height, Origin origin = Origin.TopLeft) where T : GUIControl
        {
            var standard = this.standardObjects[typeof(T)];
            var clone = (GUIControl)standard.Clone();

            clone.Position = new OpenTK.Vector2(x, y);
            clone.Size = new OpenTK.Vector2(width, height);
            clone.Origin = origin;

            return (T)clone;
        }


        public void RegisterStandardObject<T>(T obj) where T : GUIControl
        {
            this.standardObjects.Add(typeof(T), obj);
        }
    }
}
