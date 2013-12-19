using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Utils;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using Monocle.Utils.Logging;

namespace Monocle.Graphics.Particles
{
    class ParticleSystem
    {
        private const int ELEMENTS_PER_SQUARE = 6;

        private const string PROJ_MAT = "projection_matrix";
        private const string FORCES = "forces";
        
        private const string START_COLOR = "start_color";
        private const string END_COLOR = "end_color";


        private const string START_ALPHA = "start_alpha";
        private const string END_ALPHA = "end_alpha";

        private const string START_SIZE = "start_size";
        private const string END_SIZE = "end_size";

        private const string SIZE_VARIANCE = "size_variance";

        private const string LIFE_TIME = "life_time";
        private const string CURRENT_TIME = "current";

        private const string COLOR_VARIANCE = "color_variance";

        public const string START_ANGULAR_VELOCITY = "start_angular_velocity";
        public const string END_ANGULAR_VELOCITY = "end_angular_velocity";

        private const string IN_POS  = "in_position";
        private const string IN_VEL = "in_velocity";
        private const string IN_RANDOM = "in_random";
        private const string IN_TIME = "in_time";
        private const string IN_COORDS = "in_coords";
        private const string IN_OFFSET = "in_offset";


        private const string TEXTURE = "tex";
                
        private VertexBuffer<Particle> vertexBuffer;
        private IntIndexBuffer indecies;

        private Particle[] queue;

        private ParticleSettings settings;

        private float currentTime = 0;
        private int drawCounter = 0;

        private int vba;

        int firstActiveParticle;
        int firstNewParticle;
        int firstFreeParticle;
        int firstRetiredParticle;

        private int MaxParticles
        {
            get { return this.queue.Length / 4; }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Particle : IVertex
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public Vector2 Random;
            public Vector2 Offset;
            public Vector2 Coords;
            public float Time;

            public int SizeInBytes
            {
                get { return Vector2.SizeInBytes * 5 + sizeof(float); }
            }
        }

        public ParticleSystem(IGraphicsContext context,int maxParticleCount, ParticleSettings settings)
        {
            this.settings = settings;
            this.queue = new Particle[maxParticleCount * 4];

            this.vertexBuffer = new VertexBuffer<Particle>(context, OpenTK.Graphics.OpenGL.BufferUsageHint.StreamDraw);
            this.indecies = new IntIndexBuffer(context, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);

            this.SetIndexBufferSize(maxParticleCount);

            this.vertexBuffer.Bind();
            this.vertexBuffer.SetData(this.queue);



            GL.GenVertexArrays(1, out vba);
        }

        private unsafe void SetIndexBufferSize(int count)
        {
            int[] indecies = new int[count * 6];
            fixed (int* ptr = &indecies[0])
            {
                for (int i = 0; i < count; i++)
                {
                    ptr[i * 6] = (i * 4);
                    ptr[i * 6 + 1] = (i * 4 + 1);
                    ptr[i * 6 + 2] = (i * 4 + 2);
                    ptr[i * 6 + 3] = (i * 4);
                    ptr[i * 6 + 4] = (i * 4 + 2);
                    ptr[i * 6 + 5] = (i * 4 + 3);
                }
            }
            this.indecies.context.BindIndexBuffer(this.indecies);
            this.indecies.SetData(indecies);
        }


