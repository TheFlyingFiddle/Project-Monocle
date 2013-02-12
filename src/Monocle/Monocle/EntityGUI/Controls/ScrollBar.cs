using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.EntityGUI
{
    class ScrollBar : ScrollBase
    {
        public const int MIN_SCROLLBAR_BUTTON_SIZE = 15;
        public const int DEFAULT_SCROLLBAR_SIZE = 9;

        public ScrollBar(Orientation orientation, int min, int max, int value)
            : base(orientation, min, max, value) { }

        protected internal override void Draw(ref Rect area, IGUIRenderer renderer)
        {
            base.Draw(ref area, renderer);
            
            this.Draw(ref area, renderer, this.BackgroundColor, this.ButtonColor);
        }

        private void Draw(ref Rect area, IGUIRenderer renderer, Color bgc, Color bc)
        {

            renderer.DrawRect(ref area, bgc);

            float center = this.Value / (this.MaxValue - this.MinValue);
            Rect bounds = this.Bounds;
            Rect button;
            if (Orientation == EntityGUI.Orientation.Horizontal)
            {
                button = area;
                button.W = MathHelper.Clamp(MIN_SCROLLBAR_BUTTON_SIZE, this.size.X / 2, this.size.X / ((this.MaxValue - this.MinValue) / Step) * 2);
                button.X += MathHelper.Clamp(0, area.W - button.W, center * (bounds.W - button.W));
            }
            else
            {
                button = area;
                button.H = MathHelper.Clamp(MIN_SCROLLBAR_BUTTON_SIZE, this.size.Y / 2, this.size.Y / ((this.MaxValue - this.MinValue) / Step) * 2);
                button.Y += MathHelper.Clamp(0, area.H - button.H, center * (bounds.H - button.H));
            }
            renderer.DrawRect(ref button, bc);
        }
    }
}
