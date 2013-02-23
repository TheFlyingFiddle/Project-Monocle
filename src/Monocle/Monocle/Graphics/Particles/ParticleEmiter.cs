using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Utils;

namespace Monocle.Graphics.Particles
{
    class ParticleEmiter
    {
        public Vector2 Position { get; set; }
        private ParticleSystem system;
        
        private TimeSpan particleInterval;
        private TimeSpan elapsed;

        private Frame[] ParticleFrame;


        public ParticleEmiter(Vector2 initialPosition, TimeSpan particleInterval, ParticleSystem system, Frame[] particleFrame)
        {
            this.Position = initialPosition;
            this.particleInterval = particleInterval;
            this.system = system;
            this.ParticleFrame = particleFrame;
        }


        public void Update(Time time)
        {
            elapsed += time.Elapsed;
            while (elapsed > particleInterval)
            {
                elapsed -= particleInterval;
                EmitParticle();                
            }
        }

        Random random = new Random();
        private void EmitParticle()
        {
            Random random = Examples.Particles.Random;
            double angle = random.NextDouble() * Math.PI * 2;

            Vector2 position = this.Position;
            Vector2 velocity = new Vector2(200,-100 + random.Next(0,200));
            Vector2 size = new Vector2(4,4);

            var frame = ParticleFrame[random.Next(ParticleFrame.Length)];

            this.system.AddParticle(position, velocity, size, frame.TextureCoordinates);
        }        
    }
}
