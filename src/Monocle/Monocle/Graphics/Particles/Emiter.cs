using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Core;
using OpenTK;
using Monocle.Utils;

namespace Monocle.Graphics.Particles
{
    abstract class Emiter2D
    {
        public Vector2 Position
        {
            get;
            set;
        }

        public Frame[] ParticleFrames
        {
            get;
            set;
        }

        public ParticleSystem ParticleSystem
        {
            get;
            set;
        }

        public TimeSpan EmitInterval
        {
            get;
            set;
        }

        public int EmitCount
        {
            get;
            set;
        }

        private TimeSpan elapsed;

        public Emiter2D(Vector2 initialPosition, IEnumerable<Frame> particleFrames, ParticleSystem particleSystem)
        {
            this.EmitCount = 1;
            this.Position = initialPosition;
            this.ParticleFrames = particleFrames.ToArray();
            this.ParticleSystem = particleSystem;

            EmitInterval = TimeSpan.FromSeconds(1);
            elapsed = TimeSpan.Zero;
        }

        public virtual void Update(Time time)
        {
            elapsed += time.Elapsed;
            while (elapsed > EmitInterval)
            {
                elapsed -= EmitInterval;
                for (int i = 0; i < this.EmitCount; i++)
                {
                    EmitParticle(this.RandomParticleFrame());                    
                }
            }
        }

        protected abstract void EmitParticle(Frame particleFrame);


        protected Frame RandomParticleFrame()
        {
            return this.ParticleFrames[MathHelper.RandomInt(0, ParticleFrames.Length)];
        }
    }
    
    class SprayEmiter2D : Emiter2D
    {
        public float MaxSpeed
        {
            get;
            set;
        }

        public float MinSpeed
        {
            get;
            set;
        }

        public float MinAngle
        {
            get;
            set;
        }

        public float MaxAngle
        {
            get;
            set;
        }

        public SprayEmiter2D(float minSpeed, float maxSpeed, float minAngle, float maxAngle, Vector2 position, IEnumerable<Frame> particleFrames, ParticleSystem system)
            : base(position, particleFrames, system)
        {
            this.MinSpeed = minSpeed;
            this.MaxSpeed = maxSpeed;
            this.MinAngle = minAngle;
            this.MaxAngle = maxAngle;
       
        }

        protected override void EmitParticle(Frame particleFrame)
        {
            float speed = MathHelper.RandomFloat(MinSpeed, MaxSpeed);
            float angle = MathHelper.RandomFloat(MinAngle, MaxAngle);

            Vector2 velocity = new Vector2((float)Math.Sin(angle), 
                                           (float)Math.Cos(angle)) * speed;

            this.ParticleSystem.AddParticle(this.Position, velocity, Vector2.One, particleFrame.TextureCoordinates);
        }
    }

    class CircleEmiter2D : Emiter2D
    {
        protected float inner, outer;


        public CircleEmiter2D(float innerRaius, float outerRadius, Vector2 initialPosition, IEnumerable<Frame> particleFrames, ParticleSystem particleSystem)
            : base(initialPosition, particleFrames, particleSystem)
        {
            this.inner = innerRaius;
            this.outer = outerRadius;
        }


        protected override void EmitParticle(Frame particleFrame)
        {
            float angle = MathHelper.RandomFloat(0, (float)Math.PI * 2);
            float position = MathHelper.RandomFloat(inner, outer);

            Vector2 offset = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * position;

            float r = MathHelper.RandomFloat(1, 5);

            this.ParticleSystem.AddParticle(this.Position + offset, Vector2.Zero, new Vector2(r, r), particleFrame.TextureCoordinates);

        }
    }

    class BeeEmiter2D : Emiter2D
    {
        public BeeEmiter2D(Vector2 initialPosition, IEnumerable<Frame> particleFrames, ParticleSystem particleSystem)
            : base(initialPosition, particleFrames, particleSystem) { }

        public override void Update(Time time)
        {
            Vector2 n = new Vector2(MathHelper.RandomFloat(-1, 1), MathHelper.RandomFloat(-1,1)) * time.ElapsedSeconds* 1000;
            this.Position += n;
            base.Update(time);
        }

        protected override void EmitParticle(Frame particleFrame)
        {
            this.ParticleSystem.AddParticle(this.Position, Vector2.Zero, Vector2.One, particleFrame.TextureCoordinates);
        }
    }

    class HoleEmiter : CircleEmiter2D
    {


        public HoleEmiter(float innerRaius, float outerRadius, Vector2 initialPosition, IEnumerable<Frame> particleFrames, ParticleSystem particleSystem)
            : base(innerRaius, outerRadius, initialPosition, particleFrames, particleSystem)
        {
        }


        protected override void EmitParticle(Frame particleFrame)
        {
            float angle = MathHelper.RandomFloat(0, (float)Math.PI * 2);
            float position = MathHelper.RandomFloat(inner, outer);

            Vector2 offset = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * position;
            Vector2 velocity = -offset * 1.2f;


            float r = MathHelper.RandomFloat(1, 5);

            this.ParticleSystem.AddParticle(this.Position + offset, velocity, new Vector2(r, r), particleFrame.TextureCoordinates);
        }
    }


}
