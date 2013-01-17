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
    public class Texture2D 
    {
        int openglID;
        public readonly int Width, Height;

        internal Texture2D(int openglID, int width, int height)
        {
            this.openglID = openglID;
            this.Width = width;
            this.Height = height;
        }

        public Rect Bounds 
        {
            get { return new Rect(0, 0, Width, Height); } 
        }


        public static Texture2D LoadTexture(Stream stream)
        {
            Bitmap bitmap = new Bitmap(stream);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture2D(id, data.Width, data.Height);
        }


        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, this.openglID);
        }

        public void Destroy()
        {
            GL.DeleteTexture(openglID);
        }

        public override bool Equals(object obj)
        {
            if (obj is Texture2D)
            {
                return this.openglID == ((Texture2D)obj).openglID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.openglID;
        }

        public void GetImageData<T>(T[] data) where T : struct
        {
            
            GL.GetTexImage<T>(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data);

            if (GL.GetError() != ErrorCode.NoError)
            {
                throw new ArgumentException("The data must be large enough to store the entire texture!");
            }
        }


        [TypeWriter]
        public class Texture2DWriter : TypeWriter<Texture2D>
        {
            public override void WriteType(Texture2D toWrite, IWriter writer)
            {
                int[] array = new int[toWrite.Width * toWrite.Height];
                GL.GetTexImage<int>(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, array);
                writer.Write(toWrite.Width);
                writer.Write(array);
            }
        }

        [TypeReader]
        public class Texture2DReader : TypeReader<Texture2D>
        {
            public override Texture2D Read(IReader reader)
            {
                int width = reader.ReadInt32();
                int[] data = reader.Read<int[]>();

                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, data.Length / width, 0,
                              OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return new Texture2D(id, width, data.Length / width);
            }
        }
    }
}