using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.EntityGUI;

namespace Monocle.Examples
{
    class AnimationExample : OpenTKWindow
    {
        public AnimationExample(string resourceFolder)
            : base(1280, 720, 30, "AnimationBox Example", resourceFolder) { }


        protected override void Load(EntityGUI.GUIFactory factory)
        {
            //Note: Animations currently have no associated file format so 
            //we build them manually in code.


            //Loads two atlases that will be the basis for our animations.
            var webIconsAtlas = this.Resourses.LoadAsset<TextureAtlas>("Atlases\\WebIcons.atlas");
            var osIconsAtlas = this.Resourses.LoadAsset<TextureAtlas>("Atlases\\OsIcons.atlas");

            //Converts the atlases to arrays of frames.
            var anim0Frames = webIconsAtlas.Select((x) => x.Value).ToArray();
            var anim1Frames = osIconsAtlas.Select((x) => x.Value).ToArray();


            //Creates two animations from the animation frames.
            Animation anim0 = new Animation(anim0Frames, 5);
            Animation anim1 = new Animation(anim1Frames, 3);

            //Creates an animationbox with the anim0 animation.
            AnimationBox box0 = new AnimationBox(anim0);
            box0.Position = new OpenTK.Vector2(50, 50);
            box0.Size = new OpenTK.Vector2(256, 256);
            box0.BackgroundColor = Color.Purple * 0.1f;

            this.Panel.AddControl(box0);

            //Creates a second animationbox with the anim0 animation.
            //It should be noted that the animationbox clones the animation 
            //so you can use the same animation on multiple boxes.
            AnimationBox box1 = new AnimationBox(anim0);
            box1.Position = new OpenTK.Vector2(50, box0.Bounds.Bottom + 50);
            box1.Size = new OpenTK.Vector2(256, 256);
            box1.BackgroundColor = Color.Yellow * 0.1f;

            this.Panel.AddControl(box1);


            AnimationBox box2 = new AnimationBox(anim1);
            box2.Position = new OpenTK.Vector2(this.Width - 50, 50);
            box2.Size = new OpenTK.Vector2(256, 256);
            box2.BackgroundColor = Color.Green * 0.1f;

            //Put the origin to the right to simplify placement.
            box2.Origin = Origin.TopRight;

            this.Panel.AddControl(box2);
        }
    }
}
