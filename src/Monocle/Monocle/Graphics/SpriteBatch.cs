using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;

using VertexAttribPointerType = OpenTK.Graphics.OpenGL.VertexAttribPointerType;
using TextureUnit = OpenTK.Graphics.OpenGL.TextureUnit;
using BeginMode = OpenTK.Graphics.OpenGL.BeginMode;
using BufferUsageHint = OpenTK.Graphics.OpenGL.BufferUsageHint;
using DrawElementsType = OpenTK.Graphics.OpenGL.DrawElementsType;
using BufferTarget = OpenTK.Graphics.OpenGL.BufferTarget;
using OpenTK.Graphics.OpenGL;


namespace Monocle.Graphics
{
    /// <summary>
    /// A class that enables fast dynamic drawing of text and frames.
    /// <remarks>This class is NOT thread safe, ONLY use the rendering thread when interacting with this class.</remarks>
    /// </summary>
    public class SpriteBuffer : Monocle.Graphics.ISpriteBuffer
    {
        private readonly Monocle.Graphics.IGraphicsContext graphicsContext;

        private const int Max_Sprites = 1024;
        private const int Elements_Per_Square = 6;
        private int vertexArrayHandle;

        //Current number of items (text character or frame) that is currently in the batch.
        protected int elementCount;

        //The object used to send vertecies to the graphics card.
        private VertexBuffer<Vertex> vertexBuffer;
        
        //The object used to send index data to the graphics card.
        private IntIndexBuffer indexBuffer;

        //The default effect that will be used if a user does not provide a custom effect.
        private ShaderProgram default_effect;

        //The effect that is currently in use.
        private ShaderProgram activeEffect;

        //Vertecies that are to be sent to the graphics card.
        private Vertex[] vertices = new Vertex[Max_Sprites * 4];
        private Vertex[] sortedVertecies = new Vertex[Max_Sprites * 4];
  
        //Texture used by the diffrent vertices elements.
        protected Texture2D[] textures = new Texture2D[Max_Sprites];
                
        //Order that verices are to be rendered. IF SortMode.RenderOrder is used.
        protected float[] renderOrder = new float[Max_Sprites];
        
        //Used to sort verticies.
        private int[] sortedIndexes = new int[Max_Sprites];

        //Vertex format used by the vertex shader.
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex : IVertex
        {
            public Vector2 Position;
            public Vector2 TexCoords;
            public int Tint;
            public Vector2 Offset;
            public float Rotation;
            
            public int SizeInBytes { get { return 32; } }
        }


        /// <summary>
        /// Creates a spritebatch that with the default effect provided.
        /// </summary>
        /// <remarks>The effect must use a vertex shader that uses the vertex format <see cref="SpriteBatch.Vertex"/></remSarks>
        /// <param name="effect">A effect.</param>
        public SpriteBuffer(Monocle.Graphics.IGraphicsContext context, ShaderProgram effect)
        {
            if (effect == null)
                throw new ArgumentNullException("effect");

            this.graphicsContext = context;
            this.default_effect = effect;
            InitIndices();
            InitBuffers();
        }

        private void InitIndices()
        {
            this.indexBuffer = new IntIndexBuffer(this.GraphicsContext, BufferUsageHint.StaticDraw);
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
            this.GraphicsContext.BindIndexBuffer(this.indexBuffer);
            this.indexBuffer.SetData(indecies);
        }

        private void InitBuffers()
        {
            this.vertexBuffer = new VertexBuffer<Vertex>(this.GraphicsContext, BufferUsageHint.StreamDraw);
            this.GraphicsContext.BindVertexBuffer(this.vertexBuffer);
            this.vertexBuffer.SetData(this.vertices);

            this.GraphicsContext.BindVertexBuffer(null);

            
            GL.GenVertexArrays(1, out this.vertexArrayHandle);
        }

