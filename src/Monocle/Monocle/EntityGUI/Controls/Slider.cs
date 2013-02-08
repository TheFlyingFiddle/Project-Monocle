using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using Monocle.Graphics;

namespace Monocle.EntityGUI
{
    class Slider : ScrollBase
    {
        public Color FilledColor
        {
            get;
            set;
        }


        public Slider(Orientation orientation, int min, int max, int value)
            : base(orientation, min, max, value)
        {
        }

        private void Draw(ref Rect area, IGUIRenderer batch, Color bgc, Color fc, Color bc)
        {
            batch.DrawRect(ref area, bgc);

            float center = this.Value / (this.MaxValue - this.MinValue);
            Rect button;
            Rect filled = area;
            if (Orientation == EntityGUI.Orientation.Horizontal)
            {
                button = new Rect(area.X, area.Y, area.H, area.H);
                button.X += center * (area.W - area.H);
                filled.W = button.X - filled.X;
            }
            else
            {
                button = new Rect(area.X, area.Y, area.W, area.W);
                button.Y += center * (area.H - area.W);
                filled.H = button.Y - filled.Y;
            }
            batch.DrawRect(ref filled, fc);
            batch.DrawRect(ref button, bc);

        }

        protected internal override void Draw(ref Rect area, IGUIRenderer batch)
        {
            base.Draw(ref area, batch);
            if (Focused)
            {
                Color bgc = Color.AddContrast(this.BackgroundColor, 0.2f);
                Color fc = Color.AddContrast(this.FilledColor, 0.2f);
                Color bc = Color.AddContrast(this.ButtonColor, 0.2f);

                this.Draw(ref area, batch, bgc, fc, bc);
            }
            else
            {
                this.Draw(ref area, batch, this.BackgroundColor, this.FilledColor, this.ButtonColor);
            }

        }
    }
}
