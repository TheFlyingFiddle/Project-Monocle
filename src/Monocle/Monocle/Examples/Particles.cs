using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;
using System.Threading;
using Monocle.Utils;
using OpenTK.Graphics.OpenGL;

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

        private ParticleBatch batch;
        private ShaderProgram particleProgram;
        private Frame particleTexture;
        private Particle[] particles = new Particle[4096 * 64];

    

        Random random = new Random();
        protected override void Load(EntityGUI.GUIFactory factory)
        {
            this.particleProgram = this.Resourses.LoadAsset<ShaderProgram>("Sin.effect");
            this.batch = new ParticleBatch(this.GraphicsContext, this.particles.Length);

            GL.PointSize(2);

            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            this.Panel.BackgroundColor = Color.Black;
            for (int i = 0; i < particles.Length; i++)
            {
                var part = particles[i];
                part.position = new Vector2(random.Next(this.Width), random.Next(this.Height));
                part.direction = new Vector2(random.Next(-10, 10), random.Next(-10, 10));

                particles[i] = part;
            }

            this.w = this.Width;
            this.h = this.Height;

            new Thread(() => this.UpdateSprites(0, particles.Length / 4)).Start();
            new Thread(() => this.UpdateSprites(particles.Length / 4, particles.Length / 2)).Start();
            new Thread(() => this.UpdateSprites(particles.Length / 2, (particles.Length / 4) * 3)).Start();
            new Thread(() => this.UpdateSprites((particles.Length / 4) * 3, this.particles.Length)).Start();

            this.Panel.MouseDown += new EventHandler<EntityGUI.MouseButtonEventArgs>(Panel_MouseDown);
            this.Panel.MouseMove += new EventHandler<EntityGUI.MouseMoveEventArgs>(Panel_MouseMove);

            var tex = this.Resourses.LoadAsset<Texture2D>("particle-smoke.png");
            this.particleTexture = new Frame(tex.Bounds, tex);

            this.Resize += new Action(Particles_Resize);
        }


        private void UpdateSprites(int from, int to)
        {
            while (true)
            {
                for (int i = from; i < to; i++)
                {
                    var part = particles[i];
                    float wx = Wrap(0, w, (part.position.X + part.direction.X));
                    float wy = Wrap(0, h, (part.position.Y + part.direction.Y));

                    part.position = new Vector2(wx, wy);
                    part.color = GenColor(part.position);
                    particles[i] = part;
                }
            }
        }

        private float Wrap(int min, int max, float val)
        {
            if (val < min)
                return max;
            else if (val > max)
                return min;

            return val;
        }

        void Particles_Resize()
        {
            this.w = this.Width;
            this.h = this.Height;
        }

        void Panel_MouseMove(object sender, EntityGUI.MouseMoveEventArgs e)
        {
            if(random.NextDouble() > 0.95)
                Reposition(e.Position);
        }

        void Panel_MouseDown(object sender, EntityGUI.MouseButtonEventArgs e)
        {
            Reposition(e.Position);
        }

        private void Reposition(Vector2 pos)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    Vector2 norm = (this.particles[i].position - pos);
                    norm.Normalize();

                    int c = random.Next(1, 15);

                    particles[i].direction = norm * c;
                }
            });
        }

        

        protected override void Update(Utils.Time time)
        {
            base.Update(time);

            
        }
        Time time;
        protected override void Draw(Utils.Time time)
        {
            base.Draw(time);
            this.time = time;
            Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            for (int i = 0; i < particles.Length; i++)
            {
                this.batch.AddParticle(particles[i].position, new Vector2(2,2), particles[i].color);
            }

            this.batch.Render(ref proj, this.particleProgram);
        }

        private Color GenColor(Vector2 vector2)
        {
         /*   if (vector2.X < this.w / 2)
            {
                if (vector2.Y < this.h / 2)
                {
                    return Color.Gold;
                }
                else
                {
                    return new Color(Math.Abs((float)Math.Sin(vector2.X / 214)), 0.2f, Math.Abs((float)Math.Cos(vector2.Y / 214)), 1f);
                }
            }
            else
            {
                if (vector2.Y < this.h / 2)
                {
                    return new Color(0.5f, 1.0f, Math.Abs((float)Math.Cos(vector2.Y / 214)), 1f);
                }
                else
                {
                    return new Color(1f,0, Math.Abs((float)Math.Cos(vector2.Y / 100)), 1f);
                }
            }*/


            return new Color(Math.Abs((float)Math.Sin(vector2.X / 600 + time.Total.TotalSeconds / 15) / 2), Math.Abs((float)Math.Sin((vector2.X + vector2.Y) / 500) / 4), Math.Abs((float)Math.Cos(vector2.Y / 700) / 8), 0.0f);

        }
    }
}