        //Prepares the supplied effect for drawing.
        private void PrepareShaderProgram(ref Matrix4 transformation, ShaderProgram program)
        {
            this.GraphicsContext.UseShaderProgram(program);
            program.SetUniform("tex", 0);
            activeEffect.SetUniform("projection_matrix", ref transformation);


            int posIndex = this.GraphicsContext.GetAttribLocation(program.Handle, "in_position");
            int texIndex = this.GraphicsContext.GetAttribLocation(program.Handle, "in_coords");
            int coloIndex = this.GraphicsContext.GetAttribLocation(program.Handle, "in_tint");
            int offsetIndex = this.GraphicsContext.GetAttribLocation(program.Handle, "in_offset");
            int rotationIndex = this.GraphicsContext.GetAttribLocation(program.Handle, "in_rotation");

            GL.BindVertexArray(this.vertexArrayHandle);

            this.GraphicsContext.BindVertexBuffer(this.vertexBuffer);
            this.GraphicsContext.EnableVertexAttribArray(posIndex);
            this.GraphicsContext.EnableVertexAttribArray(texIndex);
            this.GraphicsContext.EnableVertexAttribArray(coloIndex);
            this.GraphicsContext.EnableVertexAttribArray(offsetIndex);
            this.GraphicsContext.EnableVertexAttribArray(rotationIndex);

            this.GraphicsContext.VertexAttribPointer(posIndex, 2, VertexAttribPointerType.Float, true, this.vertices[0].SizeInBytes, 0);
            this.GraphicsContext.VertexAttribPointer(texIndex, 2, VertexAttribPointerType.Float, true, this.vertices[0].SizeInBytes, Vector2.SizeInBytes);
            this.GraphicsContext.VertexAttribPointer(coloIndex, 4, VertexAttribPointerType.UnsignedByte, true, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 2);
            this.GraphicsContext.VertexAttribPointer(offsetIndex, 2, VertexAttribPointerType.Float, true, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 2 + sizeof(int));
            this.GraphicsContext.VertexAttribPointer(rotationIndex, 1, VertexAttribPointerType.Float, true, this.vertices[0].SizeInBytes, Vector2.SizeInBytes * 3 + sizeof(int));

        }

        public Monocle.Graphics.IGraphicsContext GraphicsContext
        {
            get { return this.graphicsContext; }
        }


        /// <summary>
        /// Adds a string to be drawn in the next batch draw call.
        /// </summary>
        /// <param name="fontUsed">The font to use.</param>
        /// <param name="toDraw">The string to be drawn.</param>
        /// <param name="position">The upper left corner position of the string.</param>
        /// <param name="color">The color to draw with.</param>
        public void BufferString(Font fontUsed, string toDraw, Vector2 position, Color4 color)
        {
            this.BufferString(fontUsed, toDraw, position, color, Vector2.Zero, Vector2.One, 0, false);
        }

        /// <summary>
        /// Adds a string to be drawn in the next batch draw call.
        /// </summary>
        /// <param name="fontUsed">The font to use.</param>
        /// <param name="toDraw">The string to be drawn.</param>
        /// <param name="position">The upper left corner position of the string.</param>
        /// <param name="color">The color to draw with.</param>
        /// <param name="origin">The origin point used.</param>
        public void BufferString(Font fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin)
        {
            this.BufferString(fontUsed, toDraw, position, color, origin, Vector2.One, 0, false);
        }

        /// <summary>
        /// Adds a string to be drawn in the next batch draw call.
        /// </summary>
        /// <param name="fontUsed">The font to use.</param>
        /// <param name="toDraw">The text to be drawn.</param>
        /// <param name="position">The upper left corner position of the text.</param>
        /// <param name="color">The color to draw with.</param>
        /// <param name="origin">The origin point used.</param>
        /// <param name="scale">The scale of the text.</param>
        /// <param name="angle">The angle the text should be totated.</param>
        /// <param name="mirror">A fralg indicating if the text should be mirrored or not.</param>
        /// <param name="renderLayer">The layer in with to render the text. (used together with SortMode.RenderOrder to render objects in specific layers)</param>
        public void BufferString(Font font, StringBuilder toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f)
        {
            this.BufferString(font, toDraw.ToString(), position, color, origin, scale, angle, mirror, renderLayer);
        }


        /// <summary>
        /// Adds a string to be drawn in the next batch draw call.
        /// </summary>
        /// <param name="fontUsed">The font to use.</param>
        /// <param name="toDraw">The text to be drawn.</param>
        /// <param name="position">The upper left corner position of the text.</param>
        /// <param name="color">The color to draw with.</param>
        /// <param name="origin">The origin point used.</param>
        /// <param name="scale">The scale of the text.</param>
        /// <param name="angle">The angle the text should be totated.</param>
        /// <param name="mirror">A fralg indicating if the text should be mirrored or not.</param>
        /// <param name="renderLayer">The layer in with to render the text. (used together with SortMode.RenderOrder to render objects in specific layers)</param>
        public void BufferString(Font fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f)
        {
            if (fontUsed == null || toDraw == null)
                throw new ArgumentNullException();

            Vector2 cursor = new Vector2(-origin.X * scale.X, -origin.Y * scale.Y);
            int tint = color.ToArgb();
            unsafe
            {
                if (this.elementCount + toDraw.Length >= this.textures.Length)
                    this.Resize();


                fixed (char* ptr = toDraw)
                {
                    if (!mirror)
                    {
                        for (int i = 0; i < toDraw.Length; i++)
                        {
                            char c = ptr[i];
                            BufferChar(fontUsed, ref position, ref origin, ref scale, ref cursor, false, tint, angle, renderLayer, c);
                        }
                    }
                    else
                    {
                        for (int i = toDraw.Length - 1; i >= 0; i--)
                        {
                            char c = ptr[i];
                            BufferChar(fontUsed, ref position, ref origin, ref scale, ref cursor, true, tint, angle, renderLayer, c);
                        }
                    }
                }
            }
        }