        private void SetupShaderProgram(ref Matrix4 projection, ShaderProgram program)
        {
            IGraphicsContext context = program.GraphicsContext;
            context.UseShaderProgram(program);

            program.SetUniform(PROJ_MAT, ref projection);
            program.SetUniform(FORCES, ref this.settings.Forces);
            
            program.SetUniform(START_COLOR, settings.StartColor);
            program.SetUniform(END_COLOR, settings.EndColor);
            program.SetUniform(START_SIZE, settings.StartSize);
            program.SetUniform(END_SIZE, settings.EndSize);
            program.SetUniform(START_ANGULAR_VELOCITY, settings.StartAngularVelocity);
            program.SetUniform(END_ANGULAR_VELOCITY, settings.EndAngularVelocity);
            program.SetUniform(COLOR_VARIANCE, settings.ColorVariance);
            program.SetUniform(SIZE_VARIANCE, settings.SizeVariance);

            Vector3 g = new Vector3(500,500, -0);
            program.SetUniform("gravity_well_1", ref g);
            g = new Vector3(1550, 500, 0);
            program.SetUniform("gravity_well_2", ref g);


            program.SetUniform(START_ALPHA, 1.5f);
            program.SetUniform(END_ALPHA, 0f);

            program.SetUniform(LIFE_TIME, settings.LifeTime);
            program.SetUniform(CURRENT_TIME, this.currentTime);

            program.SetUniform(TEXTURE, 0);
            

            GL.BindVertexArray(this.vba);

            int posIndex = program.GetAttributeLocation(IN_POS);
            int veloIndex = program.GetAttributeLocation(IN_VEL);
            int randomIndex = program.GetAttributeLocation(IN_RANDOM);
            int offsetIndex = program.GetAttributeLocation(IN_OFFSET);
            int coordIndex = program.GetAttributeLocation(IN_COORDS);
            int timeIndex = program.GetAttributeLocation(IN_TIME);
            
            

            context.BindVertexBuffer(this.vertexBuffer);

            context.EnableVertexAttribArray(posIndex);
            context.EnableVertexAttribArray(veloIndex);
            context.EnableVertexAttribArray(randomIndex);
            context.EnableVertexAttribArray(offsetIndex);
            context.EnableVertexAttribArray(coordIndex);
            context.EnableVertexAttribArray(timeIndex);

            context.VertexAttribPointer(posIndex, 2, VertexAttribPointerType.Float,
                                             false, this.queue[0].SizeInBytes, 0);

            context.VertexAttribPointer(veloIndex, 2, VertexAttribPointerType.Float,
                                             false, this.queue[0].SizeInBytes, Vector2.SizeInBytes);

            context.VertexAttribPointer(randomIndex, 2, VertexAttribPointerType.Float,
                                            false, this.queue[0].SizeInBytes, Vector2.SizeInBytes * 2);


            context.VertexAttribPointer(offsetIndex, 2, VertexAttribPointerType.Float,
                                            false, this.queue[0].SizeInBytes, Vector2.SizeInBytes * 3);


            context.VertexAttribPointer(coordIndex, 2, VertexAttribPointerType.Float,
                                false, this.queue[0].SizeInBytes, Vector2.SizeInBytes * 4);


            context.VertexAttribPointer(timeIndex, 1, VertexAttribPointerType.Float,
                                             false, this.queue[0].SizeInBytes, Vector2.SizeInBytes * 5);


            GL.BindVertexArray(0);
        }


        public void Update(Time time)
        {
            currentTime += time.ElapsedSeconds;

            RetireActiveParticles();
            FreeRetiredParticles();

            if (firstActiveParticle == firstFreeParticle)
                currentTime = 0;

            if (firstRetiredParticle == firstActiveParticle)
                drawCounter = 0;

        }
        
        private void RetireActiveParticles()
        {
            while (firstActiveParticle != firstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = currentTime - queue[firstActiveParticle * 4].Time;

                if (particleAge < settings.LifeTime)
                    break;

                // Remember the time at which we retired this particle.
                queue[firstActiveParticle * 4].Time = drawCounter;

                // Move the particle from the active to the retired queue.
                firstActiveParticle = (firstActiveParticle + 1) % this.MaxParticles;
            }
        }

        private void FreeRetiredParticles()
        {
            while (firstRetiredParticle != firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                // We multiply the retired particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                int age = drawCounter - (int)queue[firstRetiredParticle * 4].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                firstRetiredParticle = (firstRetiredParticle + 1) % this.MaxParticles;
            }
        }


