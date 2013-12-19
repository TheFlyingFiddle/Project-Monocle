using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;
using System.Threading;
using Monocle.Utils;
using OpenTK.Graphics.OpenGL;
using Monocle.Graphics.Particles;

namespace Monocle.Examples
{
    class Particles : OpenTKWindow
    {
        public Particles(string p)
            : base(1280, 720, 60, "Particles", p)
        { }

        private ShaderProgram particleProgram;
        private TextureAtlas particleAtlas;        

        private ParticleSystem[] systems;
        private Emiter2D[] emiters;

        protected override void Load(EntityGUI.GUIFactory factory)
        {
            this.particleProgram = this.Resourses.LoadAsset<ShaderProgram>("Sin.effect");
            this.Panel.BackgroundColor = Color.Gray;

            this.Panel.MouseDown += new EventHandler<EntityGUI.MouseButtonEventArgs>(Panel_MouseDown);
            this.Panel.MouseMove +=new EventHandler<EntityGUI.MouseMoveEventArgs>(Panel_MouseMove);

            particleAtlas = this.Resourses.LoadAsset<TextureAtlas>("Atlases\\Particles.atlas");


            this.systems = new ParticleSystem[10];
            this.emiters = new Emiter2D[10];
            this.Panel_MouseDown(null, null);
        }

        void Panel_MouseDown(object sender, EntityGUI.MouseButtonEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Color c = Random.NextColor() * 0.5f;
                    Color c2 = Random.NextColor();

                    ParticleSettings settings = new ParticleSettings(new Vector2(Random.Next(-30, 30), Random.Next(-30, 30)), c * 0.7f, c2,
                                                                     Color.Black * 0.5f,
                                                                     0, Random.Next(30, 40), 3f, 0, 2, Random.Next(1, 2));


                    this.systems[i * 2 + j] = new ParticleSystem(this.GraphicsContext, 256 * 2, settings);

                    double r = Random.NextDouble();
                    if (r > 0.66f)
                    {

                        SprayEmiter2D emiter = new SprayEmiter2D(100f, 200f, 0, (float)(Random.NextDouble() * Math.PI), new Vector2(this.Width / 5 * i + 150, this.Height / 2 * j + 150),
                        this.particleAtlas.ToArray(), systems[i * 2 + j]);
                        emiter.EmitInterval = TimeSpan.FromSeconds(1d / 64);
                        emiter.EmitCount = 2;


                        this.emiters[i * 2 + j] = emiter;
                    }
                    else if (r > 0.33d)
                    {
                        BeeEmiter2D emiter = new BeeEmiter2D(new Vector2(this.Width / 5 * i + 150, this.Height / 2 * j + 150),
                        new Frame[] { particleAtlas["Cloud002"], particleAtlas["Flame"], particleAtlas["Cloud004"] }, systems[i * 2 + j]);
                        emiter.EmitInterval = TimeSpan.FromSeconds(1d / 64);
                        emiter.EmitCount = 4;

                        this.emiters[i * 2 + j] = emiter;
                    }
                    else
                    {
                        CircleEmiter2D emiter = new CircleEmiter2D(50, 75, new Vector2(this.Width / 5 * i + 150, this.Height / 2 * j + 150),
                          this.particleAtlas.ToArray(), systems[i * 2 + j]);
                        emiter.EmitInterval = TimeSpan.FromSeconds(1d / 32);
                        emiter.EmitCount = 8;


                        this.emiters[i * 2 + j] = emiter;

                    }
                        
                }
            }
        }

        public static Random Random = new System.Random();

        void Panel_MouseMove(object sender, EntityGUI.MouseMoveEventArgs e)
        {
            emiters[0].Position = e.Position;
        }
               

        protected override void Update(Utils.Time time)
        {
            base.Update(time);
            foreach (var item in this.systems)
            {
                item.Update(time);
            }

            foreach (var emiter in this.emiters)
            {
                emiter.Update(time);
            }

        }

        Time time;
        protected override void Draw(Utils.Time time)
        {
            base.Draw(time);
            this.time = time;
            Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            foreach (var item in systems)
            {
                item.Render(ref proj, this.particleProgram, this.particleAtlas.Texture);
            }
        }
    }
}
