using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using OpenTK;

namespace Monocle.Graphics
{
    class PointBatch
    {        
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex : IVertex
        {
            public Vector2 Position;
            public Color Tint;

            public int SizeInBytes
            {
                get { return 12; }
            }
        }

        private IGraphicsContext context;
        
        private int elementCount, vba, maxSize;
        private Vertex[] vertecies;
        private VertexBuffer<Vertex> vertexBuffer;

        public PointBatch(IGraphicsContext context, int maxCapacity)
        {
            this.elementCount = 0;
            this.maxSize = maxCapacity;

            GL.GenVertexArrays(1, out this.vba);

            this.context = context;
            this.vertecies = new Vertex[maxCapacity];
            this.vertexBuffer = new VertexBuffer<Vertex>(context, OpenTK.Graphics.OpenGL.BufferUsageHint.StreamDraw);

            this.context.BindVertexBuffer(this.vertexBuffer);
            this.vertexBuffer.SetData(this.vertecies);
        }

        public unsafe void AddParticle(Vector2 position, Vector2 size, Color color)
        {
            this.vertecies[elementCount].Position = position;
            this.vertecies[elementCount].Tint = color;

            this.elementCount++;
            if (elementCount > maxSize)
                throw new ArgumentException();
            
        }

        public void Render(ref Matrix4 projection, ShaderProgram program)
        {
            this.SetupShaderProgram(program, ref projection);
            this.Flush();

            GL.BindVertexArray(this.vba);
            this.context.BindVertexBuffer(this.vertexBuffer);

            GL.DrawArrays(BeginMode.Points, 0, this.elementCount);
            this.elementCount = 0;
        }

        private void Flush()
        {
            this.context.BindVertexBuffer(this.vertexBuffer);
            this.vertexBuffer.SetSubData(this.vertecies, 0, this.elementCount);
        }

        private void SetupShaderProgram(ShaderProgram program, ref Matrix4 projection)
        {
            this.context.UseShaderProgram(program);
            program.SetUniform("projection_matrix", ref projection);

            int posIndex = program.GetAttributeLocation("in_position");
            int coloIndex = program.GetAttributeLocation("in_tint");

            GL.BindVertexArray(this.vba);

            this.context.BindVertexBuffer(this.vertexBuffer);
            this.context.EnableVertexAttribArray(posIndex);
            this.context.EnableVertexAttribArray(coloIndex);

            this.context.VertexAttribPointer(posIndex, 2, VertexAttribPointerType.Float, 
                                             true, this.vertecies[0].SizeInBytes, 0);

            this.context.VertexAttribPointer(coloIndex, 4, VertexAttribPointerType.UnsignedByte, 
                                             true, this.vertecies[0].SizeInBytes, Vector2.SizeInBytes);

            GL.BindVertexArray(0);
        } 
    }
}