        protected void Resize()
        {
            Array.Resize<Vertex>(ref this.vertices, this.vertices.Length * 2);
            Array.Resize<Vertex>(ref this.sortedVertecies, this.sortedVertecies.Length * 2);
            Array.Resize<int>(ref this.sortedIndexes, this.sortedIndexes.Length * 2);
            Array.Resize<Texture2D>(ref this.textures, this.textures.Length * 2);
            Array.Resize<float>(ref this.renderOrder, this.renderOrder.Length * 2);

            this.GraphicsContext.BindVertexBuffer(this.vertexBuffer);
            this.vertexBuffer.SetData(this.vertices);
            this.SetupIndices(this.textures.Length);
        }

        private void BufferChar(Font fontUsed, ref Vector2 position, ref Vector2 origin, ref Vector2 scale, ref Vector2 cursor, bool mirror, int tint, float angle, float renderLayer, char c)
        {
            if (c == '\r') 
                return;
            else if (c == '\n')
            {
                cursor.Y += fontUsed.LineHeight * scale.Y;
                cursor.X = -origin.X * scale.X;
                return;
            }
            else if (c == '\t')
            {
                CharInfo ci = fontUsed[' '];
                cursor.X += ci.Advance * Font.TabToSpaceCount * scale.X;
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
                info = fontUsed.UnkownChar;
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
            dest.X = (cursor.X + (infoOffset.X) * scale.X);
            dest.Y = (cursor.Y + (infoOffset.Y) * scale.Y);
            dest.Z = dest.X + info.SrcRect.W * scale.X;
            dest.W = dest.Y + info.SrcRect.H * scale.Y;
            
            this.CreateRect(ref dest, ref src, ref position, tint, angle);

            cursor.X += info.Advance * scale.X;

            this.textures[this.elementCount] = fontUsed.Page;
            this.renderOrder[this.elementCount++] = renderLayer;

            return;
        }

        protected unsafe void CreateRect(ref Vector4 dest, ref Vector4 src, ref Vector2 pos, int tint, float angle)
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

                                         
        /// <summary>
        /// Adds a frame to be drawn by the batch.
        /// </summary>
        /// <param name="frame">The frame to draw.</param>
        /// <param name="position">The top left corner position of the frame.</param>
        /// <param name="color">The color of the frame.</param>
        /// <param name="origin">The origin of the frame.</param>
        /// <param name="scale">The scale of the frame.</param>
        /// <param name="rotation">The rotation of the frame.</param>
        /// <param name="mirror">A fralg indicating if the frame should be mirrored.</param>
        /// <param name="renderLayer">The layer in with to render the text. (used together with SortMode.RenderOrder to render objects in specific layers)</param>
        public void BufferFrame(Frame frame, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, float rotation = 0.0f, bool mirror = false, float renderLayer = 0)
        {
            if (this.elementCount >= this.textures.Length)
                this.Resize();
         
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
            dest.Z = dest.X + frame.SrcRect.W * scale.X;
            dest.W = dest.Y + frame.SrcRect.H * scale.Y;



            this.CreateRect(ref dest, ref src, ref position, color.ToArgb(), rotation);

            this.textures[this.elementCount] = frame.Texture2D;
            this.renderOrder[this.elementCount++] = renderLayer;

        }

        /// <summary>
        /// Adds a frame to be drawn by the batch.
        /// </summary>
        /// <param name="frame">The frame to draw.</param>
        /// <param name="position">The destination of the frame.</param>
        /// <param name="color">The color of the frame.</param>
        /// <param name="rotation">The rotation of the frame.</param>
        /// <param name="mirror"> </param>
        public void BufferFrame(Frame frame, Rect position, Color4 color, float rotation = 0, bool mirror = false)
        {
            this.BufferFrame(frame, new Vector2(position.X, position.Y), color, Vector2.Zero, new Vector2((position.W / frame.SrcRect.W), position.H / frame.SrcRect.H), rotation, mirror);
        }


        /// <summary>
        /// Adds a frame to be drawn by the batch.
        /// </summary>
        /// <param name="frame">The frame to draw.</param>
        /// <param name="position">The top left corner position of the frame.</param>
        /// <param name="color">The color of the frame.</param>
        public void BufferFrame(Frame frame, Vector2 position, Color4 color)
        {
            this.BufferFrame(frame, position, color, Vector2.Zero, Vector2.One, 0f, false);
        }

        /// <summary>
        /// Draws any objects added to the batch through the various Add methods.
        /// </summary>
        public void Draw(ref Matrix4 transformation, ShaderProgram effect = null, SortMode mode = SortMode.Deffered)
        {
            if (this.elementCount <= 0)
                return;

            if (effect == null)
                this.activeEffect = default_effect;
            else
                this.activeEffect = effect;

            this.PrepareShaderProgram(ref transformation, this.activeEffect);
                    
            if (mode == SortMode.Deffered)
            {
                this.Flush(this.vertices);
                this.RenderUnsorted();
            }
            else
            {
                this.Sort(mode);
                this.Flush(this.sortedVertecies);
                this.RenderSorted();
            }

            this.elementCount = 0;
            this.GraphicsContext.UseShaderProgram(null);
        }

        private unsafe void Sort(SortMode mode)
        {
            IComparer<int> comparer;

            if (mode == SortMode.RenderOrder)
                comparer = new RenderOrderSorter(this);
            else 
                comparer = new TextureSorter(this);
            
            fixed (int* ptr = this.sortedIndexes)
            {
                fixed (Vertex* queue = this.vertices, toSort = this.sortedVertecies)
                {
                    for (int i = 0; i < this.elementCount; i++)
                    {
                        ptr[i] = i;
                    }

                    Array.Sort<int>(this.sortedIndexes, 0, this.elementCount, comparer);

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

        private void Flush(Vertex[] vertices)
        {
            this.GraphicsContext.BindIndexBuffer(this.indexBuffer);
            this.vertexBuffer.SetSubData(this.vertices, 0, this.elementCount * 4);
        }

        private void RenderSorted()
        {
            this.GraphicsContext.BindIndexBuffer(indexBuffer);
            this.GraphicsContext.BindVertexBuffer(this.vertexBuffer);
            
            int toDraw = 0;
            var toUse = textures[this.sortedIndexes[0]];
            for (int i = 0; i < this.elementCount; i++)
            {
                var index = this.sortedIndexes[i];

                if (toUse != textures[index])
                {
                    this.GraphicsContext[0] = toUse;
                    this.GraphicsContext.DrawElements(BeginMode.Triangles, toDraw * Elements_Per_Square, DrawElementsType.UnsignedInt, (i - toDraw) * Elements_Per_Square * sizeof(uint));
                    toDraw = 0;

                    toUse = textures[index];
                }
                toDraw++;
            }

            this.GraphicsContext[0] = toUse;
            this.GraphicsContext.DrawElements(BeginMode.Triangles, toDraw * Elements_Per_Square, DrawElementsType.UnsignedInt, (elementCount - toDraw) * Elements_Per_Square * sizeof(uint));
        }

        private void RenderUnsorted()
        {
            this.GraphicsContext.BindIndexBuffer(indexBuffer);
            this.GraphicsContext.BindVertexBuffer(this.vertexBuffer);
            
            int toDraw = 0;
            var toUse = textures[0];
            for (int i = 0; i < this.elementCount; i++)
            {
                if (toUse != textures[i])
                {
                    this.GraphicsContext[0] = toUse;
                    this.GraphicsContext.DrawElements(BeginMode.Triangles, toDraw * Elements_Per_Square, DrawElementsType.UnsignedInt, (i - toDraw) * Elements_Per_Square * sizeof(uint));
                    toDraw = 0;

                    toUse = textures[i];
                }
                toDraw++;
            }

            this.GraphicsContext[0] = toUse;
            this.GraphicsContext.DrawElements(BeginMode.Triangles, toDraw * Elements_Per_Square, DrawElementsType.UnsignedInt, (elementCount - toDraw) * Elements_Per_Square * sizeof(uint));
        }

        private class RenderOrderSorter : IComparer<int>
        {
            private readonly SpriteBuffer parent;

            public RenderOrderSorter(SpriteBuffer batch)
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
            private readonly SpriteBuffer parent;

            public TextureSorter(SpriteBuffer batch)
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
    }
}