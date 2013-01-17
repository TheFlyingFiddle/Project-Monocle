using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Monocle.Graphics;
using OpenTK.Graphics.OpenGL;


namespace Monocle.Content.Serialization
{
    [Processor(typeof(Bitmap), true)]
    class TextureProcessor : Processor<Bitmap, Texture2D>
    {
        public TextureProcessor()
            : this(System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
        { }

        public TextureProcessor(System.Drawing.Imaging.PixelFormat format)
        {
            this.Format = format;
        }

        public override Texture2D Process(Bitmap bitmap, IResourceContext context)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, Format);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


            bitmap.Dispose();

            return new Texture2D(id, data.Width, data.Height);
        }

        public System.Drawing.Imaging.PixelFormat Format
        {
            get;
            set;
        }
    }
}
