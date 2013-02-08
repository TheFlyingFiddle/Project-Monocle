using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.EntityGUI;

namespace Monocle.Examples
{
    class ButtonExample : OpenTKWindow
    {
        public ButtonExample(string resourceFolder)
            : base(1280, 720, 30, "Buttons and ImageBoxes Example", resourceFolder) { }

        protected override void Load(EntityGUI.GUIFactory factory)
        {
            //An atlas is a large image containing lots of smaller images.
            var webIcons = this.Resourses.LoadAsset<TextureAtlas>("Atlases\\WebIcons.atlas");
            var font = this.Resourses.LoadAsset<Font>("Fonts\\Metro.fnt");
            var bg = this.Resourses.LoadAsset<Texture2D>("Backgrounds\\Rainbow-Colors-Wallpaper-wallpapers-28469135-2560-1600.jpg");

            this.Panel.BackgroundImage = new Frame(bg.Bounds, bg);

            int i = 0;
            foreach (var icon in webIcons)
            {
                //An image box is simply a rectangle that has an image on it.
                ImageBox box = new ImageBox(icon.Value);
                box.Position = new OpenTK.Vector2(128 + 130 * i,this.Height / 2 - 128);
                box.Size = new OpenTK.Vector2(128, 128);
                box.BackgroundColor = Color.Blue * 0.3f;

                //Drawing the image box from a center position.
                box.Origin = Origin.Center;
                box.Name = icon.Key + 1;

                //Registers to events on the image box.
                box.FocusGained += EnlargeButton;
                box.FocusLost += ShrinkButton;
                //Lambda that writes what image box was pressed.
                box.Clicked += (s, e) => Console.WriteLine("WebIcon " + box.Name + " was just pressed");

                this.Panel.AddControl(box);

                //Standard button 
                Button button = new Button(font, "Button " + i);
                button.Position = new OpenTK.Vector2(box.Position.X, box.Position.Y + 130);
                button.Size = new OpenTK.Vector2(128, 128);
                button.BackgroundColor = Color.Red * 0.5f;
                button.Origin = Origin.Center;
                button.Icon = icon.Value;

                button.FocusGained += EnlargeButton;
                button.FocusLost += ShrinkButton;

                //Lambda that writes what button was pressed.
                button.Clicked += (s, e) => Console.WriteLine(button.Text + " was just pressed");

                this.Panel.AddControl(button);

                ImageBox box2 = new ImageBox(icon.Value);
                box2.Position = new OpenTK.Vector2(button.Position.X, button.Position.Y + 130);
                box2.Size = new OpenTK.Vector2(128, 128); 
                box2.BackgroundColor = Color.Purple * 0.3f;
                box2.Origin = Origin.Center;
                box2.Name = icon.Key + 2;

                box2.FocusGained += EnlargeButton;
                box2.FocusLost += ShrinkButton;
                box2.Clicked += (s, e) => Console.WriteLine("WebIcon " + box2.Name + " was just pressed");

                this.Panel.AddControl(box2);

                i++;
            }

        }

        protected override void Update(Utils.Time time)
        {
            base.Update(time);
        }

        //Makes the control larger and moves the other controls so that they do not overlap.
        private void EnlargeButton(object sender, EventArgs args)
        {
            GUIControl control = (GUIControl)sender;
            foreach (var item in this.Panel)
            {
                if (item == control) continue;

                if (item.Bounds.Left > control.Bounds.Left)
                {
                    item.Position += new OpenTK.Vector2(16, 0);
                }

                if (item.Bounds.Right < control.Bounds.Right)
                {
                    item.Position += new OpenTK.Vector2(-16, 0);
                }
               
                if (item.Bounds.Bottom > control.Bounds.Bottom)
                {
                    item.Position += new OpenTK.Vector2(0, 16);
                }

                if (item.Bounds.Top < control.Bounds.Top)
                {
                    item.Position += new OpenTK.Vector2(0, -16);
                }
            }

            control.Size += new OpenTK.Vector2(32, 32);

        }

        //Makes the control smaller and moves the other controls so that they do not overlap.
        private void ShrinkButton(object sender, EventArgs args)
        {
            GUIControl control = (GUIControl)sender;
            control.Size -= new OpenTK.Vector2(16, 16);
            foreach (var item in this.Panel)
            {
                if (item == control) continue;

                if (item.Bounds.Left > control.Bounds.Left)
                {
                    item.Position -= new OpenTK.Vector2(16, 0);
                }

                if (item.Bounds.Right < control.Bounds.Right)
                {
                    item.Position -= new OpenTK.Vector2(-16, 0);
                }

                if (item.Bounds.Bottom > control.Bounds.Bottom)
                {
                    item.Position -= new OpenTK.Vector2(0, 16);
                }

                if (item.Bounds.Top < control.Bounds.Top)
                {
                    item.Position -= new OpenTK.Vector2(0, -16);
                }
            }
            control.Size -= new OpenTK.Vector2(16, 16);
        }
    }
}
