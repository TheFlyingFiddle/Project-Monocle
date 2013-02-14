using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK;
using System.Threading;

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

        private Frame particleTexture;
        private Particle[] particles = new Particle[4096 * 4];

        Random random = new Random();
        protected override void Load(EntityGUI.GUIFactory factory)
        {
            this.Panel.BackgroundColor = Color.Black;
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].position = new Vector2(random.Next(this.Width), random.Next(this.Height));
                particles[i].direction = new Vector2(random.Next(-10, 10), random.Next(-10, 10));
            }

            this.w = this.Width;
            this.h = this.Height;

            Thread t = new Thread( () => 
                {
                    while (true)
                    {
                        for (int i = 0; i < particles.Length; i++)
                        {
                            float wx = Wrap(0, w, (particles[i].position.X + particles[i].direction.X));
                            float wy = Wrap(0, h, (particles[i].position.Y + particles[i].direction.Y));
    
                            particles[i].position = new Vector2(wx,wy);
                            particles[i].color = GenColor(particles[i].position);
                        }

                        Thread.Sleep(1);
                    }
                });
            t.IsBackground = true;
            t.Start();

            this.Panel.MouseDown += new EventHandler<EntityGUI.MouseButtonEventArgs>(Panel_MouseDown);
            this.Panel.MouseMove += new EventHandler<EntityGUI.MouseMoveEventArgs>(Panel_MouseMove);

            var tex = this.Resourses.LoadAsset<Texture2D>("particle-smoke.png");
            this.particleTexture = new Frame(tex.Bounds, tex);

            this.Resize += new Action(Particles_Resize);
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
            for (int i = 0; i < particles.Length; i++)
            {
                Vector2 norm = (this.particles[i].position - pos);
                norm.Normalize();

                int c = random.Next(1, 5);
                
                particles[i].direction = norm * c;
            }
        }

        

        protected override void Update(Utils.Time time)
        {
            base.Update(time);

            
        }

        protected override void Draw(Utils.Time time)
        {
            base.Draw(time);

            Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            for (int i = 0; i < particles.Length; i++)
            {
                this.spriteBuffer.BufferFrame(this.particleTexture, particles[i].position, particles[i].color, Vector2.Zero, new Vector2(0.2f,0.2f));
            }

            this.spriteBuffer.Draw(ref proj);
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


            return new Color(Math.Abs((float)Math.Sin(vector2.X / 214) / 4), Math.Abs((float)Math.Sin((vector2.X + vector2.Y) / 642) / 4), Math.Abs((float)Math.Cos(vector2.Y / 214) / 4), 0.0f);

        }
    }
}