        public void Render(ref Matrix4 projection, ShaderProgram program, Texture2D texture)
        {
            this.SetupShaderProgram(ref projection, program);
            this.AddNewParticlesToVertexBuffer();

            var context = program.GraphicsContext;

            GL.BindVertexArray(this.vba);

            this.indecies.Bind();
            this.vertexBuffer.Bind();
            
            context[0] = texture;

            if (firstActiveParticle < firstFreeParticle)
            {
                context.UseShaderProgram(program);
                context.DrawElements(BeginMode.Triangles, (firstFreeParticle - firstActiveParticle) * ELEMENTS_PER_SQUARE, DrawElementsType.UnsignedInt,
                    firstActiveParticle * ELEMENTS_PER_SQUARE * sizeof(uint));

            }
            else
            {
                context.DrawElements(BeginMode.Triangles, (MaxParticles - firstActiveParticle) * ELEMENTS_PER_SQUARE, DrawElementsType.UnsignedInt,
                                   firstActiveParticle * ELEMENTS_PER_SQUARE * sizeof(uint));


                if (firstFreeParticle > 0)
                {
                    context.DrawElements(BeginMode.Triangles, firstFreeParticle * 6, DrawElementsType.UnsignedInt, 0);
                }
            }

            drawCounter++;
            GL.BindVertexArray(0);
        }


        unsafe void AddNewParticlesToVertexBuffer()
        {
            this.vertexBuffer.Bind();
      
            if (firstNewParticle < firstFreeParticle)
            {
                fixed (Particle* ptr = &this.queue[firstNewParticle * 4])
                {
                    vertexBuffer.SetSubData((IntPtr)ptr, firstNewParticle * 4, (firstFreeParticle - firstNewParticle) * 4, this.queue[0].SizeInBytes);
                }
            }
            else
            {
                fixed (Particle* ptr = &this.queue[firstNewParticle * 4])
                {
                    vertexBuffer.SetSubData((IntPtr)ptr, firstNewParticle * 4, (this.MaxParticles - firstNewParticle) * 4, this.queue[0].SizeInBytes);
                }
                                
                if (firstFreeParticle > 0)
                {
                    fixed (Particle* ptr = &this.queue[0])
                    {
                        vertexBuffer.SetSubData((IntPtr)ptr, 0,firstFreeParticle * 4, this.queue[0].SizeInBytes);
                    }
                }
            }

            firstNewParticle = firstFreeParticle;
        }
        
        public void AddParticle(Vector2 position, Vector2 velocity, Vector2 size, Vector4 coords)
        {
            int nextFreeParticle = (firstFreeParticle + 1) % this.MaxParticles;
            
            if (nextFreeParticle == firstRetiredParticle)
                return;


            queue[firstFreeParticle * 4].Coords = new Vector2(coords.X, coords.Y);
            queue[firstFreeParticle * 4 + 1].Coords = new Vector2(coords.Z, coords.Y);
            queue[firstFreeParticle * 4 + 2].Coords = new Vector2(coords.Z, coords.W);
            queue[firstFreeParticle * 4 + 3].Coords = new Vector2(coords.X, coords.W);

            queue[firstFreeParticle * 4 + 0].Offset = -size / 2;
            queue[firstFreeParticle * 4 + 1].Offset = new Vector2(size.X / 2, -size.Y / 2);
            queue[firstFreeParticle * 4 + 2].Offset = size / 2;
            queue[firstFreeParticle * 4 + 3].Offset = new Vector2(-size.X / 2, size.Y / 2); ;

            Random random = Examples.Particles.Random;

            Vector2 rand = new Vector2(-1 + (float)random.NextDouble() * 2, -1f + (float)random.NextDouble() * 2f);
            for (int i = 0; i < 4; i++)
            {
                queue[firstFreeParticle * 4 + i].Position = new Vector2(position.X, position.Y);
                queue[firstFreeParticle * 4 + i].Velocity = velocity;
                queue[firstFreeParticle * 4 + i].Time = currentTime;
                queue[firstFreeParticle * 4 + i].Random = rand;

            }

            firstFreeParticle = nextFreeParticle;
        }
    }
}