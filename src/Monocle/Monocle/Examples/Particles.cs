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

        int w, h;

        private struct Particle
        {
            internal Color color;
            internal Vector2 position;
            internal Vector2 direction;
        }

        private PointBatch batch;
        private ShaderProgram particleProgram;
        private Frame particleTexture;
        private Particle[] particles = new Particle[4096 * 64];


        private ParticleSystem[] systems;
        private ParticleEmiter[] emiters;
        private ParticleSettings particleSettings;

        Random random;
        protected override void Load(EntityGUI.GUIFactory factory)
        {
            this.particleProgram = this.Resourses.LoadAsset<ShaderProgram>("Sin.effect");
            this.Panel.BackgroundColor = Color.Gray;

            random = new Random();
          //  this.Panel.MouseMove += new EventHandler<EntityGUI.MouseMoveEventArgs>(Panel_MouseMove);
            this.Panel.MouseDown += new EventHandler<EntityGUI.MouseButtonEventArgs>(Panel_MouseDown);

            var tex = this.Resourses.LoadAsset<Texture2D>("particle.png");
            this.particleTexture = new Frame(tex.Bounds, tex);


            this.systems = new ParticleSystem[10];
            this.emiters = new ParticleEmiter[10];
            this.Panel_MouseDown(null, null);
        }

        void Panel_MouseDown(object sender, EntityGUI.MouseButtonEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Color c = random.NextColor();
                    Color c2 = random.NextColor();

                    ParticleSettings settings = new ParticleSettings(new Vector2(random.Next(-200, 100), random.Next(-500, 500)), c * 0.2f, c2 * 0.7f,
                                                                     Color.Black * 0.5f,
                                                                     random.Next(2, 8), random.Next(16, 32), 3f, 5, 10, random.Next(3, 4));


                    this.systems[i * 2 + j] = new ParticleSystem(this.GraphicsContext, 1024, settings);
                    this.emiters[i * 2 + j] = new ParticleEmiter(new Vector2(this.Width / 5 * i + 150, this.Height / 2* j + 250), TimeSpan.FromSeconds(1 / 150d), systems[i * 2 + j], this.particleTexture);
                }
            }
        }

        public static Random Random = new System.Random();

        void Panel_MouseMove(object sender, EntityGUI.MouseMoveEventArgs e)
        {
            emiters[random.Next(emiters.Length)].Position = e.Position;
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
                item.Render(ref proj, this.particleProgram, this.particleTexture.Texture2D);
            }
        }
    }
}
