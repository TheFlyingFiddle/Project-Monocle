using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    class Button : ButtonBase
    {
        public Button() 
            : base()
        {
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
    }
}