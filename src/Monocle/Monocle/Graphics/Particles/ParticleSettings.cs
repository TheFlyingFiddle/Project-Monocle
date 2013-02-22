using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Monocle.Graphics.Particles
{
    class ParticleSettings
    {
        public Vector2 Forces;

        public Color StartColor;
        public Color EndColor;
        public Color ColorVariance;


        public float StartSize;
        public float EndSize;

        public float StartAngularVelocity;
        public float EndAngularVelocity;

        public float LifeTime;
        public float SizeVariance;



        public ParticleSettings(Vector2 forces, Color startC, Color endC, Color colorVariance, float startS, float endS, float sizeVariance, float startAngularVelocity, float endAngluarVelocity, float lifeTime)
        {
            this.SizeVariance = sizeVariance;
            this.ColorVariance = colorVariance;
            this.Forces = forces;
            this.StartColor = startC;
            this.EndColor = endC;
            this.StartSize = startS;
            this.EndSize = endS;
            this.LifeTime = lifeTime;
            this.StartAngularVelocity = startAngularVelocity;
            this.EndAngularVelocity = endAngluarVelocity;
        }
    }
}
