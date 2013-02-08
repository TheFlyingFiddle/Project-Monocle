using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Graphics;

namespace Monocle.EntityGUI
{
    class Label : GUIControl
    {
        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alignment of the label.
        /// </summary>
        public TextAlignment Alignment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color TextColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font of the label.
        /// </summary>
        public Font Font
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a label with the given texturefont and text.
        /// </summary>
        public Label(Font font, string text)
            : base()
        {
            this.Alignment = TextAlignment.Left;
            this.BackgroundColor = Color.Transparent;
            this.TextColor = Color.Black;
            this.Font = font;
            this.Text = text;
        }

        protected internal override void Draw(ref Rect drawableArea, IGUIRenderer batch)
        {
            batch.DrawRect(ref drawableArea, this.BackgroundColor);
            batch.DrawString(this.Font, this.Text, ref drawableArea, this.TextColor, this.Alignment);
        }
    }
}
