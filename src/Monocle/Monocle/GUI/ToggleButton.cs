using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    public class ToggleButton : ButtonBase
    {
        private readonly static ToggleButtonRenderer Default_Renderer = new ToggleButtonRenderer();

        public bool Down
        {
            get;
            private set;
        }

        public ToggleButton(MouseDevice device)
            : base(device)
        {
            this.Renderer = Default_Renderer;
        }

        protected override void OnMouseEnter(MouseEventArgs device)
        {
            base.OnMouseEnter(device);
            this.MouseHover = true;    
        }

        protected override void OnMouseExit(MouseEventArgs device)
        {
            base.OnMouseExit(device);
            this.MouseHover = false;
        }

        protected override void OnMouseDown(MouseButtonEventArgs device)
        {
            base.OnMouseDown(device);
            if (device.Button == MouseButton.Left)
                this.Pressed = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs device)
        {
            base.OnMouseUp(device);
            if (device.Button == MouseButton.Left)
                this.Pressed = false;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            this.Down = !this.Down;
        }
    }
}
