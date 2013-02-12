using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Monocle.Graphics
{
    class FrameBuffer
    {
        public readonly int Handle;
        private Texture2D texture0;

        public FrameBuffer(int handle, Texture2D texture0)
        {
            this.Handle = handle;
            this.texture0 = texture0;
        }


        public Texture2D Texture0
        {
            get { return this.texture0; }
        }



        public static FrameBuffer Create(int width, int height, IGraphicsContext context)
        {
            int textureHandle = context.GenTexture();
            context.BindTexture(TextureTarget.Texture2D, textureHandle);

            context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            context.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, false, PixelFormat.Bgra, PixelType.UnsignedByte, (IntPtr)null);

            var texture = new Texture2D(context, textureHandle, width, height);

            int handle;
            context.GenFrameBuffers(1, out handle);
            context.BindFrameBuffer(FramebufferTarget.Framebuffer, handle);

            context.FrameBufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle, 0);

            context.BindFrameBuffer(FramebufferTarget.Framebuffer, 0);
            return new FrameBuffer(handle, texture);
        }
    }
}
