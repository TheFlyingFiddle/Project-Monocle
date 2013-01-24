using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Monocle.Graphics
{
    public class ShortIndexBuffer
    {
        public readonly IGraphicsContext context;

        private readonly BufferUsageHint hint;
        internal readonly int Handle;
        
        public ShortIndexBuffer(IGraphicsContext context, BufferUsageHint hint)
        {
            this.context = context;
            this.hint = hint;
            context.GenBuffers(1, out Handle);
        }

        public void SetData(ushort[] data)
        {
            context.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(data.Length * 2), data, hint);
        }

        public void SetSubData(ushort[] data, int offset)
        {
            context.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 2), (IntPtr)(data.Length * 2), data);
        }

        public void SetSubData(ushort[] data, int offset, int length)
        {
            context.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 2), (IntPtr)(length * 2), data);
        }
    }

    public class IntIndexBuffer
    {
        public readonly IGraphicsContext context;
        private readonly BufferUsageHint hint;
        internal readonly int Handle;

        public IntIndexBuffer(IGraphicsContext context, BufferUsageHint hint)
        {
            this.context = context;
            this.hint = hint;
            context.GenBuffers(1, out Handle);
        }

        public void SetData(int[] data)
        {
            context.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(data.Length * 4), data, hint);
        }

        public void SetSubData(int[] data, int offset)
        {
            context.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 4), (IntPtr)(data.Length * 4), data);
        }

        public void SetSubData(int[] data, int offset, int length)
        {
            context.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 4), (IntPtr)(length * 4), data);
        }

    }
}