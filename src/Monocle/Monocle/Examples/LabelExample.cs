using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.EntityGUI;
using Monocle.Graphics;
using OpenTK;

namespace Monocle.Examples
{
    class LabelExample : OpenTKWindow
    {
        public LabelExample(string resourceFolder)
            : base(1280, 720, 30, "Label Example", resourceFolder) { }

        protected override void Load(GUIFactory factory)
        {
            var font = this.Resourses.LoadAsset<Font>("Fonts\\Consolas.fnt"); // Load the default font.
            var bigfont = this.Resourses.LoadAsset<Font>("Fonts\\BigMetro.fnt"); // Load another font.
            
            this.Panel.BackgroundColor = Color.White;

            //Left aligned golden label.
            Label left = new Label(font, "A left aligned label.");
            left.TextColor = Color.Gold;
            left.Position = new Vector2(50, 50);
            left.Size = new Vector2(300, font.Size);
            
            //Adds the label to the root panel. Now it will exsist in the window.
            this.Panel.AddControl(left);

            //Center aligned orange halfopaqe label.
            Label center = new Label(font, "Center aligned label:");
            center.Alignment = TextAlignment.Center;
            center.TextColor = Color.Orange * 0.5f; //Creates the color orange and premultiplies it by 0.5f alpha. 
            center.Position = new Vector2(50, left.Bounds.Bottom + 10);
            center.Size = new Vector2(300, font.Size);

            this.Panel.AddControl(center);

            //Right Aligned label.
            Label right = new Label(font,"Right Aligned Label:");
            right.Alignment = TextAlignment.Right;
            right.TextColor = Color.Magenta;
            right.Position = new Vector2(50, center.Bounds.Bottom + 10);
            right.Size = new Vector2(300, font.Size);

            this.Panel.AddControl(right);

            //Multi line label.
            Label multiLine = new Label(font, "This is a \nmulti-line label");
            multiLine.TextColor = Color.Black;
            multiLine.Position = new Vector2(50, right.Bounds.Bottom + 10);
            multiLine.Size = new Vector2(300, font.Size * 2);

            this.Panel.AddControl(multiLine);


            //Label with a non transparent background.
            Label backgroundLabel = new Label(font, "This is a label with non transparent background color.");
            backgroundLabel.BackgroundColor = Color.Purple;
            backgroundLabel.TextColor = Color.White;
            backgroundLabel.Position = new Vector2(50, multiLine.Bounds.Bottom + 10);
            backgroundLabel.Size = new Vector2(450, font.Size);

            this.Panel.AddControl(backgroundLabel);

            //Label with a big font.
            Label bigLabel = new Label(bigfont, "This is a label with a BIG font.");
            bigLabel.TextColor = Color.Black;
            bigLabel.Position = new Vector2(50, backgroundLabel.Bounds.Bottom + 10);
            bigLabel.Size = new Vector2(400, bigfont.Size);

            this.Panel.AddControl(bigLabel);
            

            //Window centered label.
            Label windowCenter = new Label(bigfont, "Window Center Label");
            windowCenter.TextColor = Color.Black;
            windowCenter.Position = new Vector2(this.Width / 2, 50);
            windowCenter.Size = new Vector2(400, bigfont.Size);
            windowCenter.Origin = Origin.Center;
            windowCenter.Alignment = TextAlignment.Center;

            //Registers a lambda to the resize event and correctly reposition the windowCenter label.
            this.Resize += ()=>
            {
                windowCenter.Position = new Vector2(this.Width / 2, 50);
            };

            this.Panel.AddControl(windowCenter);    
        }
    }
}
