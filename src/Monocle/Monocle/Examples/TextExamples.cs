using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.EntityGUI;
using Monocle.EntityGUI.Controls;

namespace Monocle.Examples
{
    class TextExample : OpenTKWindow
    {
        public TextExample(string resourceFolder)
            : base(1280, 720, 30, "Text Example", resourceFolder) { }


        protected override void Load(EntityGUI.GUIFactory factory)
        {
            var font = this.Resourses.LoadAsset<Font>("Fonts\\Metro.fnt");
            var bigfont = this.Resourses.LoadAsset<Font>("Fonts\\BigMetro.fnt");
            this.Panel.BackgroundColor = Color.Black;

            //Creates a textbox with maxlength 20
            TextBox textBox0 = new TextBox(font, "", 20);
            textBox0.Position = new OpenTK.Vector2(50, 50);

            //It should be noted that a textbox height is always font.size + padding.y + padding.h since it would 
            //i did it this way since a textbox of any other height would look bad.
            textBox0.Size = new OpenTK.Vector2(200, 20);

            //A hint is displayed if the textbox is empty.
            textBox0.Hint = "20 char text field!";

            this.Panel.AddControl(textBox0);

            //Creates a textbox with unlimited length.
            TextBox textBox1 = new TextBox(font, "");
            textBox1.BackgroundColor = Color.White;
            textBox1.TextColor = Color.Red;
            textBox1.Position = new OpenTK.Vector2(50, textBox0.Bounds.Bottom + 10);
            textBox1.Size = new OpenTK.Vector2(200, 20);
            textBox1.Hint = "Unlimited textbox.";

            this.Panel.AddControl(textBox1);

            //Creates a textbox with custom colors.
            TextBox textBox2 = new TextBox(font, "");
            textBox2.BackgroundColor = Color.Green;
            textBox2.TextColor = Color.Gold;
            textBox2.SelectedColor = Color.Red;
            textBox2.HintColor = Color.RosyBrown;
            textBox2.Position = new OpenTK.Vector2(50, textBox1.Bounds.Bottom + 10);
            textBox2.Size = new OpenTK.Vector2(200, 20);
            textBox2.Hint = "Unlimited textbox 2.";

            this.Panel.AddControl(textBox2);

            TextBox textBox3 = new TextBox(bigfont, "", 20);
            textBox3.Position = new OpenTK.Vector2(50, textBox2.Bounds.Bottom + 10);
            textBox3.Size = new OpenTK.Vector2(300, 20);
            textBox3.Hint = "Unlimited textbox 2.";

            this.Panel.AddControl(textBox3);

            //Create a numberbox startvalue 100 min 0 max 255
            NumberBox numberBox0 = new NumberBox(font, 100, byte.MinValue, byte.MaxValue);
            numberBox0.Position = new OpenTK.Vector2(50, textBox3.Bounds.Bottom + 10);
            numberBox0.Size = new OpenTK.Vector2(200, 20);

            this.Panel.AddControl(numberBox0);

            TextArea area = new TextArea(font);
            area.Position = new OpenTK.Vector2(textBox3.Bounds.Right + 20, 50);
            area.Size = new OpenTK.Vector2(400, 300);
            area.Text = "This is a text area! This is not fully implemented! Yes true it is.";
            this.Panel.AddControl(area);

            ListBox<int> numbers = new ListBox<int>(font);
            numbers.Position = new OpenTK.Vector2(area.Bounds.Right + 20, 50);
            numbers.Size = new OpenTK.Vector2(400, 300);

            for (int i = 0; i < 20; i++)
            {
                numbers.AddItem(i);
            }

            this.Panel.AddControl(numbers);
        }
    }
}
