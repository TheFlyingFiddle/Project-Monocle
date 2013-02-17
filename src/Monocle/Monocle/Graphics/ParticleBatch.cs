using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Monocle.Graphics
{
    class ParticleBatch
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
        private IntIndexBuffer indexBuffer;

        public ParticleBatch(IGraphicsContext context, int maxCapacity)
        {
            this.elementCount = 0;
            this.maxSize = maxCapacity;

            GL.GenVertexArrays(1, out this.vba);

            this.context = context;
            this.vertecies = new Vertex[maxCapacity * 4];
            this.vertexBuffer = new VertexBuffer<Vertex>(context, OpenTK.Graphics.OpenGL.BufferUsageHint.StreamDraw);
            this.indexBuffer = new IntIndexBuffer(context, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
            this.SetIndexBufferSize(maxCapacity);

            this.context.BindVertexBuffer(this.vertexBuffer);
            this.vertexBuffer.SetData(this.vertecies);
        }

        public unsafe void AddParticle(Vector2 position, Vector2 size, Color color)
        {
            fixed (Vertex* ptr = &this.vertecies[elementCount * 4])
            {
                Vertex* ptr2 = ptr;

                ptr2->Position = new Vector2(position.X, position.Y);
                ptr2->Tint = color;

                ptr2++;

                ptr2->Position = new Vector2(position.X + size.X, position.Y);
                ptr2->Tint = color;

                ptr2++;

                ptr2->Position = new Vector2(position.X + size.X, position.Y + size.Y);
                ptr2->Tint = color;

                ptr2++;

                ptr2->Position = new Vector2(position.X, position.Y + size.Y);
                ptr2->Tint = color;

            }

            this.elementCount++;
            if (elementCount > maxSize)
                throw new ArgumentException();
            
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
            this.context.BindIndexBuffer(this.indexBuffer);
            this.indexBuffer.SetData(indecies);
        }

        public void Render(ref Matrix4 projection, ShaderProgram program)
        {
            this.SetupShaderProgram(program, ref projection);
            this.Flush();

            GL.BindVertexArray(this.vba);
            this.context.BindIndexBuffer(indexBuffer);
            this.context.BindVertexBuffer(this.vertexBuffer);

            this.context.DrawElements(BeginMode.Triangles, this.elementCount * 6, DrawElementsType.UnsignedInt, 0);

            this.elementCount = 0;
        }

        private void Flush()
        {
            this.context.BindVertexBuffer(this.vertexBuffer);
            this.vertexBuffer.SetSubData(this.vertecies, 0, this.elementCount * 4);
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