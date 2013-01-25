using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.IO;
using Monocle.Content.Serialization;
using OpenTK.Graphics;

namespace Monocle.Graphics
{
    public class Texture2D : IComparable<Texture2D>
    {
        public readonly IGraphicsContext GraphicsContext;
        public readonly int Handle;
        public readonly int Width, Height;
        
        internal Texture2D(IGraphicsContext context, int openglID, int width, int height)
        {
            this.GraphicsContext = context;
            this.Handle = openglID;
            this.Width = width;
            this.Height = height;
        }

        public Rect Bounds 
        {
            get { return new Rect(0, 0, Width, Height); } 
        }


        public static Texture2D LoadTexture(IGraphicsContext context, Stream stream)
        {
            Bitmap bitmap = new Bitmap(stream);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            int id = context.GenTexture();
            context.BindTexture(TextureTarget.Texture2D, id);

            context.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, false,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            context.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture2D(context, id, data.Width, data.Height);
        }
        
        public void Destroy()
        {
            this.GraphicsContext.DeleteTexture(Handle);
        }

        public override bool Equals(object obj)
        {
            if (obj is Texture2D)
            {
                return this.Handle == ((Texture2D)obj).Handle;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Handle;
        }

        public void GetImageData<T>(T[] data) where T : struct
        {
            this.GraphicsContext.GetTexImage<T>(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data);
        }


        [TypeWriter]
        public class Texture2DWriter : TypeWriter<Texture2D>
        {
            public override void WriteType(Texture2D toWrite, IWriter writer)
            {
                int[] array = new int[toWrite.Width * toWrite.Height];
                toWrite.GraphicsContext.BindTexture(TextureTarget.Texture2D, toWrite.Handle);
                toWrite.GraphicsContext.GetTexImage<int>(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, array);
                writer.Write(toWrite.Width);
                writer.Write(array);
            }
        }

        [TypeReader]
        public class Texture2DReader : TypeReader<Texture2D>
        {
            public override Texture2D Read(IReader reader)
            {
                /*int width = reader.ReadInt32();
                int[] data = reader.Read<int[]>();

                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, data.Length / width, 0,
                              OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return new Texture2D(id, width, data.Length / width);*/

                throw new NotImplementedException();
            }
        }

        public int CompareTo(Texture2D other)
        {
            return other.Handle - this.Handle;
        }
    }
}