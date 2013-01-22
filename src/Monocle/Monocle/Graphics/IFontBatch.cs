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
    public class SpriteBatch
    {
        private const int Max_Sprites = 1024;
        private const int Elements_Per_Square = 6;
        private object _lock = new object();

        int vertexArrayHandle;
        int elementCount;
        bool hasBegun = false;

        VertexBuffer<Vertex> vertexBuffer;
        IntIndexBuffer indexBuffer;
        Effect default_effect;
        Effect activeEffect;

        Vertex[] vertices = new Vertex[Max_Sprites * 4];
        Vertex[] sortedVertecies = new Vertex[Max_Sprites * 4];
        
        Texture2D[] textures = new Texture2D[Max_Sprites];
        float[] renderOrder = new float[Max_Sprites];
        int[] sortedIndexes = new int[Max_Sprites];

        IComparer<int> comparer;

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex : IVertex
        {
            public Vector2 Position;
            public Vector2 TexCoords;
            public int Tint;
            public Vector2 Offset;
            public float Rotation;
            
            public int SizeInBytes { get { return 32; } }
        }


        public SpriteBatch(Effect effect)
        {
            if (effect == null)
                throw new ArgumentNullException("effect");

            this.default_effect = effect;
            InitIndices();
            InitBuffers();
            InitVao(this.default_effect);
        }

        private void InitIndices()
        {
            this.indexBuffer = new IntIndexBuffer(BufferUsageHint.StaticDraw);
            this.SetupIndices(Max_Sprites);
        }

        private unsafe void SetupIndices(int count)
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

        private void InitVao(Effect effect)
        {
            default_effect.Use();
            default_effect.SetUniform("tex", 0);

            GL.GenVertexArrays(1, out vertexArrayHandle);
            GL.BindVertexArray(vertexArrayHandle);

            int posIndex = GL.GetAttribLocation(effect.programID, "in_position");
            int texIndex = GL.GetAttribLocation(effect.programID, "in_coords");
            int coloIndex = GL.GetAttribLocation(effect.programID, "in_tint");
            int offsetIndex = GL.GetAttribLocation(effect.programID, "in_offset");
            int rotationIndex = GL.GetAttribLocation(effect.programID, "in_rotation");

            ErrorCode code = GL.GetError();

            GL.EnableVertexAttribArray(posIndex);
            this.vertexBuffer.Bind();
            GL.VertexAttribPointer(posIndex, 2, VertexAttribPointerType.Float, false, this.vertices[0].SizeInBytes, 0);

            GL.EnableVertexAttribArray(texIndex);
            GL.VertexAttribPointer(texIndex, 2, VertexAttribPointerType.Float, false, this.vertices[0].SizeInBytes, Vector2.SizeInBytes);

            GL.EnableVertexAttribArray(coloIndex);
            GL.VertexAttribPointer(coloIndex, 4, VertexAttribPointerType.UnsignedByte, true, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 2);

            GL.EnableVertexAttribArray(offsetIndex);
            GL.VertexAttribPointer(offsetIndex, 2, VertexAttribPointerType.Float, false, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 2 + sizeof(int));

            GL.EnableVertexAttribArray(rotationIndex);
            GL.VertexAttribPointer(rotationIndex, 1, VertexAttribPointerType.Float, false, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 3 + sizeof(int));




            this.indexBuffer.Bind();
            
            GL.BindVertexArray(0);
        }


        public void Begin(ref Matrix4 transformation, Effect effect = null, SortMode mode = SortMode.Deffered)
        {
            lock (_lock)
            {
                if (effect == null)
                    this.activeEffect = default_effect;
                else
                {
                    this.activeEffect = effect;
                    this.InitVao(this.activeEffect);
                }

                if (hasBegun)
                    throw new InvalidOperationException("Cannot begin twice.");

                activeEffect.Use();
                activeEffect.SetUniform("projection_matrix", ref transformation);


                switch (mode)
                {
                    case SortMode.Deffered:
                        this.comparer = null;
                        break;
                    case SortMode.RenderOrder:
                        this.comparer = new RenderOrderSorter(this);
                        break;
                    case SortMode.Texture:
                        this.comparer = new TextureSorter(this);
                        break;
                }

                GL.UseProgram(0);
                this.hasBegun = true;
            }
        }

        public void DrawString(TextureFont fontUsed, string toDraw, Vector2 position)
        {
            this.DrawString(fontUsed, toDraw, position, Color4.White, Vector2.Zero, Vector2.One, 0, false);
        }

        public void DrawString(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color)
        {
            this.DrawString(fontUsed, toDraw, position, color, Vector2.Zero, Vector2.One, 0, false);
        }

        public void DrawString(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin)
        {
            this.DrawString(fontUsed, toDraw, position, color, origin, Vector2.One, 0, false);
        }

        public void DrawString(TextureFont font, StringBuilder toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f)
        {
            this.DrawString(font, toDraw.ToString(), position, color, origin, scale, angle, mirror, renderLayer);
        }

        public void DrawString(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f)
        {
            if (!this.hasBegun)
                throw new ArgumentException("Cannot draw before begin has been called!");
            if (fontUsed == null || toDraw == null)
                throw new ArgumentNullException();

            Vector2 cursor = Vector2.Zero;
            int tint = color.ToArgb();

            lock (_lock)
            {
                unsafe
                {
                    fixed (char* ptr = toDraw)
                    {

                        if (!mirror)
                        {

                            for (int i = 0; i < toDraw.Length; i++)
                            {
                                if (this.elementCount >= this.textures.Length)
                                {
                                    this.Resize();
                                }

                                char c = ptr[i];
                                DrawChar(fontUsed, ref position, ref origin, ref scale, ref cursor, false, tint, angle, renderLayer, c);
                            }
                        }
                        else
                        {
                            for (int i = toDraw.Length - 1; i >= 0; i--)
                            {
                                if (this.elementCount >= this.vertices.Length)
                                {
                                    this.Resize();
                                }

                                char c = ptr[i];
                                DrawChar(fontUsed, ref position, ref origin, ref scale, ref cursor, true, tint, angle, renderLayer, c);
                            }
                        }
                    }
                }
            }
        }

        private void Resize()
        {
     //       if (this.vertices.Length >= 0xFFFF)
     //           throw new ArgumentException("The sprite batch is full! Cannot draw more then 16384 sprites in one draw call");


            Array.Resize<Vertex>(ref this.vertices, this.vertices.Length * 2);
            Array.Resize<Vertex>(ref this.sortedVertecies, this.sortedVertecies.Length * 2);
            Array.Resize<int>(ref this.sortedIndexes, this.sortedIndexes.Length * 2);
            Array.Resize<Texture2D>(ref this.textures, this.textures.Length * 2);
            Array.Resize<float>(ref this.renderOrder, this.renderOrder.Length * 2);
            
            this.vertexBuffer.Bind();
            this.vertexBuffer.SetData(this.vertices);
            this.SetupIndices(this.textures.Length);
        }

        private void DrawChar(TextureFont fontUsed, ref Vector2 position, ref Vector2 origin, ref Vector2 scale, ref Vector2 cursor, bool mirror, int tint, float angle, float renderLayer, char c)
        {
            if (c == '\r') return;

            if (c == '\n')
            {
                cursor.Y += fontUsed.LineHeight * scale.Y;
                cursor.X = 0;
                return;
            }
            else if (c == '\t')
            {
                CharInfo ci = fontUsed[' '];
                cursor.X += ci.Advance * 4 * scale.X;
                return;
            }

            CharInfo info = fontUsed[c];

            if (c == ' ')
            {
                cursor.X += info.Advance * scale.X;
                return;
            }

            if (info == null)
            {
                info = fontUsed['\u00A5'];
            }

            Vector4 src = info.TextureCoords;

            if (mirror)
            {
                float tmp = src.X;
                src.X = src.Z;
                src.Z = tmp;
            }

            Vector2 infoOffset = info.Offset;

            Vector4 dest;
            dest.X = (cursor.X + (infoOffset.X - origin.X) * scale.X);
            dest.Y = (cursor.Y + (infoOffset.Y - origin.Y) * scale.Y);
            dest.Z = dest.X + info.SrcRect.Width * scale.X;
            dest.W = dest.Y + info.SrcRect.Height * scale.Y;
            
            this.CreateRect(ref dest, ref src, ref position, tint, angle);

            cursor.X += info.Advance * scale.X;

            this.textures[this.elementCount] = fontUsed.Page;
            this.renderOrder[this.elementCount++] = renderLayer;

            return;
        }

        private unsafe void CreateRect(ref Vector4 dest, ref Vector4 src, ref Vector2 pos, int tint, float angle)
        {
            fixed (Vertex* fixedPtr = &this.vertices[this.elementCount * 4])
            {
                var ptr = fixedPtr;

                ptr->Position.X = pos.X;
                ptr->Position.Y = pos.Y;
                ptr->Offset.X = dest.X;
                ptr->Offset.Y = dest.Y;
                ptr->TexCoords.X = src.X;
                ptr->TexCoords.Y = src.Y;
                ptr->Rotation = angle;
                ptr->Tint = tint;

                ++ptr;

                ptr->Position.X = pos.X;
                ptr->Position.Y = pos.Y;
                ptr->Offset.X = dest.Z;
                ptr->Offset.Y = dest.Y;
                ptr->TexCoords.X = src.Z;
                ptr->TexCoords.Y = src.Y;
                ptr->Rotation = angle;
                ptr->Tint = tint;

                ++ptr;

                ptr->Position.X = pos.X;
                ptr->Position.Y = pos.Y;
                ptr->Offset.X = dest.Z;
                ptr->Offset.Y = dest.W;
                ptr->TexCoords.X = src.Z;
                ptr->TexCoords.Y = src.W;
                ptr->Rotation = angle;
                ptr->Tint = tint;

                ++ptr;

                ptr->Position.X = pos.X;
                ptr->Position.Y = pos.Y;
                ptr->Offset.X = dest.X;
                ptr->Offset.Y = dest.W;
                ptr->TexCoords.X = src.X;
                ptr->TexCoords.Y = src.W;
                ptr->Rotation = angle;
                ptr->Tint = tint;
            }
        }

                                         
        
        public void DrawFrame(Frame frame, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float rotation = 0.0f, bool mirror = false, float renderLayer = 0)
        {
            lock (_lock)
            {
                if (!this.hasBegun)
                    throw new ArgumentException("Cannot draw before begin has been called!");
                if (frame.Texture2D == null)
                    throw new ArgumentNullException();



                Vector4 src = frame.SrcRect.ToVector4();
                src.X /= frame.Texture2D.Width;
                src.Z /= frame.Texture2D.Width;
                src.Y /= frame.Texture2D.Height;
                src.W /= frame.Texture2D.Height;

                if (mirror)
                {
                    float tmp = src.X;
                    src.X = src.Z;
                    src.Z = tmp;
                }

                Vector4 dest;
                dest.X = -origin.X * scale.X;
                dest.Y = -origin.Y * scale.Y;
                dest.Z = dest.X + frame.SrcRect.Width * scale.X;
                dest.W = dest.Y + frame.SrcRect.Height * scale.Y;



                this.CreateRect(ref dest, ref src, ref position, color.ToArgb(), rotation);

                this.textures[this.elementCount] = frame.Texture2D;
                this.renderOrder[this.elementCount++] = renderLayer;
            }
        }


        public void DrawFrame(Frame frame, Rect position, Color4 color, float rotation = 0, bool mirror = false)
        {
            this.DrawFrame(frame, new Vector2(position.X, position.Y), color, Vector2.Zero, new Vector2((position.Width / frame.SrcRect.Width), position.Height / frame.SrcRect.Height), rotation, mirror);
        }

        public void DrawFrame(Frame frame, Rect position, Color4 color)
        {
            this.DrawFrame(frame, new Vector2(position.X, position.Y), color, Vector2.Zero, new Vector2((position.Width / frame.SrcRect.Width), position.Height / frame.SrcRect.Height), 0, false);
        }

        public void DrawFrame(Frame frame, Vector2 position, Color4 color)
        {
            this.DrawFrame(frame, position, color, Vector2.Zero, Vector2.One, 0f, false);
        }


        public void End()
        {
            lock (_lock)
            {
                if (!hasBegun)
                    throw new InvalidOperationException("Cannot end befoure we have begun.");

                this.Flush();
                this.Render();

                this.hasBegun = false;
            }
        }

        private void Flush()
        {
            this.Sort();
            this.vertexBuffer.Bind();
            this.vertexBuffer.SetSubData(this.sortedVertecies, 0, this.elementCount * 4);
        }

        private void Render()
        {
            this.activeEffect.Use();
            GL.BindVertexArray(this.vertexArrayHandle);
            
            int toDraw = 0;
            var toUse = textures[this.sortedIndexes[0]];
            for (int i = 0; i < this.elementCount; i++)
            {
                var index = this.sortedIndexes[i];

                if (toUse != textures[index])
                {
                    toUse.Bind(TextureUnit.Texture0);
                    GL.DrawElements(BeginMode.Triangles, toDraw * Elements_Per_Square, DrawElementsType.UnsignedInt, (i - toDraw) * Elements_Per_Square * sizeof(uint));
                    toDraw = 0;

                    toUse = textures[index];
                }
                toDraw++;
            }
            toUse.Bind(TextureUnit.Texture0);
            GL.DrawElements(BeginMode.Triangles, toDraw * Elements_Per_Square, DrawElementsType.UnsignedInt, (elementCount - toDraw) * Elements_Per_Square * sizeof(uint));
       
            
            GL.BindVertexArray(0);
            this.elementCount = 0;
        }

        private unsafe void Sort()
        {
            fixed (int* ptr = this.sortedIndexes)
            {
                fixed (Vertex* queue = this.vertices, toSort = this.sortedVertecies)
                {
                    for (int i = 0; i < this.elementCount; i++)
                    {
                        ptr[i] = i;
                    }

                    if(this.comparer != null)
                        Array.Sort<int>(this.sortedIndexes, 0, this.elementCount, this.comparer);

                    for (int i = 0; i < this.elementCount; i++)
                    {
                        var index = ptr[i] * 4;
                        var index1 = i * 4;
                        toSort[index1] = queue[index];
                        toSort[index1 + 1] = queue[index + 1];
                        toSort[index1 + 2] = queue[index + 2];
                        toSort[index1 + 3] = queue[index + 3];
                    }
                }
            }
        }

        private class RenderOrderSorter : IComparer<int>
        {
            private readonly SpriteBatch parent;

            public RenderOrderSorter(SpriteBatch batch)
            {
                this.parent = batch;
            }

            public int Compare(int x, int y)
            {
                float left = this.parent.renderOrder[x];
                float right = this.parent.renderOrder[y];

                if (left < right) return -1;
                else if (left > right) return 1;
                return 0;
            }
        }

        private class TextureSorter : IComparer<int>
        {
            private readonly SpriteBatch parent;

            public TextureSorter(SpriteBatch batch)
            {
                this.parent = batch;
            }

            public int Compare(int x, int y)
            {
                var left = this.parent.textures[x];
                var right = this.parent.textures[y];

                return left.CompareTo(right); 
            }
        }

        private class DefferedSorter : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x - y;
            }
        }

    }
}