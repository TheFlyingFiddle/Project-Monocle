using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Monocle.Graphics
{
    public interface IFontBatch
    {
        
    }

    public class FontBatch
    {
        private const int Max_Sprites = 4096;

        int vertexArrayHandle;
        int charCount;
        bool hasBegun = false;

        VertexBuffer<Vertex> vertexBuffer;
        ShortIndexBuffer indexBuffer;
        Effect effect;

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex : IVertex
        {
            public Vector2 Position;
            public Vector2 TexCoords;
            public int Tint;

            public int SizeInBytes { get { return Vector2.SizeInBytes + Vector2.SizeInBytes + sizeof(int); } }
        }

        Vertex[] vertices = new Vertex[Max_Sprites * 4];
        TextureFont[] fonts = new TextureFont[Max_Sprites];

        public FontBatch(Effect effect)
        {
            if (effect == null)
                throw new ArgumentNullException("effect");

            this.effect = effect;
            InitEffect();
            InitIndices();
            InitBuffers();
            InitVao();
        }

        private void InitEffect()
        {
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

            this.indexBuffer = new ShortIndexBuffer(BufferUsageHint.StaticDraw);
            this.indexBuffer.Bind();
            this.indexBuffer.SetData(indecies);
        }

        private void InitBuffers()
        {
            this.vertexBuffer = new VertexBuffer<Vertex>(BufferUsageHint.StreamDraw);
            this.vertexBuffer.Bind();
            this.vertexBuffer.SetData(this.vertices);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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
            this.vertexBuffer.Bind();
            GL.VertexAttribPointer(posIndex, 2, VertexAttribPointerType.Float, false, this.vertices[0].SizeInBytes, 0);

            GL.EnableVertexAttribArray(texIndex);
            GL.VertexAttribPointer(texIndex, 2, VertexAttribPointerType.Float, false, this.vertices[0].SizeInBytes, Vector2.SizeInBytes);

            GL.EnableVertexAttribArray(coloIndex);
            GL.VertexAttribPointer(coloIndex, 4, VertexAttribPointerType.UnsignedByte, true, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 2);

            this.indexBuffer.Bind();
            
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

        public void Draw(TextureFont fontUsed, string toDraw, Vector2 position)
        {
            this.Draw(fontUsed, toDraw, position, Color4.White, Vector2.Zero, Vector2.One, 0, false);
        }

        public void Draw(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color)
        {
            this.Draw(fontUsed, toDraw, position, color, Vector2.Zero, Vector2.One, 0, false);
        }

        public void Draw(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin)
        {
            this.Draw(fontUsed, toDraw, position, color, origin, Vector2.One, 0, false);
        }

        public void Draw(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float rotation = 0, bool mirror = false)
        {
            if (mirror)
                DrawBackwards(fontUsed, toDraw, position, color, origin, scale, rotation);
            else
                DrawForward(fontUsed, toDraw, position, color, origin, scale, rotation);
        }


        private void DrawForward(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float rotation = 0)
        {
            float sin = (float)Math.Sin(rotation);
            float cos = (float)Math.Cos(rotation);
            
            Vector2 cursor = Vector2.Zero;
            for (int i = 0; i < toDraw.Length; i++)
            {
                char c = toDraw[i];

                if (c == '\n')
                {
                    cursor.Y += fontUsed.LineHeight * scale.Y;
                    cursor.X = 0;
                    continue;
                }
                else if (c == '\t')
                {
                    CharInfo ci = fontUsed[' '];
                    cursor.X += ci.Advance * 4 * scale.X;
                    continue;
                }

                CharInfo info = fontUsed[c];
                
                if (c == ' ')
                {
                    cursor.X += info.Advance * scale.X;
                    continue;
                }

                if (info == null)
                {
                    info = fontUsed['\u00A5'];
                }

                Vector4 src = info.SrcRect.ToVector4();
                src.X /= (fontUsed.Page.Width);
                src.Z /= (fontUsed.Page.Width);
                src.Y /= (fontUsed.Page.Height);
                src.W /= (fontUsed.Page.Height);

                int tint = color.ToArgb();


                float leftOffset = cursor.X + (info.Offset.X - origin.X) * scale.X;
                float topOffset = cursor.Y + (info.Offset.Y - origin.Y) * scale.Y;

                float rightOffset = leftOffset + info.SrcRect.Width * scale.X;
                float bottomOffset = topOffset + info.SrcRect.Height * scale.Y;

                unsafe
                {
                    fixed (Vertex* fixedPtr = &this.vertices[this.charCount * 4])
                    {
                        var ptr = fixedPtr;

                        Vector2 dest;
                        dest.X = leftOffset * cos - topOffset * sin + position.X;
                        dest.Y = leftOffset * sin + topOffset * cos + position.Y;

                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.X, src.Y);
                        ptr->Tint = tint;

                        ++ptr;

                        dest.X = rightOffset * cos - topOffset * sin + position.X;
                        dest.Y = rightOffset * sin + topOffset * cos + position.Y;

                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.Z, src.Y);
                        ptr->Tint = tint;

                        ++ptr;

                        dest.X = rightOffset * cos - bottomOffset * sin + position.X;
                        dest.Y = rightOffset * sin + bottomOffset * cos + position.Y;


                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.Z, src.W);
                        ptr->Tint = tint;

                        ++ptr;

                        dest.X = leftOffset * cos - bottomOffset * sin + position.X;
                        dest.Y = leftOffset * sin + bottomOffset * cos + position.Y;

                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.X, src.W);
                        ptr->Tint = tint;
                    }
                }

                cursor.X += info.Advance * scale.X;

                this.fonts[this.charCount] = fontUsed;
                this.charCount++;
            }
        }


        private void DrawBackwards(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float rotation = 0)
        {
            float sin = (float)Math.Sin(rotation);
            float cos = (float)Math.Cos(rotation);

            Vector2 cursor = Vector2.Zero;
            for (int i = toDraw.Length - 1; i >= 0; i--)
            {   
                char c = toDraw[i];

                if (c == '\n')
                {
                    cursor.Y += fontUsed.LineHeight * scale.Y;
                    cursor.X = 0;
                    continue;
                }
                else if (c == '\t')
                {
                    CharInfo ci = fontUsed[' '];
                    cursor.X += ci.Advance * 4 * scale.X;
                    continue;
                }

                CharInfo info = fontUsed[c];

                if (c == ' ')
                {
                    cursor.X += info.Advance * scale.X;
                    continue;
                }

                Vector4 src = info.SrcRect.ToVector4();
                src.X /= (fontUsed.Page.Width);
                src.Z /= (fontUsed.Page.Width);
                src.Y /= (fontUsed.Page.Height);
                src.W /= (fontUsed.Page.Height);

                int tint = color.ToArgb();


                float leftOffset = cursor.X + (info.Offset.X - origin.X) * scale.X;
                float topOffset = cursor.Y + (info.Offset.Y - origin.Y) * scale.Y;

                float rightOffset = leftOffset + info.SrcRect.Width * scale.X;
                float bottomOffset = topOffset + info.SrcRect.Height * scale.Y;

                unsafe
                {
                    fixed (Vertex* fixedPtr = &this.vertices[this.charCount * 4])
                    {
                        var ptr = fixedPtr;

                        Vector2 dest;
                        dest.X = leftOffset * cos - topOffset * sin + position.X;
                        dest.Y = leftOffset * sin + topOffset * cos + position.Y;

                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.Z, src.Y);
                        ptr->Tint = tint;

                        ++ptr;

                        dest.X = rightOffset * cos - topOffset * sin + position.X;
                        dest.Y = rightOffset * sin + topOffset * cos + position.Y;

                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.X, src.Y);
                        ptr->Tint = tint;

                        ++ptr;

                        dest.X = rightOffset * cos - bottomOffset * sin + position.X;
                        dest.Y = rightOffset * sin + bottomOffset * cos + position.Y;


                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.X, src.W);
                        ptr->Tint = tint;

                        ++ptr;

                        dest.X = leftOffset * cos - bottomOffset * sin + position.X;
                        dest.Y = leftOffset * sin + bottomOffset * cos + position.Y;

                        ptr->Position = dest;
                        ptr->TexCoords = new Vector2(src.Z, src.W);
                        ptr->Tint = tint;
                    }
                }

                cursor.X += info.Advance * scale.X;

                this.fonts[this.charCount] = fontUsed;
                this.charCount++;
            }
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
            this.vertexBuffer.Bind();
            this.vertexBuffer.SetSubData(this.vertices, 0, this.charCount * 4);
        }

        private void Render()
        {                    
            effect.Use();
            GL.BindVertexArray(this.vertexArrayHandle);
            
            int toDraw = 0;
            TextureFont toUse = fonts[0];
            for (int i = 0; i < this.charCount; i++)
            {
                if (toUse != fonts[i])
                {
                    toUse.Page.Bind(TextureUnit.Texture0);
                    GL.DrawElements(BeginMode.Triangles, toDraw * 6, DrawElementsType.UnsignedShort, (i - toDraw) * 6 * sizeof(ushort));
                    toDraw = 0;

                    toUse = fonts[i];
                }
                toDraw++;
            }
            toUse.Page.Bind(TextureUnit.Texture0);
            GL.DrawElements(BeginMode.Triangles, toDraw * 6, DrawElementsType.UnsignedShort, (charCount - toDraw) * 6 * sizeof(ushort));
       
            
            GL.BindVertexArray(0);
            this.charCount = 0;
        }
    }
}