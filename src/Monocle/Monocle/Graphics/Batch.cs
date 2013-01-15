using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Monocle.Graphics
{
    public class Batch
    {
        const int Max_Sprites = 1024;

        int vertexArrayHandle,
            vertexBufferHandle,
            indexBufferHandle;

        int spriteCount;

        bool hasBegun = false;


        Effect effect;

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            public Vector2 Position;
            public Vector2 TexCoords;
            public int Tint;

            public static int SizeInBytes = Vector2.SizeInBytes + Vector2.SizeInBytes + sizeof(int);
        }

        Vertex[] vertices = new Vertex[Max_Sprites * 4];
        Texture2D[] textures = new Texture2D[Max_Sprites];

        public Batch()
        {
            InitEffect();
            InitIndices();
            InitBuffers();
            InitVao();
        }

        private void InitEffect()
        {
            effect = Effect.CreateEffect(BasicEffect.VertexShaderSource, BasicEffect.FragmentShaderSource);
            effect.Use();
            effect.SetUniform("tex", 0);

            GL.UseProgram(0);
        }

        private void InitIndices()
        {
            short[] indecies = new short[Max_Sprites * 6];
            for (int i = 0; i < Max_Sprites; i++)
            {
                indecies[i * 6] = (short)(i * 4);
                indecies[i * 6 + 1] = (short)(i * 4 + 1);
                indecies[i * 6 + 2] = (short)(i * 4 + 2);
                indecies[i * 6 + 3] = (short)(i * 4);
                indecies[i * 6 + 4] = (short)(i * 4 + 2);
                indecies[i * 6 + 5] = (short)(i * 4 + 3);
            }

            GL.GenBuffers(1, out indexBufferHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                new IntPtr(sizeof(short) * indecies.Length),
                indecies, BufferUsageHint.StaticDraw);
        }

        private void InitBuffers()
        {
            GL.GenBuffers(1, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);

            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, new IntPtr(this.vertices.Length * Vertex.SizeInBytes),
                                  this.vertices, BufferUsageHint.StreamDraw);


            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void InitVao()
        {
            GL.GenVertexArrays(1, out vertexArrayHandle);
            GL.BindVertexArray(vertexArrayHandle);

            ErrorCode error = GL.GetError();

            int posIndex = GL.GetAttribLocation(effect.programID, "in_position");
            int texIndex = GL.GetAttribLocation(effect.programID, "in_coords");
            int coloIndex = GL.GetAttribLocation(effect.programID, "in_tint");

            GL.EnableVertexAttribArray(posIndex);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(posIndex, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);

            GL.EnableVertexAttribArray(texIndex);
            GL.VertexAttribPointer(texIndex, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector2.SizeInBytes);

            GL.EnableVertexAttribArray(coloIndex);
            GL.VertexAttribPointer(coloIndex, 4, VertexAttribPointerType.UnsignedByte, true, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);

            GL.BindVertexArray(0);
        }


        public void Begin(ref Matrix4 transformation)
        {
            if (hasBegun)
                throw new InvalidOperationException("Cannot begin twice.");

            effect.Use();
            effect.SetUniform("projection_matrix", ref transformation);

            GL.UseProgram(0);

            this.hasBegun = true;
        }

        public void Draw(Texture2D texture, Vector4 destination, Color4 color)
        {
            this.vertices[this.spriteCount * 4].Position = new Vector2(destination.X, destination.Y);
            this.vertices[this.spriteCount * 4 + 1].Position = new Vector2(destination.Z, destination.Y);
            this.vertices[this.spriteCount * 4 + 2].Position = new Vector2(destination.Z, destination.W);
            this.vertices[this.spriteCount * 4 + 3].Position = new Vector2(destination.X, destination.W);


            this.vertices[this.spriteCount * 4].Tint = color.ToArgb();
            this.vertices[this.spriteCount * 4 + 1].Tint = color.ToArgb();
            this.vertices[this.spriteCount * 4 + 2].Tint = color.ToArgb();
            this.vertices[this.spriteCount * 4 + 3].Tint = color.ToArgb();

            this.vertices[this.spriteCount * 4].TexCoords = new Vector2(0, 0);
            this.vertices[this.spriteCount * 4 + 1].TexCoords = new Vector2(1, 0);
            this.vertices[this.spriteCount * 4 + 2].TexCoords = new Vector2(1, 1);
            this.vertices[this.spriteCount * 4 + 3].TexCoords = new Vector2(0, 1);

            this.textures[spriteCount] = texture;
            this.spriteCount += 1;
        }

        public void Draw(Texture2D texture, Vector2 position)
        {
            Vector4 dest;
            dest.X = position.X - texture.Width / 2;
            dest.Y = position.Y - texture.Height / 2;
            dest.Z = position.X + texture.Width / 2;
            dest.W = position.Y + texture.Height / 2;

            this.Draw(texture, dest, Color4.White);
        }

        public void Draw(Texture2D texture, Rect position)
        {
            this.Draw(texture, position.ToVector4(), Color4.White);
        }

        public void Draw(Texture2D texture2D, Rect position, Color4 color4, Rect srcRect)
        {
            //TODO fix.
            this.Draw(texture2D, position.ToVector4(), color4);
        }
        public void Draw(Texture2D texture2D, Vector2 position, Color4 color4, Rect rect)
        {
            //TODO fix.
            this.Draw(texture2D, position);
        }


        public void End()
        {
            if (!hasBegun)
                throw new InvalidOperationException("Cannot end befoure we have begun.");

            this.Flush();
            this.Render();

            this.hasBegun = false;
        }

        private void Flush()
        {
            GL.BindVertexArray(this.vertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.BufferSubData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)0, new IntPtr(this.spriteCount * 4 * Vertex.SizeInBytes), this.vertices);
            GL.BindVertexArray(0);
        }

        private void Render()
        {                    
            effect.Use();
            GL.BindVertexArray(this.vertexArrayHandle);
            
            int toDraw = 0;
            Texture2D toUse = textures[0];
            for (int i = 0; i < this.spriteCount; i++)
            {
                if (toUse != textures[i])
                {
                    toUse.Bind(TextureUnit.Texture0);
                    GL.DrawElements(BeginMode.Triangles, toDraw * 6, DrawElementsType.UnsignedShort, (i - toDraw) * 6 * sizeof(ushort));
                    toDraw = 0;

                    toUse = textures[i];
                }
                toDraw++;
            }
            toUse.Bind(TextureUnit.Texture0);

            GL.DrawElements(BeginMode.Triangles, toDraw * 6, DrawElementsType.UnsignedShort, (spriteCount - toDraw) * 6 * sizeof(ushort));
 
            this.spriteCount = 0;
        }
    }
}